using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    // 使用ISO15765协议打包
    static class DcmTpProtocalISO15765Packer
    {
        public const byte SingleFramePci = 0x00;
        public const byte FirstFramePci = 0x10;
        public const byte ConsecutiveFramePci = 0x20;
        public const byte FlowControlFramePci = 0x30;
        public const byte FillByte = 0xAA;

        public const int MaxSingleFrameDataLen = 7;
        public const int FirstFrameDataLen = 6;

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="appData">应用数据</param>
        /// <returns>网络报文包</returns>
        public static List<byte[]> Pack(List<byte> appData)
        {
            if (appData.Count <= MaxSingleFrameDataLen)
            {
                return PackSingleFrame(appData);
            }
            else
            {
                return PackMultiFrame(appData);
            }
        }

        // 打包多帧
        private static List<byte[]> PackMultiFrame(List<byte> appData)
        {
            List<byte[]> result = new List<byte[]>();
            byte[] frame = new byte[8];
            int offset = 0;

            //设置第一帧
            frame[0] = (byte)(FirstFramePci + ((appData.Count >> 8) & 0x0f));
            frame[1] = (byte)(appData.Count);
            appData.CopyTo(offset, frame, 2, FirstFrameDataLen);
            result.Add(frame);

            offset += FirstFrameDataLen;

            //装非最后一帧
            int contFrameCount = (appData.Count - FirstFrameDataLen) / MaxSingleFrameDataLen;
            int lastFrameBytes = (appData.Count - FirstFrameDataLen) % MaxSingleFrameDataLen;
            int serialNumber = 1;

            for (int i = 0; i < contFrameCount; i++)
            {
                frame = new byte[8];
                frame[0] = (byte)(ConsecutiveFramePci + serialNumber);
                serialNumber = (serialNumber + 1) % 0x10;

                appData.CopyTo(offset, frame, 1, MaxSingleFrameDataLen);
                offset += MaxSingleFrameDataLen;
                result.Add(frame);
            }

            //装最后一帧
            if (lastFrameBytes > 0)
            {
                frame = new byte[8];
                frame[0] = (byte)(ConsecutiveFramePci + serialNumber); //sid
                appData.CopyTo(offset, frame, 1, lastFrameBytes);
                for (int i = lastFrameBytes+1; i < 8; i++)
                {
                    frame[i] = FillByte;
                }
                result.Add(frame);
            }

            return result;
        }

        // 打包单帧
        private static List<byte[]> PackSingleFrame(List<byte> appData)
        {
            byte[] frame = new byte[8];
            frame[0] = (byte)(SingleFramePci + appData.Count);
            appData.CopyTo(frame, 1);

            // 填充数据
            for (int i = 1 + appData.Count; i < 8; i++)
            {
                frame[i] = FillByte;
            }

            List<byte[]> result = new List<byte[]>();
            result.Add(frame);
            return result;
        }

        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="pack">帧报文</param>
        /// <returns>应用数据</returns>
        public static List<byte> Unpack(List<byte[]> pack)
        {
            Debug.Assert(pack.Count > 0);
            if (pack.Count == 1)
            {
                return UnpackSingleFrame(pack);
            }
            else
            {
                return UnpackMultiFrame(pack);
            }
        }

        // 解包多帧
        private static List<byte> UnpackMultiFrame(List<byte[]> pack)
        {
            List<byte> result = new List<byte>();
            int len = ((pack[0][0] & 0x0f) << 8) + pack[0][1];
            int offset = 0;

            // 拷贝第一帧
            for (int i = 2; i < 8; i++)
            {
                result.Add(pack[offset][i]);
            }
            offset ++;

            // 解包非最后一帧
            int contFrameCount = (len - FirstFrameDataLen) / MaxSingleFrameDataLen;
            int lastFrameBytes = (len - FirstFrameDataLen) % MaxSingleFrameDataLen;

            for (int i = 0; i < contFrameCount; i++)
            {
                for (int j = 1; j < 8; j++)
                {
                    result.Add(pack[offset][j]);
                }
                ++offset;
            }

            // 解包最后一帧
            if (lastFrameBytes > 0)
            {
                Debug.Assert(offset == pack.Count - 1);
                for (int i = 1; i <= lastFrameBytes; i++)
                {
                    result.Add(pack[offset][i]);
                }
            }

            return result;
        }

        // 解包单帧
        private static List<byte> UnpackSingleFrame(List<byte[]> pack)
        {
            byte[] singleFrame = pack[0];
            int len = singleFrame[0] & 0x0f;

            List<byte> result = new List<byte>();
            for (int i = 0; i < len; i++)
            {
                result.Add(singleFrame[i + 1]);
            }

            return result;
        }

        public static bool IsSingleFrame(byte pci)
        {
            return SingleFramePci == (byte)(pci & 0xf0);
        }

        public static bool IsFirstFrame(byte pci)
        {
            return FirstFramePci == (byte)(pci & 0xf0);
        }

        public static bool IsFlowControlFrame(byte pci)
        {
            return FlowControlFramePci == (byte)(pci & 0xf0);
        }

        public static bool IsConsecutiveFrame(byte pci)
        {
            return ConsecutiveFramePci == (byte)(pci & 0xf0);
        }
    }

    // 使用ISO15765协议发送报文
    class DcmTpProtocalISO15765Sender
    {
        private CanIf canIf = CanIf.Instance();
        private Stopwatch stopwatch = new Stopwatch();

        public const int DefaultBlockSize = 10;
        public const int DefaultStMin = 10;

        public enum WorkStateEnum
        {
            Odle,
            WaitFlowControlFrame,
            SendConsecutiveFrame,
        }

        enum FlowControlStateEnum
        {
            Continue,
            Wait,
            Overflow
        }

        public WorkStateEnum WorkState { get; set; }
        public List<byte[]> Frames { get; set; }
        public int CanId { get; set; }
        public int FrameIndex { get; set; }

        private byte blockSize;
        public byte BlockSize
        {
            get { return blockSize; }
            set { blockSize = value; }
        }

        public byte CurrentBlockSize { get; set; }
        

        private byte stMin;
        public byte StMin
        {
            get {return stMin; }
            set { stMin = value; }
        }

        public enum SendResult
        {
            Ok,      //发送完成
            Fail,    //发送失败
            Working  //正在发送
        }

        public SendResult Send(int canId, List<byte[]> frames)
        {
            // 一次只能执行一次事务
            if ((WorkState != WorkStateEnum.Odle) && (frames != Frames))
            {
                return SendResult.Fail;
            }

            switch (WorkState)
            {
                case WorkStateEnum.Odle:
                    return SendSFOrFF(canId, frames);
                case WorkStateEnum.WaitFlowControlFrame:
                    return ReceiveFlowControlFrame();
                case WorkStateEnum.SendConsecutiveFrame:
                    return SendConsecutiveFrame();
                default:
                    throw new ArgumentException("DcmTpProtocalISO15765Sender:Unkno" +
                        "wn Work State: " + WorkState.ToString());
            }
        }

        public void Reset()
        {
            WorkState = WorkStateEnum.Odle;
            Frames = null;
            FrameIndex = -1;
            CanId = -1;
            BlockSize = DefaultBlockSize;
            StMin = DefaultStMin;
            stopwatch.Reset();
        }

        // 发送连续帧
        private SendResult SendConsecutiveFrame()
        {
            if (BlockSize == 0)
            {
                return SendConsecutiveFrameWithoutBlockSize();
            }
            else if (CurrentBlockSize < BlockSize)
            {
                return SendConsecutiveFrameWithBlockSize();
            }
            return SendResult.Working;
        }

        private SendResult SendConsecutiveFrameWithBlockSize()
        {
            if (stopwatch.ElapsedMilliseconds > StMin)
            {
                if (!SendMessage(CanId, Frames[FrameIndex++]))
                {
                    return SendFailed;
                }
                if (FrameIndex == Frames.Count)
                {
                    return SendSuccess;
                }
                else
                {
                    stopwatch.Restart();
                    if (++CurrentBlockSize >= BlockSize)
                    {
                        WorkState = WorkStateEnum.WaitFlowControlFrame;
                    }
                }
            }
            return SendResult.Working;
        }

        private SendResult SendConsecutiveFrameWithoutBlockSize()
        {
            if (stopwatch.ElapsedMilliseconds > StMin)
            {
                if (!SendMessage(CanId, Frames[FrameIndex++]))
                {
                    return SendFailed;
                }

                stopwatch.Restart();
                if (FrameIndex == Frames.Count)
                {
                    return SendSuccess;
                }
            }
            return SendResult.Working;
        }

        // 接收流控制帧
        private SendResult ReceiveFlowControlFrame()
        {
            byte flowControl = 0;
            if (DcmTpProtocalISO15765Receiver.Instance().ReceiveFlowControlFrame(ref flowControl,
                ref blockSize, ref stMin))
            {
                FlowControlStateEnum flowControlState =
                    (FlowControlStateEnum)flowControl;
                switch (flowControlState)
                {
                    case FlowControlStateEnum.Wait:
                        WorkState = WorkStateEnum.WaitFlowControlFrame;
                        break;
                    case FlowControlStateEnum.Overflow:
                        return SendFailed;
                    case FlowControlStateEnum.Continue:
                        stopwatch.Restart();
                        WorkState = WorkStateEnum.SendConsecutiveFrame;
                        CurrentBlockSize = 0;
                        break;
                }
                return SendResult.Working;
            }
            return SendResult.Working;
        }

        // 发送单帧或者第一帧
        private SendResult SendSFOrFF(int canId, List<byte[]> frames)
        {
            Debug.Assert(frames.Count > 0, "DcmTpProtocalISO15765Sender: Invalid SFOrFF");

            byte pci = frames[0][0];
            Debug.Assert(DcmTpProtocalISO15765Packer.IsSingleFrame(pci)
                || DcmTpProtocalISO15765Packer.IsFirstFrame(pci));
            var result = SendMessage(canId, frames[0]);

            if (DcmTpProtocalISO15765Packer.IsSingleFrame(pci)) // 单帧
            {
                Reset();
                return result ? SendResult.Ok : SendResult.Fail;
            }
            else if (DcmTpProtocalISO15765Packer.IsFirstFrame(pci)) //多帧
            {
                if (!result)
                {
                    return SendFailed;
                }
                else
                {
                    // 进入等待流控制帧状态
                    CanId = canId;
                    Frames = frames;
                    WorkState = WorkStateEnum.WaitFlowControlFrame;
                    FrameIndex = 1;
                    return SendResult.Working;
                }
            }
            else
            {
                return SendFailed;
            }
        }

        private SendResult SendFailed
        {
            get
            {
                Reset();
                return SendResult.Fail;
            }
        }

        private SendResult SendSuccess
        {
            get
            {
                Reset();
                return SendResult.Ok;
            }
        }

        // 发送报文
        private bool SendMessage(int canId, byte[] frame)
        {
            Message message = new Message();
            message.Id = canId;
            frame.CopyTo(message.Data, 0);
            message.DataLen = 8;
            return canIf.SendMessage(message);
        }

        #region Singleton implement
        private static DcmTpProtocalISO15765Sender instance;
        private static object syncRoot = new object();

        private DcmTpProtocalISO15765Sender()
        {
            Reset();
        }

        public static DcmTpProtocalISO15765Sender Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new DcmTpProtocalISO15765Sender();
                    }
                }
            }
            return instance;
        }

        internal bool SendFlowControlFrame(int hostId, byte flowControlState,
            byte blockSize, byte stMin)
        {
            Message message = new Message();
            message.Id = hostId;
            message.DataLen = 8;

            message.Data[0] = (byte)(DcmTpProtocalISO15765Packer.FlowControlFramePci
                + (flowControlState & 0x0f));
            message.Data[1] = blockSize;
            message.Data[2] = stMin;

            for (int i = 3; i < 8; i++)
            {
                message.Data[i] = DcmTpProtocalISO15765Packer.FillByte;
            }

            return canIf.SendMessage(message);
        }
        #endregion
    }

    // 使用ISO15765协议接收报文
    class DcmTpProtocalISO15765Receiver
    {
        private CanIf canIf = CanIf.Instance();

        public const int FlowControlState = 0;
        public const int BlockSize = 0;
        public const int StMin = 1;

        public enum WorkStateEnum
        {
            Idle,
            SendFlowControlFrame,
            ReceiveConsecutiveFrame
        }

        public WorkStateEnum WorkState { get; set; }
        public int LeftConsecutiveFrameCount { get; set; }
        public int HostId { get; set; }
        public List<byte[]> Frames { get; set; }

        public void Reset()
        {
            WorkState = WorkStateEnum.Idle;
            LeftConsecutiveFrameCount = 0;
            Frames = null;
        }

        public enum ReceiveResult
        {
            Ok,
            Fail,
            Working
        }

        public ReceiveResult Receive(int hostId, List<byte[]> frames)
        {
            switch (WorkState)
            {
                case WorkStateEnum.Idle:
                    return ReceiveSFOrFF(hostId, frames);
                case WorkStateEnum.SendFlowControlFrame:
                    return SendFlowControl();
                case WorkStateEnum.ReceiveConsecutiveFrame:
                    return ReceiveConsecutiveFrame();
                default:
                    throw new InvalidOperationException("DcmTpProtocalISO15765Receiver: Unknown " +
                        "Work State: " + WorkState.ToString());
            }
        }

        private ReceiveResult ReceiveConsecutiveFrame()
        {
            Message message = ReceiveMessage();
            if (message != null)
            {
                Frames.Add(message.Data);
                if (--LeftConsecutiveFrameCount <= 0)
                {
                    return ReceiveSuccess;
                }
                else
                {
                    return ReceiveResult.Working;
                }
            }
            return ReceiveResult.Working;
        }

        private ReceiveResult SendFlowControl()
        {
            if (!DcmTpProtocalISO15765Sender.Instance()
                .SendFlowControlFrame(HostId, FlowControlState, BlockSize, StMin))
            {
                return ReceiveFailed;
            }
            WorkState = WorkStateEnum.ReceiveConsecutiveFrame;
            return ReceiveResult.Working;
        }

        private ReceiveResult ReceiveSFOrFF(int hostId, List<byte[]> frames)
        {
            Message message = ReceiveMessage();

            if (message != null)
            {
                byte pci = message.Data[0];
                Debug.Assert(DcmTpProtocalISO15765Packer.IsSingleFrame(pci) ||
                    DcmTpProtocalISO15765Packer.IsFirstFrame(pci), "DcmTpProtocalISO15765Receiver" +
                    "->ReceiveSFOrFF: Not Single Frame Or First Frame");

                frames.Clear();
                frames.Add(message.Data);
                if (DcmTpProtocalISO15765Packer.IsSingleFrame(pci))
                {
                    return ReceiveSuccess;
                }
                else if (DcmTpProtocalISO15765Packer.IsFirstFrame(pci))
                {
                    HostId = hostId;
                    int totalLen = (((message.Data[0] & 0x0f) << 8) + message.Data[1]);
                    LeftConsecutiveFrameCount = (totalLen - 6) / 7
                        + ((((totalLen - 6) % 7) != 0) ? 1 : 0);
                    Frames = frames;
                    WorkState = WorkStateEnum.SendFlowControlFrame;

                    //立即发送流控制帧
                    SendFlowControl();
                }
            }
            return ReceiveResult.Working;
        }

        public ReceiveResult ReceiveSuccess
        {
            get
            {
                Reset();
                return ReceiveResult.Ok;
            }
        }

        public ReceiveResult ReceiveFailed
        {
            get
            {
                Reset();
                return ReceiveResult.Fail;
            }
        }

        public bool ReceiveFlowControlFrame(ref byte flowState, ref byte blockSize, ref byte stMin)
        {
            Message message = ReceiveMessage();
            
            if (message != null)
            {
                if (DcmTpProtocalISO15765Packer.IsFlowControlFrame(message.Data[0]))
                {
                    flowState = (byte)(message.Data[0] & 0x0f);
                    blockSize = message.Data[1];
                    stMin = message.Data[2];
                    return true;
                }
                else
                {
                    Debug.Assert(false, "DcmTpProtocalISO15765Receiver: Filter setting error");
                }
            }
            return false;
        }

        private Message ReceiveMessage()
        {
            return canIf.ReceiveMessage(0);
        }

        #region Singleton implement
        private static DcmTpProtocalISO15765Receiver instance;
        private static object syncRoot = new object();

        private DcmTpProtocalISO15765Receiver()
        {
            Reset();
        }

        public static DcmTpProtocalISO15765Receiver Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new DcmTpProtocalISO15765Receiver();
                    }
                }
            }
            return instance;
        }
        #endregion
    }

    // 实现了ISO15765协议
    class DcmTpProtocalISO15765 : DcmTpProtocal
    {
        public enum WorkStateEnum
        {
            Idle = 0,
            Send,
            Receive
        }
        public WorkStateEnum WorkState { get; set; }

        public DcmTpHandleResult DcmTpHandleResult { get; set; }

        public List<byte[]> FramesToSend { get; set; }
        public List<byte[]> FramesReceived { get; set; }
        public List<byte> AppDataReceived { get; set; }
        public int CanId { get; set; }
        public int HostId { get; set; }

        // 抑制肯定响应
        public bool SuppressResp { get; set; }

        private DcmTpProtocalISO15765Sender sender = DcmTpProtocalISO15765Sender.Instance();
        private DcmTpProtocalISO15765Receiver receiver = DcmTpProtocalISO15765Receiver.Instance();

        private Stopwatch stopwatch = new Stopwatch();
        public const int ReceiveTimeout = 1 * 1000; //超时时间1s

        private const bool DefaultReceiveTimeoutCheck = true;
        public bool ReceiveTimeoutCheck { get; set; }

        public void Reset()
        {
            WorkState = WorkStateEnum.Idle;
            FramesToSend = null;
            FramesReceived = null;
            stopwatch.Reset();

            //将sender, receiver都Reset，放置因为两者出现错误而终止
            sender.Reset();
            receiver.Reset();
        }

        public bool Execute(int canId, int hostId, List<byte> appData)
        {
            switch (WorkState)
            {
                case WorkStateEnum.Idle:
                    InitFramesToSend(appData);
                    WorkState = WorkStateEnum.Send;
                    CanId = canId;
                    HostId = hostId;

                    if (appData.Count > 1)
                    {
                        SuppressResp = ((byte)0x80 & appData[1]) != 0;
                    }
                    else
                    {
                        SuppressResp = false;
                    }
                    return false;
                case WorkStateEnum.Send:
                    return Send();
                case WorkStateEnum.Receive:
                    return Receive();
                default:
                    throw new InvalidOperationException("DcmTpProtocalISO15765.Send: Unknown work " +
                        "state -> " + WorkState.ToString());
            }
        }

        private bool Receive()
        {
            var result = receiver.Receive(CanId, FramesReceived);
            bool ret = false;
            switch (result)
            {
                case DcmTpProtocalISO15765Receiver.ReceiveResult.Ok:
                    DcmTpHandleResult = DcmTpHandleResult.OK;
                    // 解包数据
                    AppDataReceived = DcmTpProtocalISO15765Packer.Unpack(FramesReceived);
                    Reset();
                    ret = true;
                    break;
                case DcmTpProtocalISO15765Receiver.ReceiveResult.Working:
                    ret = false;
                    break;
                case DcmTpProtocalISO15765Receiver.ReceiveResult.Fail:
                    DcmTpHandleResult = DcmTpHandleResult.FailWithReceive;
                    Reset();
                    ret = true;
                    break;
                default:
                    throw new InvalidOperationException("DcmTpProtocalISO15765.Send: Unknown work " +
                        "state -> " + WorkState.ToString());
            }

            if (!ret)
            {
                // 检测接收超时
                if (ReceiveTimeoutCheck && (stopwatch.ElapsedMilliseconds >= ReceiveTimeout))
                {
                    DcmTpHandleResult = DcmTpHandleResult.FailWithReceiveTimeout;
                    Reset();
                    return true;
                }
            }

            return ret;
        }

        private bool Send()
        {
            var result = sender.Send(CanId, FramesToSend);
            switch (result)
            {
                case DcmTpProtocalISO15765Sender.SendResult.Ok:
                    return OnSendOk();
                case DcmTpProtocalISO15765Sender.SendResult.Working:
                    return false;
                case DcmTpProtocalISO15765Sender.SendResult.Fail:
                    DcmTpHandleResult = DcmTpHandleResult.FailWithSend;
                    Reset();
                    return true;
                default:
                    throw new InvalidOperationException("DcmTpProtocalISO15765.Send: Unknown work " +
                        "state -> " + WorkState.ToString());
            }
        }

        private bool OnSendOk()
        {
            //如果是功能请求，并且肯定响应进制位被设置
            if ((DcmService.FunctionRequestId == CanId) && SuppressResp)
            {
                DcmTpHandleResult = DcmTpHandleResult.OKWithSupporessResp;
                Reset();
                AppDataReceived = new List<byte>();
                return true;
            }
            else
            {
                FramesReceived = new List<byte[]>();
                WorkState = WorkStateEnum.Receive;
                stopwatch.Restart();
            }
            return false;
        }

        private void InitFramesToSend(List<byte> appData)
        {
            FramesToSend = DcmTpProtocalISO15765Packer.Pack(appData);
        }

        public DcmTpHandleResult GetHandleResult()
        {
            return DcmTpHandleResult;
        }

        public List<byte> GetReceivedData()
        {
            List<byte> result = AppDataReceived;
            AppDataReceived = null;
            return result;
        }

        #region Singleton implement
        private static DcmTpProtocalISO15765 instance;
        private static object syncRoot = new object();

        private DcmTpProtocalISO15765()
        {
            Reset();

            ReceiveTimeoutCheck = DefaultReceiveTimeoutCheck;
        }

        public static DcmTpProtocalISO15765 Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new DcmTpProtocalISO15765();
                    }
                }
            }
            return instance;
        }

        #endregion
    }
}

//#define DEBUG_ENABLED

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DcmService
{
    public static class DcmService
    {
        // 使用定时器来模拟报文输入
        private static DcmApp dcmApp = DcmApp.Instance();

#if DEBUG_ENABLED
        private static Timer timer;
        private static Random random;
#endif

        static DcmService()
        {
#if DEBUG_ENABLED
            timer = new Timer();
            timer.Interval = 500;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;

            random = new Random();
#endif
        }

        /// <summary>
        /// 发送物理请求
        /// </summary>
        /// <param name="package">要发送的数据</param>
        public static void SendPhysicalRequest(List<byte> package)
        {
            if (CanIsOpened())
            {
                dcmApp.SendRequest(PhysicalRequestId, package);
            }
        }

        /// <summary>
        /// 发送功能请求
        /// </summary>
        /// <param name="package">要发送的数据</param>
        public static void SendFunctionalRequest(List<byte> package)
        {
            if (CanIsOpened())
            {
                dcmApp.SendRequest(FunctionRequestId, package);
            }
        }

        private static int physicalRequestId;

        /// <summary>
        /// 物理请求Id
        /// </summary>
        public static int PhysicalRequestId
        {
            get { return physicalRequestId; }
            set
            {
                if (IsValidCanId(value))
                {
                    physicalRequestId = value;

                    //通知App
                }
            }
        }

        public static bool OpenCan()
        {
            ECAN.INIT_CONFIG initConfig = new ECAN.INIT_CONFIG();

            initConfig.AccCode = 
                UsbCanUtil.CalculateStandardFrameAccCode((uint)DcmService.ResponseId);

            initConfig.AccMask = 0x0;
            initConfig.Reserved = 0x0;
            initConfig.Filter = 0x00;
            initConfig.Timing0 = 0x0;
            initConfig.Timing1 = 0x1c;
            initConfig.Mode = 0x00;

            return UsbCanUtil.Instance().Open(UsbCanUtil.DefaultDeviceType, ref initConfig);
        }

        public static void CloseCan()
        {
            UsbCanUtil.Instance().Close();
        }

        public static bool CanIsOpened()
        {
            return UsbCanUtil.Instance().IsOpened;
        }

        private static int functionRequestId;

        /// <summary>
        /// 功能请求Id
        /// </summary>
        public static int FunctionRequestId
        {
            get { return functionRequestId; }
            set
            {
                if (IsValidCanId(value))
                {
                    functionRequestId = value;

                    // 通知App
                }
            }
        }

        private static int responseId;
        public static int ResponseId
        {
            get { return responseId; }
            set
            {
                if (IsValidCanId(value))
                {
                    responseId = value;

                    // 这儿需要重新初始化Can
                    if (CanIsOpened())
                    {
                        CloseCan();
                        OpenCan();
                    }
                }
            }
        }

        public static uint CanTickPeriod
        {
            get
            {
                return dcmApp.CanTickPeriod;
            }
            set
            {
                dcmApp.CanTickPeriod = value * 1000;
            }
        }

        public static bool CanTickEnabled
        {
            get { return dcmApp.CanTickEnable; }
            set { dcmApp.CanTickEnable = value; }
        }

        public static bool SuppressResponse
        {
            get { return dcmApp.SuppressResponse; }
            set { dcmApp.SuppressResponse = value; }
        }

        public static bool IsValidCanId(int canId)
        {
            return (canId >= 0) && (canId <= 0x7ff);
        }

        /// <summary>
        /// 解析诊断数据事件
        /// </summary>
        public static event ParsingDataIncommingEventHanlder ParsingDataIncomming;

        public static void FireParsingDataIncommingEvent(ParsingDataIncommingEventArgs args)
        {
            ParsingDataIncomming?.Invoke(args);
        }

        /// <summary>
        /// 一帧传输数据传输过来
        /// </summary>
        public static event RawDataIncommingEventHandler RawDataIncomming;

        public static void FireRawDataIncommingEvent(RawDataIncommingEventArgs args)
        {
            RawDataIncomming?.Invoke(args);
        }

        public static event SessionChangedEventHandler SessionChanged;
        public static void FireSessionChangedEvent(SessionChangedEventArgs args)
        {
            SessionChanged?.Invoke(args);
        }

#if DEBUG_ENABLED
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            #region Debug Raw Data Incomming event
            RawDataIncommingEventArgs rawArgs = new RawDataIncommingEventArgs();
            rawArgs.CanId = random.Next(0x7ff);

            Func<List<byte>> autoRandomPackage = () =>
            {
                List<byte> result = new List<byte>();
                int packageLen = random.Next(8);
                packageLen = Math.Max(1, packageLen);

                for (int i = 0; i < packageLen; i++)
                {
                    result.Add((byte)random.Next(0xff));
                }
                return result;
            };
            rawArgs.Data = autoRandomPackage();

            RawDataIncomming?.Invoke(rawArgs);
            #endregion

            #region Debug Parsing incomming event
            ParsingDataIncommingEventArgs parsingArgs = new ParsingDataIncommingEventArgs();
            parsingArgs.EntryList = new List<KeyValuePair<string, string>>();
            int argsCount = random.Next(0x30);
            argsCount = Math.Max(argsCount, 1);
            for (int i = 0; i < argsCount; i++)
            {
                parsingArgs.EntryList.Add(new KeyValuePair<string, string>(
                    string.Format("Key={0}", random.Next(0x10)),
                    string.Format("Value={0}", random.Next(0x10))));
            }
            ParsingDataIncomming?.Invoke(parsingArgs);
            #endregion
        }
#endif
    }
}

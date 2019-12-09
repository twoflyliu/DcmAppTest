using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    /// <summary>
    /// 主要负责如下职责：
    /// 1. 发送数据
    /// 2. 接收数据
    /// 3. 当数据发送完成后，通知DcmService
    /// 4. 当数据接收完毕后，通知DcmService
    /// </summary>
    class CanIf
    {
        private CanDriver canDriver = CanDriver.Instance();

        /// <summary>
        /// 发送报文
        /// </summary>
        /// <param name="message">要发送的报文</param>
        /// <returns>报文发送成功与否</returns>
        public bool SendMessage(Message message)
        {
            bool ok = canDriver.SendMessage(message);
            NotifyRawDataIncomming(ok, 
                RawDataIncommingEventArgs.OperationEum.Send,
                message);
            return ok;
        }

        /// <summary>
        /// 接收报文
        /// </summary>
        /// <param name="timeout">接收超时时间，单位ms</param>
        /// <returns>返回接收到的报文</returns>
        public Message ReceiveMessage(int timeout)
        {
            Message message = canDriver.ReceiveMessage(timeout);
            
            if (message != null)
            {
                NotifyRawDataIncomming(true, 
                    RawDataIncommingEventArgs.OperationEum.Receive,
                    message);
            }

            return message;
        }

        private void NotifyRawDataIncomming(bool ok,
            RawDataIncommingEventArgs.OperationEum operation, Message message)
        {
            RawDataIncommingEventArgs args = new RawDataIncommingEventArgs();
            args.Ok = ok;
            args.CanId = message.Id;
            args.Operation = operation;

            args.Data = new List<byte>();
            for (int i = 0; i < message.DataLen; i++)
            {
                args.Data.Add(message.Data[i]);
            }

            DcmService.FireRawDataIncommingEvent(args);
        }

        #region Singleton implement
        private static CanIf instance;
        private static object syncRoot = new object();

        public static CanIf Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CanIf();
                    }
                }
            }
            return instance;
        }

        private CanIf() { }
        #endregion
    }
}

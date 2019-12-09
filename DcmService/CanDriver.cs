using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    /// <summary>
    /// 1. 负责发送数据
    /// 2. 负责接收数据
    /// </summary>
    class CanDriver
    {
        public bool SendMessage(Message message)
        {
            ECAN.CAN_OBJ obj = new ECAN.CAN_OBJ();

            obj.ID = (uint)message.Id;
            obj.TimeStamp = 0;
            obj.TimeFlag = 0;
            obj.SendType = 0;
            obj.RemoteFlag = 0;
            obj.ExternFlag = 0;
            obj.DataLen = 8;

            obj.Data = new byte[8];
            message.Data.CopyTo(obj.Data, 0);            

            obj.Reserved = new byte[8];
            for (int i = 0; i < 3; i++)
            {
                obj.Reserved[i] = 0;
            }

            bool ret = UsbCanUtil.Instance().Send(ref obj);
            return ret;
        }

        public Message ReceiveMessage(int timeout)
        {
            ECAN.CAN_OBJ obj;
            var ok = UsbCanUtil.Instance().Receive(out obj, (uint)timeout);
            if (!ok)
            {
                return null;
            }
            else
            {
                Message message = new Message();
                message.Id = (int)obj.ID;
                message.DataLen = obj.DataLen;
                obj.Data.CopyTo(message.Data, 0);
                return message;
            }
            
        }

        #region Singleton implement
        private static CanDriver instance;
        private static object syncRoot = new object();

        private CanDriver()
        {

        }

        public static CanDriver Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CanDriver();
                    }
                }
            }
            return instance;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ECAN;

namespace DcmService
{
    public class UsbCanUtil
    {
        public const uint DefaultDeviceType = 1;

        private bool opened = false;
        public bool IsOpened
        {
            get
            {
                return opened;
            }
        }

        public bool Open(UInt32 DeviceType, ref INIT_CONFIG init)
        {
            if (IsOpened) return true;

            if (ECANDLL.OpenDevice(DeviceType, 0, 0) != ECANStatus.STATUS_OK)
            {
                MessageBox.Show("Usb Can打开失败", "错误", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            if (ECANDLL.InitCAN(DeviceType, 0, 0, ref init) != ECANStatus.STATUS_OK)
            {
                MessageBox.Show("Usb Can初始化失败", "错误", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                return false;
            }

            if (ECANDLL.ClearBuffer(DeviceType, 0, 0) != ECANStatus.STATUS_OK)
            {
                MessageBox.Show("清除CAN缓冲区失败", "错误", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                return false;
            }

            if (ECANDLL.StartCAN(DeviceType, 0, 0) != ECANStatus.STATUS_OK)
            {
                MessageBox.Show("启动CAN失败", "错误", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                return false;
            }
            else
            {
                opened = true;
                Console.WriteLine("USB CAN open successfully!");
                return true;
            }
        }

        public bool Open(UInt32 DeviceType)
        {
            ECAN.INIT_CONFIG defaultInitConfig = new ECAN.INIT_CONFIG();
            defaultInitConfig.AccCode = 0x00000000;

            //defaultInitConfig.AccCode = 
            //    this.CalculateStandardFrameAccCode((uint)DcmService.ResponseId);

            defaultInitConfig.AccMask = 0x0;
            defaultInitConfig.Reserved = 0x0;
            defaultInitConfig.Filter = 0x00;
            defaultInitConfig.Timing0 = 0x0;
            defaultInitConfig.Timing1 = 0x1c;
            defaultInitConfig.Mode = 0x00;
            return Open(DeviceType, ref defaultInitConfig);
        }

        public void Close()
        {
            if (IsOpened)
            {
                opened = false;
                ECANDLL.CloseDevice(DefaultDeviceType, 0);
            }
        }

        public bool Send(ref CAN_OBJ obj)
        {
            if (!IsOpened)
            {
                return false;
            }
            return ECANStatus.STATUS_OK == 
                ECANDLL.Transmit(DefaultDeviceType, 0, 0, ref obj, 1);
        }

        public bool Receive(out CAN_OBJ obj, uint timeout=0)
        {
            return ECANStatus.STATUS_OK ==
                ECANDLL.Receive(DefaultDeviceType, 0, 0, out obj, 1, timeout);
        }

        public bool ReadErrorInfo(out CAN_ERR_INFO info)
        {
            return ECANStatus.STATUS_OK ==
                ECANDLL.ReadErrInfo(DefaultDeviceType, 0, 0, out info);
        }

        public static uint CalculateStandardFrameAccCode(uint msgid)
        {
            return ((msgid & 0x7FF) << 17) << 4;
        }

        #region Singleton implement
        private static UsbCanUtil instance;
        private static object syncRoot = new object();

        private UsbCanUtil()
        {
           
        }

        public static UsbCanUtil Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new UsbCanUtil();
                    }
                }
            }
            return instance;
        }
        #endregion
    }
}

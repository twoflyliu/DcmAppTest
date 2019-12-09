using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    /// <summary>
    /// 主要负责如下职能：
    /// 1. 使用协议层完成通信事务
    /// 2. 当通信完成后，通知Dcm层
    /// </summary>
    class DcmTp 
    {
        private DcmTpProtocal protocol = DcmTpProtocalISO15765.Instance();
        public DcmTpProtocal Protocol
        {
            set
            {
                protocol = value;
            }
        }

        private readonly DcmAppRxIndicationCallback rxIndication;

        public enum WorkStateEnum
        {
            Idle = 0,
            Working
        };
        private WorkStateEnum workState;
        public bool IsIdle
        {
            get
            {
                return workState == WorkStateEnum.Idle;
            }
        }

        /// <summary>
        /// 清空内部所有状态
        /// </summary>
        public void Reset()
        {
            workState = WorkStateEnum.Idle;
            protocol.Reset();
        }

        public bool Execute(int canId, int hostId, List<byte> data)
        {
            workState = WorkStateEnum.Working;
            if (protocol == null)
            {
                throw new InvalidOperationException("protocal unset");
            }

            bool ret = protocol.Execute(canId, hostId, data);
            if (ret)
            {
                NotifyDcmAppRxIndication(canId, hostId, data);
            }
            return ret;
        }

        private void NotifyDcmAppRxIndication(int canId, int hostId, List<byte> reqData)
        {
            DcmTpHandleResult handleResult = protocol.GetHandleResult();
            List<byte> receivedData = protocol.GetReceivedData();

            DcmAppRxIndicationArgs args = new DcmAppRxIndicationArgs();
            args.Result = handleResult;
            args.RequestData = reqData;
            args.ResponseData = receivedData;
            args.RequestCanId = canId;
            args.ResponseCanId = hostId;
            rxIndication(args);

            workState = WorkStateEnum.Idle;
        }

        #region Singleton implement
        private static DcmTp instance;
        private static object syncRoot = new object();

        private DcmTp(DcmAppRxIndicationCallback rxIndication)
        {
            this.rxIndication = rxIndication 
                ?? throw new ArgumentNullException("Dcm App Rx Indication Cannot be null");

            workState = WorkStateEnum.Idle;
        }

        public static DcmTp Instance(DcmAppRxIndicationCallback rxIndication = null)
        {
            if (instance == null)
            {
                lock(syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new DcmTp(rxIndication);
                    }
                }
            }
            return instance;
        }
        #endregion
    }


}

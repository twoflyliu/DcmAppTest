using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{

    class DcmAppDataParserISO14229 : DcmAppDataParserAdapter
    {
        private const byte NegativeResponseCode = 0x7f;

        public delegate void ParseHandler(List<byte> data, List<KeyValuePair<string, string>> result);

        public Dictionary<byte, ParseHandler> Parsers; 

        public DcmAppDataParserISO14229()
        {
            Parsers = new Dictionary<byte, ParseHandler>();

            Parsers[Service.ReadDTCInformation] = ReadDTCInformationParser.Parse;
            Parsers[Service.Session] = SessionParser.Parse;
        }

        protected override List<KeyValuePair<string, string>> DoParse(List<byte> data, out bool postiveRes)
        {
            List<KeyValuePair<string, string>> result =
                new List<KeyValuePair<string, string>>();

            postiveRes = false;
            if (data.Count > 0)
            {
                byte code = data[0];
                if (code == NegativeResponseCode)
                {
                    postiveRes = false;
                    ParseNegativeResponse(data, result);
                }
                else
                {
                    postiveRes = true;
                    ParsePostiveResponse(data, result);
                }
            }

            return result;
        }

        private void ParsePostiveResponse(List<byte> data, List<KeyValuePair<string, string>> result)
        {
            if (data.Count < 1)
            {
                return;
            }

            byte sid = (byte)(data[0] & 0xBF);
            result.Add(new KeyValuePair<string, string>("服务ID  ", GetServiceName(sid)));
            result.Add(new KeyValuePair<string, string>("肯定响应", 
                GetPostiveResponseData(data)));

            ParseHandler handler;
            if (Parsers.TryGetValue(sid, out handler))
            {
                handler(data, result);
            }
        }

        private string GetPostiveResponseData(List<byte> data)
        {
            if (data.Count < 2)
            {
                return string.Empty;
            }

            return ByteListToHexString(data, 1);
        }

        private string ByteListToHexString(List<byte> data, int offset)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = offset; i < data.Count; i++)
            {
                sb.AppendFormat("{0:X2} ", data[i]);
            }
            return sb.ToString().TrimEnd();
        }

        private void ParseNegativeResponse(List<byte> data, List<KeyValuePair<string, string>> result)
        {
            if (data.Count != 3)
            {
                return;
            }

            byte sid = data[1];
            byte nrc = data[2];

            result.Add(new KeyValuePair<string, string>("服务ID  ", GetServiceName(sid)));
            result.Add(new KeyValuePair<string, string>("否定响应", GetErrorDesc(nrc)));
        }

        private string GetErrorDesc(byte nrc)
        {
            string err;
            if (NegativeResponse.ErrorDescrption.TryGetValue(nrc, out err))
            {
                return err;
            }
            return string.Format("{0:X2}", nrc);
        }

        public string GetServiceName(byte sid)
        {
            string name;
            if (Service.ServiceDescription.TryGetValue(sid, out name))
            {
                return name;
            }
            return string.Format("{0:X2}", sid);
        }

        static class NegativeResponse
        {
            // 标准否定响应码
            public const byte GeneralReject = 0x10;
            public const byte ServiceNotFound = 0x11;
            public const byte SubFunctionNotSupported = 0x12;
            public const byte IncorrectMessageLengthOrInvalidFormat = 0x13;
            public const byte ResponseMessageTooLong = 0x14;

            public const byte BusyRepeatRequest = 0x21;
            public const byte ConditionNotCorrect = 0x22;
            public const byte RequestSequenceError = 0x24;

            public const byte RequestOutOfRange = 0x31;
            public const byte SecurityAccessDenied = 0x33;
            public const byte InvalidKey = 0x35;
            public const byte ExceededNumberOfAttemps = 0x36;
            public const byte RequiredTimeDelayNotExpired = 0x37;

            public const byte UploadDownloadNotAccepted = 0x70;
            public const byte TransferDataSuspended = 0x71;
            public const byte GeneralProgrammingFailure = 0x72;
            public const byte ErrorBlockSequenceCounter = 0x73;
            public const byte RequestCorrectRecievedResponsePending = 0x78;
            public const byte SubFunctionNotSupportedInActiveSession = 0x7e;
            public const byte ServiceNotSupportedInActiveSession = 0x7f;



            // 存放否定响应码对应的描述
            public static Dictionary<byte, string> ErrorDescrption { get; }

            static NegativeResponse()
            {
                ErrorDescrption = new Dictionary<byte, string>();
                ErrorDescrption[GeneralReject] = "请求被否决";
                ErrorDescrption[ServiceNotFound] = "服务器不支持客户端请求的诊断服务";
                ErrorDescrption[SubFunctionNotSupported] = "服务器不支持客户端请求服务的子功能";
                ErrorDescrption[IncorrectMessageLengthOrInvalidFormat] = "消息的长度是错误的";
                ErrorDescrption[ResponseMessageTooLong] = "响应消息太长";

                ErrorDescrption[BusyRepeatRequest] = "重复请求繁忙";
                ErrorDescrption[ConditionNotCorrect] = "请求的服务条件不满足";
                ErrorDescrption[RequestSequenceError] = "请求时序错误";
                ErrorDescrption[RequestOutOfRange] = "请求超出范围";

                ErrorDescrption[InvalidKey] = "无效的key";
                ErrorDescrption[ExceededNumberOfAttemps] = "超出尝试次数";
                ErrorDescrption[RequiredTimeDelayNotExpired] = "在定时器延时的时间接收到一个请求";
                ErrorDescrption[SecurityAccessDenied] = "安全访问被否决";
                ErrorDescrption[UploadDownloadNotAccepted] = "上传下载未被接受";
                ErrorDescrption[TransferDataSuspended] = "传输数据被挂起";
                ErrorDescrption[GeneralProgrammingFailure] = "通用编程失败";
                ErrorDescrption[ErrorBlockSequenceCounter] = "错误的块序列号";

                ErrorDescrption[RequestCorrectRecievedResponsePending] = "请求被正确接收到，响应被挂起";
                ErrorDescrption[SubFunctionNotSupportedInActiveSession] = "子功能在活动会话下不被支持";
                ErrorDescrption[ServiceNotSupportedInActiveSession] = "服务在活动会话下不被支持";
            }
        }

        static class Service
        {
            // 标准服务SID
            public const byte Session = 0x10;
            public const byte WriteDataByIdentifier = 0x2E;
            public const byte ReadDataByIdentifier = 0x22;
            public const byte SecurityAccess = 0x27;
            public const byte DTCControlSetting = 0x85;
            public const byte IOControl = 0x2F;
            public const byte Routine = 0x31;
            public const byte CommunicationControl = 0x28;
            public const byte EcuReset = 0x11;
            public const byte DownloadRequest = 0x34;
            public const byte DownloadTransmit = 0x36;
            public const byte DownloadStop = 0x37;
            public const byte TesterPresent = 0x3E;
            public const byte ClearDiagnosticInfo = 0x14;
            public const byte ReadDTCInformation = 0x19;

            public static Dictionary<byte, string> ServiceDescription { get; }

            static Service()
            {
                ServiceDescription = new Dictionary<byte, string>();

                ServiceDescription[Session] = "诊断会话控制服务";
                ServiceDescription[WriteDataByIdentifier] = "写数据服务";
                ServiceDescription[ReadDataByIdentifier] = "读数据服务";
                ServiceDescription[SecurityAccess] = "安全访问服务";
                ServiceDescription[DTCControlSetting] = "诊断控制服务";
                ServiceDescription[IOControl] = "IO控制服务";
                ServiceDescription[Routine] = "路由服务";
                ServiceDescription[CommunicationControl] = "通信控制";
                ServiceDescription[EcuReset] = "复位服务";
                ServiceDescription[ReadDTCInformation] = "DTC读取服务";
                ServiceDescription[DownloadRequest] = "请求下载服务";
                ServiceDescription[DownloadTransmit] = "传输数据服务";
                ServiceDescription[DownloadStop] = "传输终止服务";
                ServiceDescription[TesterPresent] = "维持在线服务";
                ServiceDescription[ClearDiagnosticInfo] = "DTC清除服务";
            }
        }

        static class ReadDTCInformationParser
        {
            enum SubFunction
            {
                ISOSAEReserved = 0x00,
                ReportNumberOfDTCByStatusMask,
                ReportDTCByStatusMask,
                ReportDTCSnapshotIdentification,
                ReportDTCSnapshotRecordByDTCNumber,
                ReportDTCSnapshotRecordByRecordNumber,
                ReportDTCExtendedDataRecordByDTCNumber,
                ReportNumberOfDTCBySeverityMaskRecord,
                ReportDTCBySeverityMaskRecord,
                ReportSeverityInformationOfDTC,
                ReportSupportedDTC,
                ReportFirstTestFailedDTC,
                ReportFirstConfirmedDTC,
                ReportMostRecentTestFailedDTC,
                ReportMostRecentConfirmedDTC,
                ReportMirrorMemoryDTCByStatusMask,
                ReportMirrorMemoryDTCExtendedDataRecordByDTCNumber,
                ReportNumberOfMirrorMemoryDTCByStatusMask,
                ReportNumberOfEmissionsRelatedOBDDTCByStatusMask,
                ReportEmissionsRelatedOBDDTCByStatusMask
            }

            public static void Parse(List<byte> data,
                List<KeyValuePair<string, string>> result)
            {
                SubFunction subFunction = (SubFunction)data[1];
                switch (subFunction)
                {
                    case SubFunction.ReportNumberOfDTCByStatusMask:
                        ParseReportNumberOfDTCByStatusMask(data, result);
                        break;
                    case SubFunction.ReportDTCByStatusMask:
                        ParseReportDTCByStatusMask(data, result);
                        break;
                    case SubFunction.ReportSupportedDTC:
                        ParseReportSupportedDTC(data, result);
                        break;
                }
            }

            //0A 子功能
            private static void ParseReportSupportedDTC(List<byte> data,
                List<KeyValuePair<string, string>> result)
            {
                ParseReportDTCByStatusMask(data, result);
            }

            //02 子功能
            private static void ParseReportDTCByStatusMask(List<byte> data,
                List<KeyValuePair<string, string>> result)
            {
                int count = (data.Count - 2 - 1) / 4;

                if (data.Count < 3)
                {
                    return;
                }

                result.Add(new KeyValuePair<string, string>("可获得的状态掩码",
                    string.Format("{0:X2}", data[2])));
                int offset = 3;
                for (int i = 0; i < count; i++)
                {
                    string temp = string.Format("DTC-> 0x{0:X6}, Status-> 0x{1:X2}",
                        (data[3 + offset] << 16) | (data[4 + offset] << 8) | data[5 + offset],
                        data[6 + offset]);
                    result.Add(new KeyValuePair<string, string>("状态", temp));
                    offset += 4;
                }
            }

            //01 子功能
            private static void ParseReportNumberOfDTCByStatusMask(List<byte> data,
                List<KeyValuePair<string, string>> result)
            {
                if (data.Count < 6)
                {
                    return;
                }

                string[] DtcFormat = new string[]{ "ISO15031-6DTCFormat",
                    "ISO14229-1DTCFormat", "SAEJ1939-73DTCFormat", "11992-4DTCFormat", "UNKNOWN"};

                //忽略sid, 和子功能，所以索引从2开始
                result.Add(new KeyValuePair<string, string>("可获得的状态掩码", 
                    string.Format("{0:X2}", data[2])));
                result.Add(new KeyValuePair<string, string>("DTC格式",
                    DtcFormat[Math.Min(data[3], (byte)4)]));

                int dtcCount = (data[4] << 8) | data[5];
                result.Add(new KeyValuePair<string, string>("DTC数目", dtcCount.ToString()));
            }
        }

        static class SessionParser
        {
            enum SubFunction
            {
                IsoSaeReservedM = 0x00,
                DefaultSession,
                ProgramSession,
                ExtendSession,
                IsoSaeReservedU, //0x04-0x3F
                CarSupplierSession, //0x40-0x5F
                SysSupplierSession, //0x60-0x7E
                IsoSaeReservedM2 = 0x7f //0x7f
            }

            public static void Parse(List<byte> data,
                List<KeyValuePair<string, string>> result)
            {
                if (data.Count < 2)
                {
                    return;
                }
                var subFunction = (SubFunction)data[1];

                // 通知会话变化
                NotifySessionChanged(subFunction);

                // 解析数据
                
            }

            private static void NotifySessionChanged(SubFunction subFunction)
            {
                SessionChangedEventArgs args = new SessionChangedEventArgs();
                args.DefaultSession = (subFunction == SubFunction.DefaultSession);
                DcmService.FireSessionChangedEvent(args);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    public delegate void ParsingDataIncommingEventHanlder(ParsingDataIncommingEventArgs e);
    public class ParsingDataIncommingEventArgs
    {
        public List<KeyValuePair<string, string>> EntryList { get; set; }
        public List<byte> ResponseData { get; set; }
        public List<byte> RequestData { get; set; }
        public bool PostiveResponse { get; set; } //是否是肯定响应
        public int RequestCanId { get; set; }
        public int ResponseCanId { get; set; }
    }

    public delegate void RawDataIncommingEventHandler(RawDataIncommingEventArgs e);
    public class RawDataIncommingEventArgs
    {
        public int CanId { get; set; }
        public List<byte> Data { get; set; }
        public bool Ok { get; set; }
        public DateTime Timestamp {get; set;}

        public enum OperationEum
        {
            Send = 0,
            Receive
        }

        public OperationEum Operation { get; set; }

        public RawDataIncommingEventArgs()
        {
            Timestamp = DateTime.Now;
        }
    }

    public delegate void SessionChangedEventHandler(SessionChangedEventArgs e);
    public class SessionChangedEventArgs
    {
        /// <summary>
        /// 当前是否处于默认会话，在默认会话下，建议关闭心跳包，在非非默认会话下需要使能心跳包
        /// </summary>
        public bool DefaultSession { get; set; }
    }
}

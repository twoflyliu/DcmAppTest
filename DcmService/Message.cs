using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    class Message
    {
        /// <summary>
        /// 报文最大允许的数据长度
        /// </summary>
        public const int MaxDataLen = 8;

        /// <summary>
        /// 报文Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 报文数据长度
        /// </summary>
        public int DataLen { get; set; }

        private byte[] data;

        /// <summary>
        /// 报文数据
        /// </summary>
        public byte[] Data
        {
            get
            {
                if (data == null)
                {
                    data = new byte[8];
                }
                return data;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmConfig
{
    public class Config
    {
        public int PhysicalRequestId { get; set; }
        public int FunctionRequestId { get; set; }
        public int ResponseId { get; set; }
        public string SecurityAccessType { get; set; }

        public bool CanTickEnabled { get; set; }
        public int CanTickPeriod { get; set; }
        public bool SuppressTickResponse { get; set; }

        public Config()
        {
            PhysicalRequestId = 0x00;
            FunctionRequestId = 0x01;
            ResponseId = 0x02;
            SecurityAccessType = string.Empty;
        }
    }
}

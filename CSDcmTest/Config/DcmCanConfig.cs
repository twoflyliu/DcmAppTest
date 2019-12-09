using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSDcmTest
{
    class DcmCanConfig
    {
        [Category("Can"), Description("配置Can物理请求ID")]
        [TypeConverter(typeof(CanIdTypeConverter))]
        public int CanPhysicalRequestId { get; set; }

        [Category("Can"), Description("配置Can功能请求ID")]
        [TypeConverter(typeof(CanIdTypeConverter))]
        public int CanFunctionRequestId { get; set; }

        [Category("Can"), Description("配置Can响应ID")]
        [TypeConverter(typeof(CanIdTypeConverter))]
        public int CanResponseId { get; set; }

        [Category("安全访问"), Description("配置安全访问算法")]
        [TypeConverter(typeof(SecurityAccessTypeConverter))]
        public string SecurityAccessType { get; set; }

        [Category("维持在线"), Description("使能心跳包")]
        public bool CanTickEnabled { get; set; }

        [Category("维持在线"), Description("配置维持在线周期（单位：秒）")]
        public uint CanTickPeriod { get; set; }

        [Category("维持在线"), Description("抑制响应")]
        public bool SuppressResponse { get; set; }

        public DcmCanConfig()
        {
            CanPhysicalRequestId = 0x766;
            CanFunctionRequestId = 0x7DF;
            CanResponseId = 0x706;

            CanTickEnabled = false;
            CanTickPeriod = 3;
            SuppressResponse = true;

            foreach (var name in SecurityAccessAlgorithManager.Instance()
                .GetSecurityAccessAlgorithNames())
            {
                SecurityAccessType = name;
                break;
            }
        }
    }
}

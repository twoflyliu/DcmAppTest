using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmConfig
{
    /// <summary>
    /// 解析方向
    /// </summary>
    public enum ParsingDirection
    {
        None = 0,           //不进行解析
        Send,           //解析要发送的数据
        Receive,        //解析接收的数据
        Bidirection     //双向
    }
}

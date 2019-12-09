using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    /// 1. 负责打包/解包应用层传过来的数据
    /// 2. 负责将打包好的数据以一种指定的协议传输出去
    /// 3. 负责接收ECU传过来的数据，并进行解包
    interface DcmTpProtocal
    {
        /// <summary>
        /// 按照各自协议执行一条各自的发送/接收流程
        /// </summary>
        /// <param name="canId">CAN报文ID</param>
        /// <param name="hostId">主机ID</param>
        /// <param name="data">要发送的数据</param>
        /// <returns>true, 则表示接收，false表示未结束</returns>
        bool Execute(int canId, int hostId, List<byte> data);

        /// <summary>
        /// 获取处理结果
        /// </summary>
        /// <returns></returns>
        DcmTpHandleResult GetHandleResult();

        /// <summary>
        /// 获取接收到的数据
        /// </summary>
        /// <returns>接收到的数据</returns>
        List<byte> GetReceivedData();

        /// <summary>
        /// 清空内部所有状态
        /// </summary>
        void Reset();
    }
}

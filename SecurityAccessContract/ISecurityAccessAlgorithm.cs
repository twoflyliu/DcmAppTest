using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityAccessContract
{
    public interface ISecurityAccessAlgorithm
    {
        /// <summary>
        /// 算法实现
        /// </summary>
        /// <param name="securityLevel">算法安全级别</param>
        /// <param name="rawData">原始数据</param>
        /// <returns>加密后的数据</returns>
        List<byte> Encrypt(int securityLevel, List<byte> rawData);

        /// <summary>
        /// 算法名称
        /// </summary>
        string Name { get; }
    }
}

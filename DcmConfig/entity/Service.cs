using System.Collections.Generic;

namespace DcmConfig
{
    public class Service
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 子功能列表
        /// </summary>
        public List<SubFunction> SubFunctions { get; set; }

        public Service()
        {
            SubFunctions = new List<SubFunction>();
        }
    }
}
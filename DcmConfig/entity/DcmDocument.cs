using System;
using System.Collections.Generic;

namespace DcmConfig
{
    /// <summary>
    /// 表示整个配置文档
    /// </summary>
    public class DcmDocument
    {
        /// <summary>
        /// 文档版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 服务列表
        /// </summary>
        public List<Service> Services { get; set; }

        /// <summary>
        /// 值描述文档
        /// </summary>
        public Vdf4Cs.VdfDocument VdfDocument { get; set; }

        /// <summary>
        /// 值描述文件路径
        /// </summary>
        public string VdfFile { get; set; }

        private Dictionary<ulong, SubFunction> parsingReceiveSubFunctionTable;
        private object syncRoot = new object();
        /// <summary>
        /// 所有负责接收解析子功能表，方便便于查找对应的子功能
        /// </summary>
        public Dictionary<ulong, SubFunction> ParsingReceiveSubFunctionTable
        {
            get
            {
                if (parsingReceiveSubFunctionTable == null)
                {
                    lock(syncRoot)
                    {
                        if (parsingReceiveSubFunctionTable == null)
                        {
                            parsingReceiveSubFunctionTable =
                                new Dictionary<ulong, SubFunction>();
                            foreach (var service in Services)
                            {
                                foreach (var subFunction in service.SubFunctions)
                                {
                                    //if (subFunction.Message == null) continue;
                                    if (subFunction.ParsingDirection == ParsingDirection.Receive
                                        || subFunction.ParsingDirection == ParsingDirection.Bidirection)
                                    {
                                        parsingReceiveSubFunctionTable.Add(GenParsingReceiveKey(subFunction.Prefix),
                                            subFunction);
                                    }
                                }
                            }
                        }
                    }
                }
                return parsingReceiveSubFunctionTable;
            }
        }

        public void UpdateReceiveSubFunctionTable()
        {
            lock(syncRoot)
            {
                parsingReceiveSubFunctionTable = null;
            }
        }

        public static ulong GenParsingReceiveKey(List<byte> prefix, int len = -1)
        {
            ulong key = 0;
            if (len == -1)
            {
                len = prefix.Count;
            }

            for (int i = 0; i <len; i++)
            {
                key = (key << 8) | prefix[i];
            }
            return key;
        }

        public Config Config { get; set; }

        public DcmDocument()
        {
            Version = DcmConfig.CurrVersion;
            Services = new List<Service>();
            Config = new Config();

            VdfDocument = new Vdf4Cs.VdfDocument();
        }

        public void RemoveSubFunction(SubFunction subFunction)
        {
            if (subFunction == null)
            {
                return;
            }
            subFunction.VdfMessage = null; //移除对于Message的拥有关系

            foreach (var service in Services)
            {
                if (service.SubFunctions.Remove(subFunction))
                {
                    break;
                }
            }
        }
    }
}
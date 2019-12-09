using System;
using System.Collections;
using System.Collections.Generic;
using Vdf4Cs;

namespace DcmConfig
{
    /// <summary>
    /// 表示一个子功能
    /// </summary>
    public class SubFunction : IComparable
    {
        /// <summary>
        /// 子功能名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public List<byte> Prefix { get; set; }

        /// <summary>
        /// Can地址类型
        /// </summary>
        public CanAddressType CanAddressType { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public int DataLen { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType DataType { get; set; }

        /// <summary>
        /// 数据（长度为DataLen)
        /// </summary>
        public List<byte> Data { get; set; }

        /// <summary>
        /// 值描述名称
        /// </summary>
        public string Message { get; set; }

        private VdfMessage vdfMessage = null;
        public VdfMessage VdfMessage
        {
            get { return vdfMessage; }
            set
            {
                if (vdfMessage != value)
                {
                    if (vdfMessage != null)
                    {
                        vdfMessage.Owners.Remove(this);
                    }
                    if (value != null)
                    {
                        value.Owners.Add(this);
                    }
                    vdfMessage = value;
                }
            }
        }

        /// <summary>
        /// 解析发送的数据
        /// </summary>
        public ParsingDirection ParsingDirection { get; set; }

        public SubFunction()
        {
            ParsingDirection = ParsingDirection.Send; //默认解析发送
        }

        public override string ToString()
        {
            return Name + "- Sub Function";
        }

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is SubFunction))
            {
                return -1;
            }
            var rhs = obj as SubFunction;
            int ret = Name.CompareTo(rhs.Name);
            if (ret != 0)
            {
                if (Prefix == null)
                {
                    return -1;
                }
                
                if (rhs.Prefix == null)
                {
                    return 1;
                }

                var minCount = Math.Min(Prefix.Count, rhs.Prefix.Count);
                for (int i = 0; i < minCount; i++)
                {
                    ret = Prefix[i] - rhs.Prefix[i];
                    if (ret != 0)
                    {
                        break;
                    }
                }

                if (ret == 0)
                {
                    return Prefix.Count - rhs.Prefix.Count;
                }
                return ret;
            }
            return ret;
        }
    }
}
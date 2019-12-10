using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vdf4Cs
{
    public enum VdfByteOrder
    {
        Intel = 0,
        Motorola
    }

    public class VdfMessage
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ByteLen { get; set; }
        public Dictionary<string, VdfSignal> SignalTable { get; set; }

        public SortedSet<object> Owners { get; set; }

        public VdfMessage()
        {
            Owners = new SortedSet<object>();
            SignalTable = new Dictionary<string, VdfSignal>();
        }

        public override string ToString()
        {
            return Name + " - VdfMessage";
        }
    }

    public class VdfSignal : IComparable
    {
        public string Name { get; set; }
        public int StartBit { get; set; }
        public int BitLen { get; set; }
        public VdfByteOrder ByteOrder { get; set; }
        public string ValueDesc { get; set; }

        private VdfValueDesc vdfValueDesc = null;
        public VdfValueDesc VdfValueDesc
        {
            get { return vdfValueDesc; }
            set
            {
                if (vdfValueDesc != value)
                {
                    if (vdfValueDesc != null)
                    {
                        vdfValueDesc.Owners.Remove(this);
                    }

                    if (value != null)
                    {
                        value.Owners.Add(this);
                    }

                    vdfValueDesc = value;
                }
            }
        }

        public VdfSignal()
        {
            Name = "";
            ByteOrder = VdfByteOrder.Intel;
            ValueDesc = string.Empty;
        }

        public override string ToString()
        {
            return Name + " - VdfSignal";
        }

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is VdfSignal))
            {
                return -1;
            }
            var rhs = obj as VdfSignal;
            int ret = Name.CompareTo(rhs.Name);
            if (ret != 0)
            {
                return ret;
            }
            else
            {
                ret = StartBit - rhs.StartBit;
                if (ret == 0)
                {
                    return BitLen - rhs.BitLen;
                }
                return ret;
            }
        }
    }

    public abstract class VdfValueDesc
    {
        public string Name { get; set; }

        public SortedSet<VdfSignal> Owners { get; set; }

        delegate void VdfValueDescLoadHandler(XmlNode valueDescNode,
            Dictionary<string, VdfValueDesc> valueDescTable);
        private static Dictionary<string, VdfValueDescLoadHandler> ValueDescLoad;

        static VdfValueDesc()
        {
            ValueDescLoad = new Dictionary<string, VdfValueDescLoadHandler>();
            SupportedValueDescType = new List<string>();

            AddValueDesc(VdfBcdValueDesc.Type, VdfBcdValueDesc.Load);
            AddValueDesc(VdfPhyValueDesc.Type, VdfPhyValueDesc.Load);
            AddValueDesc(VdfXncodeValueDesc.Type, VdfXncodeValueDesc.Load);
            AddValueDesc(VdfAsciiValueDesc.Type, VdfAsciiValueDesc.Load);
        }

        public VdfValueDesc()
        {
            Owners = new SortedSet<VdfSignal>();
        }

        public override string ToString()
        {
            return Name + " - VdfValueDesc";
        }

        private static void AddValueDesc(string type, VdfValueDescLoadHandler load)
        {
            SupportedValueDescType.Add(type);
            ValueDescLoad.Add(type, load);
        }

        public static List<string> SupportedValueDescType { get; private set; }

        internal static void Load(string type, XmlNode valueDescNode,
            Dictionary<string, VdfValueDesc> valueDescTable)
        {
            VdfValueDescLoadHandler load;
            if (ValueDescLoad.TryGetValue(type.ToUpper(), out load))
            {
                load(valueDescNode, valueDescTable);
            }
            else
            {
                throw new VdfException("Unknown ValueDescType " + type);
            }
        }

        public abstract void Save(XmlElement valueDescTableElem, XmlDocument xmlDoc,
            VdfDocument vdfDocument);
        

        public static T Load<T>(XmlNode valueDescNode,
            Dictionary<string, VdfValueDesc> valueDescTable)
            where T: VdfValueDesc, new()
        {
            T t = new T();

            //解析名称
            var attr = valueDescNode.Attributes[VdfTag.NameAttrName];
            if (attr != null)
            {
                t.Name = attr.Value;
            }

            // 必须有名称，没有名称则无法引用
            if (string.IsNullOrEmpty(t.Name))
            {
                return null;
            }

            return t;
        }

        // 进行编码
        public virtual string Encode(uint value, bool withUnit)
        {
            throw new NotImplementedException();
        }

        public virtual string Encode(List<byte> data, VdfSignal signal, bool withUnit)
        {
            throw new NotImplementedException();
        }

        public virtual uint Decode(string value)
        {
            throw new NotImplementedException();
        }

        public virtual void Decode(string value, VdfSignal signal, List<byte> outBytes)
        {
            throw new NotImplementedException();
        }


        public  XmlElement CreateValueDesc(XmlElement valueDescTableElem,
            XmlDocument xmlDoc) 
        {
            // 创建ValueDesc元素节点
            var valDescElem = xmlDoc.CreateElement(VdfTag.ValueDescElemName);
            valueDescTableElem.AppendChild(valDescElem);

            // 设置Name属性
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.NameAttrName, Name);

            return valDescElem;
        }


    }

    public class VdfXncodeValueDesc : VdfValueDesc
    {
        public const string Type = "XNCODE";

        public Dictionary<int, string> EntryTable;

        public VdfXncodeValueDesc()
        {
            EntryTable = new Dictionary<int, string>();
        }

        public override string ToString()
        {
            return Name + " - VdfXncodeValueDesc";
        }

        public static void Load(XmlNode valueDescNode,
            Dictionary<string, VdfValueDesc> valueDescTable)
        {
            VdfXncodeValueDesc xncodeValueDesc = VdfValueDesc.Load<VdfXncodeValueDesc>(valueDescNode,
                valueDescTable);
            if (xncodeValueDesc == null)
            {
                return;
            }

            valueDescTable.Add(xncodeValueDesc.Name, xncodeValueDesc);
            xncodeValueDesc.EntryTable = new Dictionary<int, string>();

            //解析所有Entry
            var entryNodes = valueDescNode.SelectNodes(VdfTag.EntryElemName);
            foreach (XmlNode entryNode in entryNodes)
            {
                // 获取值
                int value;
                if (!XmlUtils.TryGetIntAttrValue(entryNode, VdfTag.ValueAttrName, out value)) //首先尝试10进制解析
                {
                    // 失败时候使用十六进制解析
                    if (!XmlUtils.TryGetHexIntAttrValue(entryNode, VdfTag.ValueAttrName, 
                        out value))
                    {
                        continue;
                    }
                }
                // 获取描述
                string desc;
                if (!XmlUtils.TryGetStringAttrValue(entryNode, VdfTag.DescriptionAttrName, out desc))
                {
                    continue;
                }

                try
                {
                    xncodeValueDesc.EntryTable.Add(value, desc);
                }
                catch (Exception)
                {
                    throw new VdfException(string.Format("Value Description [{0}] already exist " +
                        "[Decimal:{1}, Hex:0x{1:X}] key", xncodeValueDesc.Name, value));
                }
            }
        }

        public override string Encode(uint value, bool withUnit)
        {
            string ret;
            if (EntryTable.TryGetValue((int)value, out ret))
            {
                return ret;
            }
            return string.Empty;
        }

        public override uint Decode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            foreach (var entry in EntryTable)
            {
                if (value == entry.Value)
                {
                    return (uint)entry.Key;
                }
            }
            return Convert.ToUInt32(value);
        }

        public override void Save(XmlElement valueDescTableElem, XmlDocument xmlDoc, 
            VdfDocument vdfDocument)
        {
            var valueDescElem = CreateValueDesc(valueDescTableElem, xmlDoc);
            XmlUtils.SetElemAttribute(valueDescElem, xmlDoc, VdfTag.TypeAttrName, Type);

            foreach (var entry in EntryTable)
            {
                SaveEntry(entry, valueDescElem, xmlDoc, vdfDocument);
            }
        }

        private void SaveEntry(KeyValuePair<int, string> entry, 
            XmlElement valueDescElem, XmlDocument xmlDoc, VdfDocument vdfDocument)
        {
            var entryElem = xmlDoc.CreateElement(VdfTag.EntryElemName);
            valueDescElem.AppendChild(entryElem);
            XmlUtils.SetElemAttribute(entryElem, xmlDoc, VdfTag.ValueAttrName, entry.Key);
            XmlUtils.SetElemAttribute(entryElem, xmlDoc, VdfTag.DescriptionAttrName, entry.Value);
        }
    }

    // 以字节位单位
    public class VdfBcdValueDesc : VdfValueDesc
    {
        public const string Type = "BCD";

        public int Factor { get; set; }
        public int Offset { get; set; }
        public bool Fill { get; set; }

        //CanFillAlpha = true, 那么数据值表示只读信息，不表示数字, 只读信息以分隔符进行分隔, 位长度必须是8的整数倍
        //当为false的时候，Factor, Offset才有效, 并且只表示1个字节数据
        public bool CanFillAlpha { get; set; }
        public int Minimum { get; set; }
        public int Maximum { get; set; }

        //字节分隔符
        public string Separator { get; set; }

        public VdfBcdValueDesc()
        {
            Factor = 1;
            Offset = 0;
            Fill = true;
            CanFillAlpha = true;
            Minimum = 0x00;
            Maximum = 0xff;

            Separator = string.Empty;
        }

        public override string ToString()
        {
            return Name + " - VdfBcdValueDesc";
        }

        public static void Load(XmlNode valueDescNode,
            Dictionary<string, VdfValueDesc> valueDescTable)
        {
            VdfBcdValueDesc valueDesc = VdfValueDesc.Load<VdfBcdValueDesc>(valueDescNode,
               valueDescTable);
            if (valueDesc == null)
            {
                return;
            }
            valueDescTable.Add(valueDesc.Name, valueDesc);

            int ival;
            if (XmlUtils.TryGetIntAttrValue(valueDescNode, VdfTag.FactorAttrName, out ival))
            {
                valueDesc.Factor = ival;
            }
            if (XmlUtils.TryGetIntAttrValue(valueDescNode, VdfTag.OffsetAttrName, out ival))
            {
                valueDesc.Offset = ival;
            }

            bool bval;
            if (XmlUtils.TryGetBoolAttrValue(valueDescNode, VdfTag.FillAttrName, out bval))
            {
                valueDesc.Fill = bval;
            }
            if (XmlUtils.TryGetBoolAttrValue(valueDescNode, VdfTag.CanFillAlphaAttrName, out bval))
            {
                valueDesc.CanFillAlpha = bval;
            }

            if (XmlUtils.TryGetIntAttrValue(valueDescNode, VdfTag.MaximumAttrName, out ival))
            {
                valueDesc.Maximum = ival;
            }
            if (XmlUtils.TryGetIntAttrValue(valueDescNode, VdfTag.MinimumAttrName, out ival))
            {
                valueDesc.Minimum = ival;
            }

            if (XmlUtils.TryGetStringAttrValue(valueDescNode, VdfTag.SeparatorAttrName, out string sval))
            {
                valueDesc.Separator = sval;
            }
        }

        string ByteToHexString(byte data, bool fill = true)
        {
            return string.Format(fill ? "{0:X2}" : "{0:X}", data);
        }

        public uint GetValue(List<byte> data, VdfSignal signal)
        {
            return GetValue(data, signal.ByteOrder, signal.StartBit, signal.BitLen);
        }

        private uint GetValue(List<byte> data, VdfByteOrder byteOrder, int startBit, int bitLen)
        {
            switch (byteOrder)
            {
                default:
                case VdfByteOrder.Intel:
                    return ValueUtils.GetValueIntel(data, startBit, bitLen);
                case VdfByteOrder.Motorola:
                    return ValueUtils.GetValueMotorola(data, startBit, bitLen);
            }
        }

        public override string Encode(List<byte> data, VdfSignal signal, bool withUnit)
        {
            if (CanFillAlpha)
            {
                if (signal.BitLen % 8 != 0)
                {
                    throw new VdfException("VdfBcdValueDesc: When CanFillAlpha is true, " +
                        "Signal Bit Len is 8's times, But Current (" + signal.Name + ") bit len is " + signal.BitLen);
                }

                List<string> hexStr = new List<string>();

                int offset = signal.StartBit;
                int finalBit = signal.StartBit + signal.BitLen;
                while (offset < finalBit)
                {
                    hexStr
                        .Add(ByteToHexString(
                            (byte)GetValue(data, signal.ByteOrder, offset, 8), Fill));
                    offset += 8;
                }
                if (signal.ByteOrder == VdfByteOrder.Motorola)
                {
                    hexStr.Reverse();
                }
                return string.Join(Separator, hexStr);
            }
            else
            {
                string hexStr = string.Empty;
                try
                {
                    var val = GetValue(data, signal);
                    hexStr = string.Format("{0:X}", val);
                    try
                    {
                        int idata = Convert.ToInt32(hexStr);
                        idata = idata * Factor + Offset;
                        return idata.ToString();
                    }
                    catch (Exception)
                    {
                        // 没有按照规则来，仅报价数字字符
                        return hexStr;
                    }
                }
                catch (Exception)
                {
                    throw new VdfException("VdfBcdValueDesc: When CanFillAlpha is false, " +
                        "BCD integer value cannot containes A-F. BCD Integer = " + hexStr);
                }
            }
        }

        public override uint Decode(string value)
        {
            if (CanFillAlpha)
            {
                var strList = value.Split(Separator.ToCharArray());
                uint ret = 0x00;
                int offset = 0;
                foreach (var str in strList)
                {
                    byte bval = Convert.ToByte(str, 16);
                    ret |= (uint)(bval << offset);
                    offset += 8;
                }
                return ret;
            }
            else
            {
                try
                {
                    int idata = Convert.ToInt32(value);
                    var temp = (uint)((idata - Offset) / Factor);
                    return Convert.ToUInt32(string.Format("{0}", temp), 16);
                }
                catch (Exception)
                {
                    throw new VdfException("VdfBcdValueDesc: When CanFillAlpha is false, " +
                        "BCD integer value cannot containes A-F. BCD Integer = " + value);
                }
            }
        }

        public override void Save(XmlElement valueDescTableElem, XmlDocument xmlDoc, 
            VdfDocument vdfDocument)
        {
            var valDescElem = CreateValueDesc(valueDescTableElem, xmlDoc);
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.TypeAttrName, Type);

            if (CanFillAlpha)
            {
                XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.SeparatorAttrName, Separator);
            }
            else
            {
                XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.FactorAttrName, Factor);
                XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.OffsetAttrName, Offset);
                XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.MinimumAttrName, Minimum);
                XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.MaximumAttrName, Maximum);
            }
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc,
                VdfTag.CanFillAlphaAttrName, CanFillAlpha);
        }
    }

    public class VdfPhyValueDesc : VdfValueDesc
    {
        public const string Type = "PHY";

        public double Factor { get; set; }
        public double Offset { get; set; }
        public double Maximum { get; set; }
        public double Minimum { get; set; }
        public string Unit { get; set; }

        public VdfPhyValueDesc()
        {
            Factor = 1;
            Offset = 0;
            Unit = string.Empty;
            Maximum = double.MaxValue;
            Minimum = double.MinValue;
        }

        public override string ToString()
        {
            return Name + " - VdfPhyValueDesc";
        }

        public static void Load(XmlNode valueDescNode,
            Dictionary<string, VdfValueDesc> valueDescTable)
        {
            VdfPhyValueDesc valueDesc = VdfValueDesc.Load<VdfPhyValueDesc>(valueDescNode,
                valueDescTable);
            if (valueDesc == null)
            {
                return;
            }

            double dval;
            if (XmlUtils.TryGetDoubleAttrValue(valueDescNode, VdfTag.FactorAttrName, out dval))
            {
                valueDesc.Factor = dval;
            }

            if (XmlUtils.TryGetDoubleAttrValue(valueDescNode, VdfTag.OffsetAttrName, out dval))
            {
                valueDesc.Offset = dval;
            }

            if (XmlUtils.TryGetDoubleAttrValue(valueDescNode, VdfTag.MaximumAttrName, out dval))
            {
                valueDesc.Maximum = dval;
            }

            if (XmlUtils.TryGetDoubleAttrValue(valueDescNode, VdfTag.MinimumAttrName, out dval))
            {
                valueDesc.Minimum = dval;
            }

            string sval;
            if (XmlUtils.TryGetStringAttrValue(valueDescNode, VdfTag.UnitAttrName, out sval))
            {
                valueDesc.Unit = sval;
            }

            valueDescTable.Add(valueDesc.Name, valueDesc);
        }

        public override string Encode(uint value, bool withUnit)
        {
            double phyVal = value * Factor + Offset;
            return phyVal.ToString() + (withUnit ? Unit : string.Empty);
        }

        public override uint Decode(string value)
        {
            if (value.EndsWith(Unit))
            {
                value = value.Substring(0, value.Length - Unit.Length);
            }

            double dval;
            if (double.TryParse(value, out dval))
            {
                return (uint)((dval - Offset) / Factor + 0.000001);
            }
            else
            {
                throw new VdfException(string.Format("PhyValueDesc({0}): Decode non double value", value));
            }
        }

        public override void Save(XmlElement valueDescElem, XmlDocument xmlDoc,
            VdfDocument vdfDocument)
        {
            var valDescElem = CreateValueDesc(valueDescElem, xmlDoc);
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.TypeAttrName, Type);
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.FactorAttrName, Factor);
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.OffsetAttrName, Offset);
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.UnitAttrName, Unit);
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.MinimumAttrName, Minimum);
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.MaximumAttrName, Maximum);
        }
    }

    public class VdfAsciiValueDesc : VdfValueDesc
    {
        public const string Type = "ASCII";

        public override void Save(XmlElement valueDescTableElem, XmlDocument xmlDoc,
            VdfDocument vdfDocument)
        {
            var valDescElem = CreateValueDesc(valueDescTableElem, xmlDoc);
            XmlUtils.SetElemAttribute(valDescElem, xmlDoc, VdfTag.TypeAttrName, Type);
        }

        public static void Load(XmlNode valueDescNode,
            Dictionary<string, VdfValueDesc> valueDescTable)
        {
            VdfAsciiValueDesc valueDesc = VdfValueDesc.Load<VdfAsciiValueDesc>(valueDescNode,
                valueDescTable);
            if (valueDesc == null)
            {
                return;
            }
            valueDescTable.Add(valueDesc.Name, valueDesc);
        }

        public override string Encode(List<byte> data, VdfSignal signal, bool withUnit)
        {
            if (signal.BitLen > data.Count * 8)
            {
                throw new VdfException("Full signal data length is less than " +
                    "signal [" + signal.Name + "] length");
            }
            if (signal.BitLen % 8 != 0)
            {
                throw new VdfException("ASCII Signal ["
                    + signal.Name + "] bit len must be 8 times");
            }

            StringBuilder sb = new StringBuilder();

            VdfEncoder.ForEachByte(data, signal, (byteData) =>
            {
                sb.Append((char)byteData); //可能存在问题
            });

            return sb.ToString();
        }

        public override void Decode(string value, VdfSignal signal, List<byte> outBytes)
        {
            if (signal.BitLen % 8 != 0)
            {
                throw new VdfException("ASCII Signal ["
                    + signal.Name + "] bit len must be 8 times");
            }

            outBytes.Clear();
            foreach (var ch in value.ToCharArray())
            {
                outBytes.Add((byte)ch);
            }

            // 设置填充字符为空白
            int expectByteLen = signal.BitLen / 8;
            for (int i = outBytes.Count; i < expectByteLen; i++)
            {
                outBytes.Add((byte)' ');
            }
        }
    }
}

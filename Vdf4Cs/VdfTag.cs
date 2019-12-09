using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdf4Cs
{
    public class VdfTag
    {
        // 元素节点名称
        public const string VdfElemName = "Vdf";
        public const string ValueDescTableElemName = "ValueDescTable";
        public const string ValueDescElemName = "ValueDesc";
        public const string EntryElemName = "Entry";
        public const string MessageTableElemName = "MessageTable";
        public const string MessageElemName = "Message";
        public const string SignalElemName = "Signal";

        // 属性节点名称
        public const string VersionAttrName = "Version";
        public const string NameAttrName = "Name";
        public const string TypeAttrName = "Type";
        public const string FactorAttrName = "Factor";
        public const string OffsetAttrName = "Offset";
        public const string FillAttrName = "Fill";
        public const string CanFillAlphaAttrName = "CanFillAlpha";
        public const string DescriptionAttrName = "Description";
        public const string ValueAttrName = "Value";
        public const string ByteLenAttrName = "ByteLen";
        public const string StartBitAttrName = "StartBit";
        public const string BitLenAttrName = "BitLen";
        public const string ByteOrderAttrName = "ByteOrder";
        public const string ValueDescAttrName = "ValueDesc";
        public const string MaximumAttrName = "Maximum";
        public const string MinimumAttrName = "Minimum";
        public const string UnitAttrName = "Unit";
        public const string SeparatorAttrName = "Seperator";
    }
}

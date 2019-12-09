using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vdf4Cs
{
    internal class VdfDocumentLoader
    {
        delegate void ParseHandler(XmlNode vdfNode, VdfDocument vdfDocument);
        private static Dictionary<string, ParseHandler> parseHandlerTable
            = new Dictionary<string, ParseHandler>();

        static VdfDocumentLoader()
        {
            parseHandlerTable.Add("1.0", LoadVersion1_0);
        }

        internal static void Load(string vdfFile, VdfDocument vdfDocument)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(vdfFile);
                Load(doc, vdfDocument);
            }
            catch (Exception ex)
            {
                throw new VdfException(ex.Message, ex.InnerException);
            }
        }

        private static void Load(XmlDocument doc, VdfDocument vdfDocument)
        {
            //获取Vdf根元素
            var elems = doc.GetElementsByTagName(VdfTag.VdfElemName);
            if (elems.Count == 0 || elems.Count > 1)
            {
                throw new VdfException("Vdf element count must be one.");
            }

            //获取版本号
            var vdfRoot = elems[0];
            string version = VdfDocument.Version;
            if (vdfRoot.Attributes[VdfTag.VersionAttrName] != null)
            {
                version = vdfRoot.Attributes[VdfTag.VersionAttrName].Value;
            }

            ParseHandler parser;
            if (parseHandlerTable.TryGetValue(version, out parser))
            {
                parser(vdfRoot, vdfDocument);
            }
            else
            {
                throw new VdfException("Vdf File Version is newer than current Vdf Parser. " +
                    "Please use newest Vdf4Cs library to handle");
            }
        }

        private static void LoadVersion1_0(XmlNode vdfNode, VdfDocument vdfDocument)
        {
            var node = vdfNode.SelectSingleNode(VdfTag.ValueDescTableElemName);
            if (node != null)
            {
                LoadValueDescTable(node, vdfDocument);
            }

            node = vdfNode.SelectSingleNode(VdfTag.MessageTableElemName);
            if (node != null)
            {
                LoadMessageTable(node, vdfDocument);
            }
        }

        private static void LoadMessageTable(XmlNode messageTableNode, VdfDocument vdfDocument)
        {
            vdfDocument.MessageTable = new Dictionary<string, VdfMessage>();
            var nodes = messageTableNode.SelectNodes(VdfTag.MessageElemName);
            if (nodes != null)
            {
                foreach (XmlNode messageNode in nodes)
                {
                    LoadMessage(messageNode, vdfDocument.MessageTable, vdfDocument);
                }
            }
        }

        private static void LoadMessage(XmlNode messageNode, 
            Dictionary<string, VdfMessage> messageTable, VdfDocument vdfDocument)
        {
            string sval;
            if (!XmlUtils.TryGetStringAttrValue(messageNode, VdfTag.NameAttrName, out sval))
            {
                return; //报文如果没有名称，则直接丢弃掉
            }

            VdfMessage message = new VdfMessage();
            message.Name = sval;
            if (XmlUtils.TryGetStringAttrValue(messageNode, VdfTag.DescriptionAttrName, out sval))
            {
                message.Description = sval;
            }

            int ival;
            if (XmlUtils.TryGetIntAttrValue(messageNode, VdfTag.ByteLenAttrName, out ival))
            {
                message.ByteLen = ival;
            }
            messageTable.Add(message.Name, message);

            //解析所有信号
            var nodes = messageNode.SelectNodes(VdfTag.SignalElemName);
            message.SignalTable = new Dictionary<string, VdfSignal>();
            foreach (XmlNode node in nodes)
            {
                LoadSignal(node, message, vdfDocument);
            }
        }

        private static void LoadSignal(XmlNode node, VdfMessage message, VdfDocument vdfDocument)
        {
            string sval;
            
            if (!XmlUtils.TryGetStringAttrValue(node, VdfTag.NameAttrName, out sval))
            {
                return;
            }

            VdfSignal signal = new VdfSignal();
            signal.Name = sval;
            message.SignalTable.Add(signal.Name, signal);

            int ival;
            if (XmlUtils.TryGetIntAttrValue(node, VdfTag.StartBitAttrName, out ival))
            {
                signal.StartBit = ival;
            }
            else
            {
                throw new VdfException("Signal " + signal.Name + " missing StartBit Attribute");
            }

            if (XmlUtils.TryGetIntAttrValue(node, VdfTag.BitLenAttrName, out ival))
            {
                signal.BitLen = ival;
            }
            else
            {
                throw new VdfException("Signal " + signal.Name + " missing BitLen Attribute");
            }

            if (XmlUtils.TryGetStringAttrValue(node, VdfTag.ByteOrderAttrName, out sval))
            {
                VdfByteOrder vdfByteOrder;

                if (!Enum.TryParse<VdfByteOrder>(sval, out vdfByteOrder))
                {
                    throw new VdfException(
                        string.Format("Invalid Byte Order: {0}, Only Can be {1} or {2}",
                        sval, "Intel", "Motorola"));
                }
                signal.ByteOrder = vdfByteOrder;
            }

            if (XmlUtils.TryGetStringAttrValue(node, VdfTag.ValueDescAttrName, out sval))
            {
                signal.ValueDesc = sval;

                //有个要求，只有值描述已经更新过了，Signal中的值描述才会更新
                signal.VdfValueDesc = vdfDocument.ValueDesc(sval);
                if (signal.VdfValueDesc == null)
                {
                    throw new VdfException("ValueDesc " + sval + " Is Undefined Before " +
                        "Define Signal " + signal.Name);
                }
            }
        }

        private static void LoadValueDescTable(XmlNode valueDescTableNode, VdfDocument vdfDocument)
        {
            vdfDocument.ValueDescTable = new Dictionary<string, VdfValueDesc>();
            var nodes = valueDescTableNode.SelectNodes(VdfTag.ValueDescElemName);
            if (nodes != null)
            {
                foreach (XmlNode valueDescNode in nodes)
                {
                    LoadValueDesc(valueDescNode, vdfDocument.ValueDescTable);
                }
            }
        }

        private static void LoadValueDesc(XmlNode valueDescNode, 
            Dictionary<string, VdfValueDesc> valueDescTable)
        {
            string type = null;
            if (valueDescNode.Attributes[VdfTag.TypeAttrName] != null)
            {
                type = valueDescNode.Attributes[VdfTag.TypeAttrName].Value;
            }
            
            if (string.IsNullOrEmpty(type))
            {
                throw new VdfException(string.Format("{0} Element Missing {1} Attribute",
                    VdfTag.ValueDescElemName, VdfTag.TypeAttrName));
            }

            VdfValueDesc.Load(type, valueDescNode, valueDescTable);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vdf4Cs
{
    static class VdfDocumentSaver
    {
        delegate void SaveHandler(XmlDocument xmlDoc, VdfDocument vdfDocument);
        private static Dictionary<string, SaveHandler> saveHandlerTable
            = new Dictionary<string, SaveHandler>();

        static VdfDocumentSaver()
        {
            saveHandlerTable.Add("1.0", SaveVersion1_0);
        }

        internal static void Save(string file, VdfDocument vdfDocument)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", ""));

            SaveHandler handler;
            if (saveHandlerTable.TryGetValue(VdfDocument.Version, out handler))
            {
                handler(xmlDoc, vdfDocument);
            }

            xmlDoc.Save(file);
        }

        private static void SaveVersion1_0(XmlDocument xmlDoc, VdfDocument vdfDocument)
        {
            // 创建根节点
            var rootElem = xmlDoc.CreateElement(VdfTag.VdfElemName);
            rootElem.SetAttribute(VdfTag.VersionAttrName, VdfDocument.Version);
            xmlDoc.AppendChild(rootElem);

            // 保存值描述
            SaveValueDescTable(rootElem, xmlDoc, vdfDocument);

            // 保存报文表
            SaveMessageTable(rootElem, xmlDoc, vdfDocument);
        }

        private static void SaveMessageTable(XmlElement rootElem, XmlDocument xmlDoc, VdfDocument vdfDocument)
        {
            var messageTblElem = xmlDoc.CreateElement(VdfTag.MessageTableElemName);
            rootElem.AppendChild(messageTblElem);

            foreach (var entry in vdfDocument.MessageTable)
            {
                SaveMessage(entry.Value, messageTblElem, xmlDoc, vdfDocument);
            }
        }

        private static void SaveMessage(VdfMessage message, 
            XmlElement messageTblElem, XmlDocument xmlDoc, VdfDocument vdfDocument)
        {
            var messageElem = xmlDoc.CreateElement(VdfTag.MessageElemName);
            messageTblElem.AppendChild(messageElem);

            XmlUtils.SetElemAttribute(messageElem, xmlDoc, VdfTag.NameAttrName, message.Name);
            XmlUtils.SetElemAttribute(messageElem, xmlDoc,
                VdfTag.DescriptionAttrName, message.Description);
            XmlUtils.SetElemAttribute(messageElem, xmlDoc, 
                VdfTag.ByteLenAttrName, message.ByteLen);

            foreach (var signalEntry in message.SignalTable)
            {
                SaveSignal(signalEntry.Value, messageElem, xmlDoc, vdfDocument);
            }
        }

        private static void SaveSignal(VdfSignal signal, XmlElement messageElem,
            XmlDocument xmlDoc, VdfDocument vdfDocument)
        {
            var signalElem = xmlDoc.CreateElement(VdfTag.SignalElemName);
            messageElem.AppendChild(signalElem);

            XmlUtils.SetElemAttribute(signalElem, xmlDoc, VdfTag.NameAttrName, signal.Name);
            XmlUtils.SetElemAttribute(signalElem, xmlDoc, 
                VdfTag.StartBitAttrName, signal.StartBit);
            XmlUtils.SetElemAttribute(signalElem, xmlDoc, VdfTag.BitLenAttrName, signal.BitLen);
            XmlUtils.SetElemAttribute(signalElem, xmlDoc, 
                VdfTag.ByteOrderAttrName, signal.ByteOrder);
            XmlUtils.SetElemAttribute(signalElem, xmlDoc, VdfTag.ValueDescAttrName, signal.ValueDesc);
        }

        private static void SaveValueDescTable(XmlElement rootElem, XmlDocument xmlDoc, VdfDocument vdfDocument)
        {
            var valueDescTblElem = xmlDoc.CreateElement(VdfTag.ValueDescTableElemName);
            rootElem.AppendChild(valueDescTblElem);

            foreach (var valueDesc in vdfDocument.ValueDescTable)
            {
                valueDesc.Value.Save(valueDescTblElem, xmlDoc, vdfDocument);
            }
        }
    }
}

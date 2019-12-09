using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Vdf4Cs;

namespace DcmConfig
{
    // 接口必须显示实现
    class DcmDocument1_1Serializer : DcmDocument1_0Serializer
    {
        protected override void OnBeforeParseDocument(DcmDocument doc, XmlDocument xmlDoc)
        {
            string filePath;
            doc.VdfDocument = DoParseVdf(xmlDoc, out filePath);
            doc.VdfFile = filePath;
        }

        private VdfDocument DoParseVdf(XmlDocument xmlDoc, out string filePath)
        {
            var node = xmlDoc.SelectSingleNode(DcmConfig.ConfigElemName + "/" 
                + DcmConfig.VdfElemName);
            filePath = null;
            if (node == null)
            {
                return null;
            }

            VdfDocument vdfDocument = new VdfDocument();
            string sval;
            if (Vdf4Cs.XmlUtils.TryGetStringAttrValue(node, DcmConfig.FileAttrName, out sval))
            {
                bool tryFileName = false;
                // 猜测文件路径
                if (!File.Exists(sval)) //如果存在则是绝对路径, 不存在则是相对路径
                {
                    sval = Path.Combine(Path.GetDirectoryName(
                        XmlUtils.XmlFilePath(xmlDoc)), sval);
                    tryFileName = true;
                }
                if (File.Exists(sval))
                {
                    if (tryFileName)
                    {
                        filePath = Path.GetFileName(sval);
                    }
                    else
                    {
                        filePath = sval;
                    }
                    vdfDocument.Load(sval);
                }
                else
                {
                    vdfDocument = new VdfDocument(); //建一个的文档
                    filePath = Path.GetFileNameWithoutExtension(xmlDoc.BaseURI) + "Vdf.xml";
                }
            }
            return vdfDocument;
        }

        protected override SubFunction doParseSubFunction(XmlNode subFunctionNode)
        {
            SubFunction subFunction = base.doParseSubFunction(subFunctionNode);

            // 解析值描述
            var node = subFunctionNode.SelectSingleNode(DcmConfig.MessageElemName);
            if (node != null)
            {
                subFunction.Message = node.InnerText.Trim();
            }

            return subFunction;
        }
    }
}

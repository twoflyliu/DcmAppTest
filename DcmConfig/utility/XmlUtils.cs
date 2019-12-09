using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DcmConfig
{
    public static class XmlUtils
    {
        public static string XmlFilePath(XmlDocument xmlDoc)
        {
            string[] fileProtocols = new string[] { @"file:\", @"file://", @"/", @"\"};
            var baseUri = xmlDoc.BaseURI;
            foreach (string protocol in fileProtocols)
            {
                if (baseUri.StartsWith(protocol))
                {
                    baseUri = baseUri.Substring(protocol.Length);
                }
            }
            return baseUri;
        }

        public static void AppendComment(XmlElement parentElem, XmlDocument doc, 
            string comment)
        {
            var commentElem = doc.CreateComment(comment);
            parentElem.AppendChild(commentElem);
        }

        public static void AppendCanIdElement(XmlElement parentElem, XmlDocument doc,
            string name, int canId)
        {
            var elem = doc.CreateElement(name);
            parentElem.AppendChild(elem);
            elem.InnerText = string.Format("0x{0:X3}", canId);
        }

        public static void AppendHexListElement(XmlElement parentElem, XmlDocument doc,
            string name, List<byte> value)
        {
            var elem = doc.CreateElement(name);
            parentElem.AppendChild(elem);

            StringBuilder innerText = new StringBuilder();
            foreach (var data in value)
            {
                innerText.AppendFormat("{0:X2} ", data);
            }
            elem.InnerText = innerText.ToString().Trim();
        }

        public static void AppendElement(XmlElement parentElem, XmlDocument doc,
            string name, object value)
        {
            var elem = doc.CreateElement(name);
            parentElem.AppendChild(elem);
            elem.InnerText = value.ToString();
        }
        
        public static XmlElement CreateElement(XmlElement parentElment, XmlDocument doc, 
            string elemName)
        {
            var elem = doc.CreateElement(elemName);
            parentElment.AppendChild(elem);
            return elem;
        }
    }
}

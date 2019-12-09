using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Vdf4Cs
{
    public static class XmlUtils
    {
        public static bool TryGetIntAttrValue(XmlNode node, string attrName, out int value)
        {
            var attr = node.Attributes[attrName];
            bool ret = false;
            value = 0;
            if (attr != null)
            {
                ret = int.TryParse(attr.Value, out value);
            }
            return ret;
        }

        public static bool TryGetHexIntAttrValue(XmlNode node, string attrName, out int value)
        {
            var attr = node.Attributes[attrName];
            bool ret = false;
            value = 0;
            if (attr != null)
            {
                try
                {
                    value = Convert.ToInt32(attr.Value.ToString(), 16);
                    ret = true;
                }
                catch (Exception)
                {
                }
            }
            return ret;
        }

        public static bool TryGetDoubleAttrValue(XmlNode node, string attrName, out double value)
        {
            var attr = node.Attributes[attrName];
            bool ret = false;
            value = 0;
            if (attr != null)
            {
                ret = double.TryParse(attr.Value, out value);
            }
            return ret;
        }

        public static bool TryGetBoolAttrValue(XmlNode node, string attrName, out bool value)
        {
            var attr = node.Attributes[attrName];
            bool ret = false;
            value = false;
            if (attr != null)
            {
                ret = bool.TryParse(attr.Value, out value);
            }
            return ret;
        }

        public static bool TryGetStringAttrValue(XmlNode node, string attrName, out string value)
        {
            var attr = node.Attributes[attrName];
            bool ret = false;
            value = "";
            if (attr != null)
            {
                value = attr.Value;
                ret = true;
            }
            return ret;
        }

        public static void SetElemAttribute(XmlElement elem, XmlDocument xmlDoc,
            string attrName, object attrValue)
        {
            string value = string.Empty;
            if (attrValue != null)
            {
                value = attrValue.ToString();
            }
            elem.SetAttribute(attrName, value);
        }
    }
}

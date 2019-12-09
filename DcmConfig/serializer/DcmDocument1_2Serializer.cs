using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DcmConfig
{
    class DcmDocument1_2Serializer : DcmDocument1_1Serializer
    {
        protected override SubFunction doParseSubFunction(XmlNode subFunctionNode)
        {
            SubFunction subFunction = base.doParseSubFunction(subFunctionNode);

            // 解析ParsingDirection
            var node = subFunctionNode.SelectSingleNode(DcmConfig.ParsingDirectionElemName);
            if (node != null)
            {
                string name = node.InnerText.Trim();

                ParsingDirection pd;
                if (Enum.TryParse<ParsingDirection>(name, out pd))
                {
                    subFunction.ParsingDirection = pd;
                }
            }

            return subFunction;
        }
    }
}

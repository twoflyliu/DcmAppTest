using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DcmConfig
{
    class DcmDocument1_0Serializer : DcmDocumentSerializer
    {
        public DcmDocument Deserial(XmlDocument xmlDoc)
        {
            DcmDocument doc = new DcmDocument();
            OnBeforeParseDocument(doc, xmlDoc);

            var elems = xmlDoc.GetElementsByTagName(DcmConfig.ServicesElemName);
            doc.Services = doParseServices(elems);

            OnDeserialOK(doc, xmlDoc);
            return doc;
        }

        protected virtual void OnDeserialOK(DcmDocument doc,
            XmlDocument xmlDoc)
        {

        }

        protected virtual void OnBeforeParseDocument(DcmDocument doc, XmlDocument xmlDoc)
        {

        }

        protected virtual List<Service> doParseServices(XmlNodeList servicesNodeList)
        {
            List<Service> result = new List<Service>();

            if (servicesNodeList.Count == 1)
            {
                var servicesNode = servicesNodeList[0];
                XmlNodeList serviceNodeList = servicesNode.SelectNodes(DcmConfig.ServiceElemName);

                foreach (XmlNode serviceNode in serviceNodeList)
                {
                    result.Add(doParseService(serviceNode));
                }
            }
            else if (servicesNodeList.Count > 1)
            {
                throw new DcmFileFormatException("Dcm File can olny has" +
                    " one " + DcmConfig.ServicesElemName
                    + " Element");
            }

            return result;
        }

        protected virtual Service doParseService(XmlNode serviceNode)
        {
            Service service = new Service();

            // 解析节点名称
            XmlNode nameNode = serviceNode.SelectSingleNode(DcmConfig.NameElemName);
            if (nameNode != null)
            {
                service.Name = nameNode.InnerText;
            }

            // 解析子功能
            XmlNode subFunctionsNode = serviceNode.SelectSingleNode(DcmConfig.SubFunctionsElemName);
            if (subFunctionsNode != null)
            {
                service.SubFunctions = new List<SubFunction>();
                XmlNodeList subFunctionNodeList =
                    subFunctionsNode.SelectNodes(DcmConfig.SubFunctionElemName);
                foreach (XmlNode subFunctionNode in subFunctionNodeList)
                {
                    service.SubFunctions.Add(doParseSubFunction(subFunctionNode));
                }
            }

            return service;
        }

        protected virtual SubFunction doParseSubFunction(XmlNode subFunctionNode)
        {
            SubFunction subFunction = new SubFunction();

            // 解析名称
            XmlNode node = subFunctionNode.SelectSingleNode(DcmConfig.NameElemName);
            if (node != null)
            {
                subFunction.Name = node.InnerText;
            }

            // 解析前缀
            node = subFunctionNode.SelectSingleNode(DcmConfig.PrefixElemName);
            if (node != null)
            {
                doParsePrefix(node, subFunction);
            }

            // 解析类型
            node = subFunctionNode.SelectSingleNode(DcmConfig.TypeElemName);
            if (node != null)
            {
                doParseDataType(node, subFunction);
            }

            // 解析长度
            node = subFunctionNode.SelectSingleNode(DcmConfig.LenElemName);
            if (node != null)
            {
                doParseDataLen(node, subFunction);
            }

            //解析地址
            node = subFunctionNode.SelectSingleNode(DcmConfig.AddressElemName);
            if (node != null)
            {
                doParseAddressType(node, subFunction);
            }

            return subFunction;
        }

        protected void doParseAddressType(XmlNode node, SubFunction subFunction)
        {
            try
            {
                CanAddressType dataType;
                if (Enum.TryParse<CanAddressType>(node.InnerText, out dataType))
                {
                    subFunction.CanAddressType = dataType;
                }
            }
            catch (Exception)
            {
                subFunction.CanAddressType = DcmConfig.DefaultCanAddressType;
            }
        }

        protected void doParseDataLen(XmlNode node, SubFunction subFunction)
        {
            try
            {
                subFunction.DataLen = Convert.ToInt32(node.InnerText);
            }
            catch (Exception)
            {
                subFunction.DataLen = 0;
            }
        }

        protected void doParseDataType(XmlNode node, SubFunction subFunction)
        {
            try
            {
                DataType dataType;
                if (Enum.TryParse<DataType>(node.InnerText, out dataType))
                {
                    subFunction.DataType = dataType;
                }
            }
            catch (Exception)
            {
                subFunction.DataType = DcmConfig.DefaultDataType;
            }
        }

        protected void doParsePrefix(XmlNode node, SubFunction subFunction)
        {
            subFunction.Prefix = new List<byte>();
            try
            {
                var dataStrList = node.InnerText.Trim().Split(" \t\r\n".ToCharArray());
                foreach (string dataStr in dataStrList)
                {
                    subFunction.Prefix.Add(Convert.ToByte(dataStr, 16));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void Serial(DcmDocument doc, XmlDocument xmlDoc, string file)
        {
            throw new NotImplementedException();
        }
    }
}

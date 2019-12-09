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
    class DcmDocument1_3Serializer: DcmDocument1_2Serializer
    {
        bool TryGetHexIntInnerText(XmlNode node, out int result)
        {
            bool ret = true;
            result = 0;
            try
            {
                result = Convert.ToInt32(node.InnerText, 16);
            }
            catch (Exception)
            {
                ret = false;
            }
            return ret;
        }

        bool TryGetIntInnerText(XmlNode node, out int result)
        {
            bool ret = true;
            result = 0;
            try
            {
                result = Convert.ToInt32(node.InnerText);
            }
            catch (Exception)
            {
                ret = false;
            }
            return ret;
        }

        bool TryGetBoolInnerText(XmlNode node, out bool value)
        {
            value = false;
            if (node == null)
            {
                return false;
            }
            return bool.TryParse(node.InnerText, out value);
        }

        protected override void OnBeforeParseDocument(DcmDocument doc, XmlDocument xmlDoc)
        {
            base.OnBeforeParseDocument(doc, xmlDoc);

            // 解析配置
            XmlNode node = xmlDoc.SelectSingleNode(DcmConfig.ConfigElemName + "/" +
                DcmConfig.CfgElemName);
            if (node != null)
            {
                DoParseCfg(doc, node);
            }
        }

        public virtual void DoParseCfg(DcmDocument doc, XmlNode cfgNode)
        {
            doc.Config = new Config();
            var cfg = doc.Config;

            // 解析CAN ID
            XmlNode node = cfgNode.SelectSingleNode(DcmConfig.PhysicalRequestIdElemName);
            int ival;
            if (TryGetHexIntInnerText(node, out ival))
            {
                cfg.PhysicalRequestId = ival;
            }

            node = cfgNode.SelectSingleNode(DcmConfig.FunctionRequestidElemName);
            if (TryGetHexIntInnerText(node, out ival))
            {
                cfg.FunctionRequestId = ival;
            }

            node = cfgNode.SelectSingleNode(DcmConfig.ResponseIdElemName);
            if (TryGetHexIntInnerText(node, out ival))
            {
                cfg.ResponseId = ival;
            }

            node = cfgNode.SelectSingleNode(DcmConfig.SecurityAccessTypeElemName);
            if (node != null)
            {
                cfg.SecurityAccessType = node.InnerText.Trim();
            }

            node = cfgNode.SelectSingleNode(DcmConfig.CanTickEnabledElemName);
            bool bval;
            if (TryGetBoolInnerText(node, out bval))
            {
                cfg.CanTickEnabled = bval;
            }

            node = cfgNode.SelectSingleNode(DcmConfig.CanTickPeriodElemName);
            if (TryGetIntInnerText(node, out ival))
            {
                cfg.CanTickPeriod = ival;
            }

            node = cfgNode.SelectSingleNode(DcmConfig.SuppressTickResponseElemName);
            if (TryGetBoolInnerText(node, out bval))
            {
                cfg.SuppressTickResponse = bval;
            }
        }

        // 之前的版本都没有实现保存功能，当前版本才进行实现
        public override void Serial(DcmDocument doc, XmlDocument xmlDoc, string file)
        {
            SerialRoot(doc, xmlDoc, file);        //保存根部
        }

        protected virtual void SerialRoot(DcmDocument doc, XmlDocument xmlDoc, string file)
        {
            // 保存根
            var element = xmlDoc.CreateElement(DcmConfig.ConfigElemName);
            element.SetAttribute(DcmConfig.ConfigVersionAttrName, DcmConfig.CurrVersion);
            xmlDoc.AppendChild(element);

            SerialVdf(element, doc, xmlDoc, file);         //保存Vdf
            SerialConfig(element, doc, xmlDoc);      //保存配置
            SerialServices(element, doc, xmlDoc);    //保存所有服务
        }

        protected virtual void SerialServices(XmlElement rootElement, DcmDocument doc,
            XmlDocument xmlDoc)
        {
            var servicesElem = XmlUtils.CreateElement(rootElement, xmlDoc,
                DcmConfig.ServicesElemName);

            foreach (var service in doc.Services)
            {
                var serviceElem = XmlUtils.CreateElement(servicesElem, xmlDoc,
                    DcmConfig.ServiceElemName);
                SerialService(serviceElem, service, doc, xmlDoc);
            }
        }

        protected virtual void SerialService(XmlElement serviceElem, Service service, 
            DcmDocument doc, XmlDocument xmlDoc)
        {
            XmlUtils.AppendElement(serviceElem, xmlDoc, DcmConfig.NameElemName,
                service.Name);
            SerialSubFunctions(serviceElem, service, doc, xmlDoc);
        }

        protected virtual void SerialSubFunctions(XmlElement serviceElem, Service service,
            DcmDocument doc, XmlDocument xmlDoc)
        {
            var subFunctionsElem = XmlUtils.CreateElement(serviceElem, xmlDoc, DcmConfig.SubFunctionsElemName);
            foreach (var subFunction in service.SubFunctions)
            {
                SerialSubFunction(subFunctionsElem, subFunction, doc, xmlDoc);
            }
        }

        protected virtual void SerialSubFunction(XmlElement subFunctionsElem, SubFunction subFunction,
            DcmDocument doc, XmlDocument xmlDoc)
        {
            var subFunctionElem = XmlUtils.CreateElement(subFunctionsElem, xmlDoc,
                DcmConfig.SubFunctionElemName);
            XmlUtils.AppendElement(subFunctionElem, xmlDoc, DcmConfig.NameElemName,
                subFunction.Name);
            XmlUtils.AppendHexListElement(subFunctionElem, xmlDoc, DcmConfig.PrefixElemName,
                subFunction.Prefix);
            XmlUtils.AppendElement(subFunctionElem, xmlDoc, DcmConfig.TypeElemName,
                subFunction.DataType);
            XmlUtils.AppendElement(subFunctionElem, xmlDoc, DcmConfig.LenElemName,
                subFunction.DataLen);
            XmlUtils.AppendElement(subFunctionElem, xmlDoc, DcmConfig.AddressElemName,
                subFunction.CanAddressType);

            if (subFunction.Message != null)
            {
                XmlUtils.AppendElement(subFunctionElem, xmlDoc, DcmConfig.MessageElemName,
                    subFunction.Message);
            }

            if (subFunction.ParsingDirection != ParsingDirection.None)
            {
                XmlUtils.AppendElement(subFunctionElem, xmlDoc,
                    DcmConfig.ParsingDirectionElemName, subFunction.ParsingDirection);
            }
        }

        protected virtual void SerialConfig(XmlElement rootElement, DcmDocument doc,
            XmlDocument xmlDoc)
        {
            var configElement = xmlDoc.CreateElement(DcmConfig.CfgElemName);
            rootElement.AppendChild(configElement);

            XmlUtils.AppendComment(configElement, xmlDoc, "Can Id设置");
            XmlUtils.AppendCanIdElement(configElement, xmlDoc,
                DcmConfig.PhysicalRequestIdElemName, doc.Config.PhysicalRequestId);
            XmlUtils.AppendCanIdElement(configElement, xmlDoc,
                DcmConfig.FunctionRequestidElemName, doc.Config.FunctionRequestId);
            XmlUtils.AppendCanIdElement(configElement, xmlDoc,
                DcmConfig.ResponseIdElemName, doc.Config.ResponseId);

            XmlUtils.AppendComment(configElement, xmlDoc, "安全访问设置");
            XmlUtils.AppendElement(configElement, xmlDoc,
                DcmConfig.SecurityAccessTypeElemName, doc.Config.SecurityAccessType);

            XmlUtils.AppendComment(configElement, xmlDoc, "下面是维持在线相关设置");
            XmlUtils.AppendElement(configElement, xmlDoc,
                DcmConfig.CanTickEnabledElemName, doc.Config.CanTickEnabled);
            XmlUtils.AppendElement(configElement, xmlDoc,
                DcmConfig.CanTickPeriodElemName, doc.Config.CanTickPeriod);
            XmlUtils.AppendElement(configElement, xmlDoc,
                DcmConfig.SuppressTickResponseElemName, doc.Config.SuppressTickResponse);
        }

        // 保存Vdf设置
        protected virtual void SerialVdf(XmlElement rootElement, DcmDocument doc,
            XmlDocument xmlDoc, string file)
        {
            var vdfElement = xmlDoc.CreateElement(DcmConfig.VdfElemName);

            if (string.IsNullOrEmpty(doc.VdfFile))
            {
                doc.VdfFile = Path.GetFileNameWithoutExtension(file) + "Vdf.xml";
            }

            vdfElement.SetAttribute(DcmConfig.FileAttrName, doc.VdfFile);
            rootElement.AppendChild(vdfElement);

            // 保存vdfw文档
            if (doc.VdfDocument != null)
            {
                string fullPath = doc.VdfFile;

                if (!Path.IsPathRooted(doc.VdfFile)) //相对路径
                {
                    var dirName = Path.GetDirectoryName(file);
                    fullPath = Path.Combine(dirName, doc.VdfFile);
                }
                doc.VdfDocument.Save(fullPath);
            }
        }

        protected override void OnDeserialOK(DcmDocument doc,
            XmlDocument xmlDoc)
        {
            base.OnDeserialOK(doc, xmlDoc);

            // 内部缓存所有的VdfMessage
            var vdfDocument = doc.VdfDocument;
            if (vdfDocument == null)
            {
                return;
            }

            foreach (var service in doc.Services)
            {
                foreach (var subFunction in service.SubFunctions)
                {
                    if (!string.IsNullOrEmpty(subFunction.Message))
                    {
                        VdfMessage vdfMessage = vdfDocument.Message(subFunction.Message);
                        if (vdfMessage != null)
                        {
                            subFunction.VdfMessage = vdfMessage;
                        }
                    }
                }
            }
        }
    }
}

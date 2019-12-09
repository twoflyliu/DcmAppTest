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
    public static class DcmConfig
    {
        public const string ConfigVersionAttrName = "Version";
        public const string ConfigElemName = "Config";
        public const string ServicesElemName = "Services";
        public const string ServiceElemName = "Service";
        public const string NameElemName = "Name";
        public const string SubFunctionsElemName = "SubFunctions";
        public const string SubFunctionElemName = "SubFunction";
        public const string PrefixElemName = "Prefix";
        public const string TypeElemName = "Type";
        public const string LenElemName = "Len";
        public const string AddressElemName = "Address";

        public const string VdfElemName = "Vdf";
        public const string FileAttrName = "File";

        public const string MessageElemName = "Message";
        public const string ParsingDirectionElemName = "ParsingDirection";

        public const string CfgElemName = "Cfg";
        public const string PhysicalRequestIdElemName = "PhysicalRequestId";
        public const string FunctionRequestidElemName = "FunctionRequestId";
        public const string ResponseIdElemName = "ResponseId";
        public const string SecurityAccessTypeElemName = "SecurityAccessType";
        public const string CanTickEnabledElemName = "CanTickEnabled";
        public const string CanTickPeriodElemName = "CanTickPeriod";
        public const string SuppressTickResponseElemName = "SuppressTickResponse";


        public const string DefaultVersion = "1.0";    //读的时候默认版本号
        public const string CurrVersion = "1.3";       //写的时候默认版本号

        public const DataType DefaultDataType = DataType.Hex;
        public const CanAddressType DefaultCanAddressType = CanAddressType.Physical;

        private static Dictionary<string, DcmDocumentSerializer>
            parserMap = new Dictionary<string, DcmDocumentSerializer>();

        static DcmConfig()
        {
            parserMap.Add("1.0", new DcmDocument1_0Serializer());
            parserMap.Add("1.1", new DcmDocument1_1Serializer());
            parserMap.Add("1.2", new DcmDocument1_2Serializer());
            parserMap.Add("1.3", new DcmDocument1_3Serializer());
        }

        /// <summary>
        /// 加载指定配置文件
        /// </summary>
        /// <param name="configFile">配置文件名称</param>
        /// <returns>加载好的文档</returns>
        public static DcmDocument LoadFile(string configFile)
        {
            try
            {
                // 加载文档
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configFile);

                return LoadXmlDom(xmlDoc);
            }
            catch (Exception ex)
            {
                if (ex is DcmFileFormatException)
                {
                    throw;
                }
                else
                {
                    throw new DcmFileFormatException(ex.Message, ex.InnerException);
                }
            }
        }

        public static DcmDocument LoadXmlString(string xmlString)
        {
            try
            {
                // 加载文档
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlString);

                return LoadXmlDom(xmlDoc);
            }
            catch (Exception ex)
            {
                if (ex is DcmFileFormatException)
                {
                    throw;
                }
                else
                {
                    throw new DcmFileFormatException(ex.Message, ex.InnerException);
                }
            }
        }

        public static DcmDocument LoadXmlDom(XmlDocument xmlDoc)
        {
            // 获取版本号
            var elems = xmlDoc.GetElementsByTagName(ConfigElemName);
            if (elems == null || elems.Count == 0)
            {
                throw new DcmFileFormatException("Config File missing " + ConfigElemName +
                    " Element");
            }

            var configElem = elems[0];
            string version = DefaultVersion;
            if (configElem.Attributes[ConfigVersionAttrName] != null)
            {
                version = configElem.Attributes[ConfigVersionAttrName].Value;
            }

            DcmDocumentSerializer parser = GetSerializer(version);
            var result = parser.Deserial(xmlDoc);
            result.Version = version;
            return result;
        }

        public static void Save(DcmDocument document, string file)
        {
            try
            {
                DoSave(document, file);
            }
            catch (DcmFileFormatException)
            {
                document.Version = CurrVersion;
                DoSave(document, file);
            }
        }

        private static void DoSave(DcmDocument document, string file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", ""));

            DcmDocumentSerializer serializer = GetSerializer(document.Version);
            serializer.Serial(document, xmlDoc, file);

            xmlDoc.Save(file);
        }

        private static DcmDocumentSerializer GetSerializer(string version)
        {
            DcmDocumentSerializer parser;
            if (parserMap.TryGetValue(version, out parser))
            {
                return parser;
            }
            else
            {
                throw new DcmFileFormatException("Current config file version is newer than" +
                    " current library. Please install newest library");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmService
{
    interface DcmAppDataParser
    {
        List<KeyValuePair<string, string>> Parse(List<byte> data, out bool postiveResp);
        void AddDcmAppDataParser(DcmAppDataParser parser);
        void RemoveDcmAppDataParser(DcmAppDataParser parser);
    }

    abstract class DcmAppDataParserAdapter : DcmAppDataParser
    {
        public void AddDcmAppDataParser(DcmAppDataParser parser)
        {
        }

        public List<KeyValuePair<string, string>> Parse(List<byte> data, out bool postiveResp)
        {
            return DoParse(data, out postiveResp);
        }

        protected  abstract List<KeyValuePair<string, string>> DoParse(List<byte> data
            , out bool postiveRes);
        

        public void RemoveDcmAppDataParser(DcmAppDataParser parser)
        {
        }
    }

    class DcmAppDataParserGroup : DcmAppDataParser
    {
        private List<DcmAppDataParser> parsers = new List<DcmAppDataParser>();

        public void AddDcmAppDataParser(DcmAppDataParser parser)
        {
            parsers.Add(parser);
        }

        public void RemoveDcmAppDataParser(DcmAppDataParser parser)
        {
            parsers.Remove(parser);
        }

        public List<KeyValuePair<string, string>> Parse(List<byte> data, out bool postiveRes)
        {
            List<KeyValuePair<string, string>> result;
            postiveRes = false;
            foreach (var parser in parsers)
            {
                result = parser.Parse(data, out postiveRes);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DcmConfig
{
    interface DcmDocumentSerializer
    {
        DcmDocument Deserial(XmlDocument xmlDoc);
        void Serial(DcmDocument doc, XmlDocument xmlDoc, string file);
    }
}

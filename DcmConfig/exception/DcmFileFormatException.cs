using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DcmConfig
{
    public class DcmFileFormatException : Exception
    {
        public DcmFileFormatException(string msg)
            : base(msg)
        {

        }

        public DcmFileFormatException(string msg, Exception innerException)
            : base(msg, innerException)
        {

        }
    }
}

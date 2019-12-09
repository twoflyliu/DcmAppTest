using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdf4Cs
{
    class VdfException : Exception
    {
        public VdfException(string msg)
            : base(msg)
        {

        }

        public VdfException(string msg, Exception innerException)
            : base(msg, innerException)
        {

        }
    }
}

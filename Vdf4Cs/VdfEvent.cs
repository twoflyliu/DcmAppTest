using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdf4Cs
{
    public class ValueDescriptionChangedEventArgs
    {
        public VdfValueDesc ValueDesc { get; set; }
    }

    public delegate void
        ValueDescriptionChangedEventHandler(ValueDescriptionChangedEventArgs e);

    public class MessageChangedEventArgs
    {
        public VdfMessage Message { get; set; }
    }

    public delegate void
        MessageChangedEventHandler(MessageChangedEventArgs e);

    public class SignalChangedEventArgs
    {
        public VdfSignal Signal { get; set; }
    }
    public delegate void
        SignalChangedEventHandler(SignalChangedEventArgs e);
}

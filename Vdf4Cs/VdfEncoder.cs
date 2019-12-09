using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdf4Cs
{
    public static class VdfEncoder
    {
        // 对信号的值进行编码
        // 信号值从data中获取
        public static string Encode(List<byte> data, VdfSignal signal, bool withUnit=false)
        {
            if (signal.VdfValueDesc == null)
            {
                throw new VdfException("Signal Value Desc cannot be null");
            }

            string ret;

            // 使用两个编码接口，
            // 因为BCD编码使用传整形值是不能满足需求的，必须传递字节数组，并且需要直到，字节序
            try
            {
                uint value = 0;
                switch (signal.ByteOrder)
                {
                    case VdfByteOrder.Intel:
                        value = ValueUtils.GetValueIntel(data, signal.StartBit, signal.BitLen);
                        break;
                    case VdfByteOrder.Motorola:
                        value = ValueUtils.GetValueMotorola(data, signal.StartBit, signal.BitLen);
                        break;
                    default:
                        throw new VdfException("Invalid Byte Order: " + signal.ByteOrder.ToString());
                }
                ret = signal.VdfValueDesc.Encode(value, withUnit);
            }
            catch (NotImplementedException)
            {
                ret = signal.VdfValueDesc.Encode(data, signal, withUnit);
            }

            return ret;
        }

        public static void ForEachByte(List<byte> data, VdfSignal signal,
            Action<byte> action)
        {
            switch(signal.ByteOrder)
            {
                case VdfByteOrder.Intel:
                    ValueUtils.ForEachByteIntel(data, signal.StartBit, signal.BitLen,
                        action);
                    break;
                case VdfByteOrder.Motorola:
                    ValueUtils.ForEachByteMotorola(data, signal.StartBit, signal.BitLen,
                        action);
                    break;
                default:
                    throw new ArgumentException("VdfEncoder.ForEachByte: " +
                        "Invalid Signal ByteOrder");
            }
        }
    }
}

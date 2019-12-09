using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdf4Cs
{
    public static class VdfDecoder
    {
        // 对value进行解码，将结果更新到data中
        public static void Decode(List<byte> data, VdfSignal signal, string value)
        {
            if (signal.VdfValueDesc == null)
            {
                throw new VdfException("Signal Value Desc cannot be null");
            }

            try
            {
                decodeByValue(data, signal, value);
            }
            catch (NotImplementedException)
            {
                decodeByByteArr(data, signal, value);
            }
        }

        private static void decodeByByteArr(List<byte> data, VdfSignal signal, string value)
        {
            List<byte> outBytes = new List<byte>();
            signal.VdfValueDesc.Decode(value, signal, outBytes);

            int index = 0;

            Action<List<byte>, int, int, uint> setValue = null;
            switch (signal.ByteOrder)
            {
                case VdfByteOrder.Intel:
                    setValue = ValueUtils.SetValueIntel;
                    break;
                case VdfByteOrder.Motorola:
                    setValue = ValueUtils.SetValueMotorola;
                    break;
                default:
                    throw new VdfException("Invalid Byte Order: " + signal.ByteOrder.ToString());
            }

            ForEachByte(data, signal, (sbit, blen, val) =>
            {
                setValue(data, sbit, blen, outBytes[index++]);
            });
        }

        public static void ForEachByte(List<byte> data, VdfSignal signal,
            Action<int, int, byte> action)
        {
            switch (signal.ByteOrder)
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

        private static void decodeByValue(List<byte> data, VdfSignal signal, string value)
        {
            // 按照值的方式
            uint decodeValue = signal.VdfValueDesc.Decode(value);
            switch (signal.ByteOrder)
            {
                case VdfByteOrder.Intel:
                    ValueUtils.SetValueIntel(data, signal.StartBit, signal.BitLen, decodeValue);
                    break;
                case VdfByteOrder.Motorola:
                    ValueUtils.SetValueMotorola(data, signal.StartBit, signal.BitLen, decodeValue);
                    break;
                default:
                    throw new VdfException("Invalid Byte Order: " + signal.ByteOrder.ToString());
            }
        }
    }
}

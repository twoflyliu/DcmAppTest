using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdf4Cs
{
    public static class ValueUtils
    {
        public static uint GetValueIntel(List<byte> data, int startBit, int bitLen)
        {
            uint value = 0;

            // 起始长度(MSB行）
            int startRowIndex = startBit / 8;
            int startBegColIndex = startBit % 8;
            int startRowBitLen = (8 - startBegColIndex);
            int startRowOffsetBit = startBegColIndex;

            if (startRowBitLen > bitLen) {
                startRowBitLen = bitLen;
            }

            //最低行(LSB)
            value = (uint)((data[startRowIndex]>>startRowOffsetBit) & ((1<<startRowBitLen)-1));

            // 中间行
            int curRowIndex = startRowIndex + 1;
            int offsetBit = startRowBitLen;
            int midRowCnt = (bitLen - startRowBitLen) / 8;
            for (int i = 0; i<midRowCnt; i++) {
                value |= (uint)(data[curRowIndex++] << offsetBit);
                offsetBit += 8;
            }

            // 最后一行
            int lastRowBitLen = bitLen - (8 * midRowCnt) - startRowBitLen;
            if (lastRowBitLen > 0) {
                value |= (uint)((data[curRowIndex] & ((1 << lastRowBitLen)-1)) << offsetBit);
            }

            return value;
        }

        //startBit是MSB
        public static uint GetValueMotorola(List<byte> data, int startBit, int bitLen)
        {
            uint value = 0;

            // 起始长度(MSB行）
            int startRowIndex = startBit / 8;
            int startEndColIndex = startBit % 8;
            int startRowBitLen = startEndColIndex + 1;
            int startRowMaxBitLen = startRowBitLen;
            int startRowOffsetBit = 0;

            if (bitLen < startRowBitLen)
            {
                startRowOffsetBit = startRowBitLen - bitLen;
                startRowBitLen = bitLen;
            }

            //最低行(MSB)
            value = (uint)((data[startRowIndex] & ((1 << startRowMaxBitLen) - 1)) >> startRowOffsetBit);

            // 中间行
            int curRowIndex = startRowIndex + 1;
            int midRowCnt = (bitLen - startRowBitLen) / 8;
            int offsetBit = startRowBitLen;
            for (int i = 0; i < midRowCnt; i++)
            {
                value = (value << offsetBit) | data[curRowIndex++];
                offsetBit = 8;
            }

            // 最后一行
            int lastRowBitLen = bitLen - (8 * midRowCnt) - startRowBitLen;
            if (lastRowBitLen > 0)
            {
                value = (value << lastRowBitLen) | (uint)(data[curRowIndex] >> (8 - lastRowBitLen));
            }

            return value;
        }

        public static void SetValueIntel(List<byte> data, int startBit, int bitLen, uint value)
        {
            // 起始长度(MSB行）信息
            int startRowIndex = startBit / 8;
            int startBegColIndex = startBit % 8;
            int startRowBitLen = (8 - startBegColIndex);
            int startRowOffsetBit = startBegColIndex;

            if (startRowBitLen > bitLen)
            {
                startRowBitLen = bitLen;
            }

            // 中间行信息
            int midRowCnt = (bitLen - startRowBitLen) / 8;

            // 最后行信息
            int lastRowBitLen = bitLen - (8 * midRowCnt) - startRowBitLen;
            int curRowIndex = startRowIndex;

            // 设置最低行(LSB)
            int mask = ((1 << startRowBitLen) - 1) << startRowOffsetBit;
            data[curRowIndex] &= (byte)(~mask);
            data[curRowIndex++] |= (byte)((value & ((1 << startRowBitLen) - 1)) << startRowOffsetBit);
            value >>= startRowBitLen;

            // 设置中间行
            for (int i = 0; i < midRowCnt; i++)
            {
                data[curRowIndex++] = (byte)(value & 0xff);
                value >>= 8;
            }

            // 设置最后行
            if (lastRowBitLen > 0)
            {
                mask = ((1 << lastRowBitLen) - 1);
                data[curRowIndex] &= (byte)(~mask);
                data[curRowIndex] |= (byte)(value & ((1 << lastRowBitLen) - 1));
            }
        }

        public static void SetValueMotorola(List<byte> data, int startBit, int bitLen, uint value)
        {
            // 起始长度(MSB行）信息
            int startRowIndex = startBit / 8;
            int startEndColIndex = startBit % 8;
            int startRowBitLen = startEndColIndex + 1;
            int startRowMaxBitLen = startRowBitLen;
            int startRowOffsetBit = 0;
            int mask = 0;

            if (startRowBitLen > bitLen)
            {
                startRowOffsetBit = startRowBitLen - bitLen;
                startRowBitLen = bitLen;
            }

            // 中间行信息
            int midRowCnt = (bitLen - startRowBitLen) / 8;

            // 最后行信息
            int lastRowBitLen = bitLen - (8 * midRowCnt) - startRowBitLen;

            int curRowIndex = startRowIndex;

            // 设置起始行（MSB）
            mask = (((1 << startRowMaxBitLen) - 1) >> startRowOffsetBit) << startRowOffsetBit;
            data[curRowIndex] &= (byte)(~mask);
            data[curRowIndex++] |= (byte)(((value >> (bitLen - startRowBitLen)) & ((1 << startRowBitLen) - 1))
                    << startRowOffsetBit);

            // 设置中间行
            for (int i = 0; i < midRowCnt; ++i)
            {
                data[curRowIndex++] = (byte)((value >> (lastRowBitLen + (midRowCnt - 1 - i) * 8)) & 0xff);
            }

            // 设置最后行
            if (lastRowBitLen > 0)
            {
                mask = ((1 << lastRowBitLen) - 1) << (8 - lastRowBitLen);
                data[curRowIndex] &= (byte)(~mask);
                data[curRowIndex] |= (byte)((value & ((1 << lastRowBitLen) - 1)) << (8 - lastRowBitLen));
            }
        }

        public static void ForEachByteIntel(List<byte> data, int startBit, int bitLen,
            Action<int, int, byte> action)
        {
            if (startBit < 0 || bitLen < 0)
            {
                throw new ArgumentException("ForEachByteMotorola: startBit and bitLen must be >= 0");
            }

            if (bitLen % 8 != 0)
            {
                throw new ArgumentException("ForEachByteIntel: bitLen must be 8 times");
            }

            if (action == null)
            {
                return;
            }

            int leftByteLen = bitLen / 8;
            const int SingleByteBitLen = 8;
            while (leftByteLen > 0)
            {
                byte value = (byte)GetValueIntel(data, startBit, 8);
                action(startBit, SingleByteBitLen, value);

                startBit += 8;
                leftByteLen--;
            }
        }

        public static void ForEachByteIntel(List<byte> data, int startBit, int bitLen,
            Action<byte> action)
        {
            if (null == action)
            {
                return;
            }

            ForEachByteIntel(data, startBit, bitLen, (sbit, blen, value) =>
            {
                action(value);
            });
        }

        public static void ForEachByteMotorola(List<byte> data, int startBit, int bitLen,
            Action<int, int, byte> action)
        {
            if (startBit < 0 || bitLen < 0)
            {
                throw new ArgumentException("ForEachByteMotorola: startBit and bitLen must be >= 0");
            }

            if (bitLen % 8 != 0)
            {
                throw new ArgumentException("ForEachByteMotorola: bitLen must be 8 times");
            }

            if (action == null)
            {
                return;
            }

            int leftByteLen = bitLen / 8;
            const int SingleByteBitLen = 8;
            while (leftByteLen > 0)
            {
                byte value = (byte)GetValueMotorola(data, startBit, 8);
                action(startBit, SingleByteBitLen, value);

                startBit -= 8;
                leftByteLen--;
            }
        }

        public static void ForEachByteMotorola(List<byte> data, int startBit, int bitLen,
            Action<byte> action)
        {
            if (null == action)
            {
                return;
            }

            ForEachByteMotorola(data, startBit, bitLen, (sbit, blen, value) =>
            {
                action(value);
            });
        }
    }
}

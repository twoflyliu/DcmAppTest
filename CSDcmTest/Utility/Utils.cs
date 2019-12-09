using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSDcmTest
{
    static class Utils
    {
        public static string HexArrayToString(List<byte> hexArray)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte hex in hexArray)
            {
                sb.AppendFormat("{0:X2} ", hex);
            }
            return sb.ToString().Trim();
        }

        public static string ByteToHex(byte data)
        {
            return string.Format("{0:X2}", data);
        }

        public static byte HexToByte(string hex)
        {
            return Convert.ToByte(hex, 16);
        }

        public static void DataGridViewAddRows<T,U>(DataGridView dataGridView,
            List<KeyValuePair<T,U>> rows)
        {
            foreach (var entry in rows)
            {
                dataGridView.Rows.Add(new string[] { entry.Key.ToString(),
                    entry.Value.ToString()});
            }
        }

        public static void DataGridViewSaveRow(StreamWriter writer, 
            DataGridView dataGridView, DataGridViewRow row, string seperator
            , bool eol = false)
        {
            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                writer.Write(row.Cells[i].Value.ToString());
                if (i != dataGridView.ColumnCount-1)
                {
                    writer.Write(seperator);
                }
            }
            if (eol)
            {
                writer.WriteLine();
            }
        }

        public static List<byte> NewInitializedList(int len, byte defaultVal = 0)
        {
            var ret = new List<byte>();
            for (int i = 0; i < len; i++)
            {
                ret.Add(defaultVal);
            }
            return ret;
        }

        public static List<byte> NewInitializedList(int len, List<byte> oldList, byte defaultVal = 0)
        {
            var ret = new List<byte>();
            for (int i = 0; i < len; i++)
            {
                if (oldList != null && i < oldList.Count)
                {
                    ret.Add(oldList[i]);
                }
                else
                {
                    ret.Add(defaultVal);
                }
            }
            return ret;
        }
    }
}

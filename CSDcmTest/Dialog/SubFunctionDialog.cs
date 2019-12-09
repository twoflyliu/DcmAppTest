using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSDcmTest.Dialog
{
    public partial class SubFunctionDialog : Form
    {
        public enum Operation
        {
            New = 0,
            Update
        }

        public const string Empty = "无";

        public SubFunctionDialog(Operation operation, Vdf4Cs.VdfDocument vdoc)
        {
            InitializeComponent();
            Text = (operation == Operation.New) ? "新建子功能" : "编辑子功能";

            // 初始化地址类型
            var arr = Enum.GetValues(typeof(DcmConfig.CanAddressType));
            int index = 0;
            foreach (var item in arr)
            {
                comboBoxAddress.Items.Add(item.ToString());
                if (item.ToString().Equals(DcmConfig.CanAddressType.Physical.ToString()))
                {
                    comboBoxAddress.SelectedIndex = index;
                }
                ++index;
            }

            // 初始化解析方向
            arr = Enum.GetValues(typeof(DcmConfig.ParsingDirection));
            index = 0;
            foreach (var item in arr)
            {
                comboBoxParsingDirection.Items.Add(item.ToString());
                if (item.ToString().Equals(DcmConfig.ParsingDirection.Send.ToString()))
                {
                    comboBoxAddress.SelectedIndex = index;
                }
                ++index;
            }


            // 初始化数据格式
            comboBoxMessage.Items.Add(Empty);
            if (vdoc != null && vdoc.MessageTable != null
                && vdoc.MessageTable.Count > 0)
            {
                foreach (var entry in vdoc.MessageTable)
                {
                    comboBoxMessage.Items.Add(entry.Key);
                }
            }
            comboBoxMessage.SelectedIndex = 0;
        }

        public string SubFunctionName
        {
            get
            {
                return textBoxName.Text;
            }
            set
            {
                textBoxName.Text = value;
            }
        }

        public List<byte> PrefixData
        {
            get
            {
                string[] values = textBoxPrefix.Text.Split(" \t".ToCharArray(), 
                    StringSplitOptions.RemoveEmptyEntries);
                List<byte> result = new List<byte>();
                foreach (string value in values)
                {
                    result.Add(Convert.ToByte(value, 16));
                }
                return result;
            }
            set
            {
                textBoxPrefix.Text = Utils.HexArrayToString(value);
            }
        }

        public int DataLen
        {
            get
            {
                return Convert.ToInt32(textBoxDataLen.Text);
            }
            set
            {
                textBoxDataLen.Text = value.ToString();
            }
        }

        public DcmConfig.CanAddressType AddressType
        {
            get
            {
                DcmConfig.CanAddressType addressType;
                if (!Enum.TryParse(comboBoxAddress.Text, out addressType))
                {
                    addressType = DcmConfig.CanAddressType.Physical; //默认物理地址
                }
                return addressType;
            }
            set
            {
                comboBoxAddress.Text = value.ToString();
            }
        }

        public DcmConfig.ParsingDirection ParsingDirection
        {
            get
            {
                DcmConfig.ParsingDirection parsingDirection;
                if (!Enum.TryParse(comboBoxParsingDirection.Text, out parsingDirection))
                {
                    parsingDirection = DcmConfig.ParsingDirection.Send;
                }
                return parsingDirection;
            }

            set
            {
                comboBoxParsingDirection.Text = value.ToString();
            }
        }

        public string Message
        {
            get
            {
                if (Empty.Equals(comboBoxMessage.Text))
                {
                    return null;
                }
                return comboBoxMessage.Text;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    comboBoxMessage.Text = Empty;
                }
                else
                {
                    comboBoxMessage.Text = value;
                    if (string.IsNullOrEmpty(comboBoxMessage.Text))
                    {
                        comboBoxMessage.Text = Empty;
                    }
                }
            }
        }

        private void textBoxName_Validating(object sender, CancelEventArgs e)
        {
            if (textBoxName.Text.Trim().Length == 0)
            {
                MessageBox.Show("子功能名称不为为空", "校验失败", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void textBoxPrefix_Validating(object sender, CancelEventArgs e)
        {
            var prefixStr = textBoxPrefix.Text.Trim();
            Regex regex = new Regex(@"^[0-9a-fA-F]{1,2}(\s[0-9a-fA-F]{1,2})*$");
            if (!regex.IsMatch(prefixStr))
            {
                MessageBox.Show("前缀不是有效的Hex列表", "校验失败", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void textBoxDataLen_Validating(object sender, CancelEventArgs e)
        {
            var dataLenStr = textBoxDataLen.Text.Trim();
            Regex regex = new Regex(@"^\d+$");
            if (!regex.IsMatch(dataLenStr))
            {
                MessageBox.Show("数据长度不是有效的数字", "校验失败", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }
    }
}

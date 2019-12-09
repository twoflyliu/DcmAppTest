using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vdf4Cs
{
    public partial class MessagePage : PageBase
    {
        public MessagePage(VdfBox vdfBox) : base(vdfBox)
        {
            InitializeComponent();
        }

        public int OldByteLen { get; set; }

        protected override void UpdateUI()
        {
            base.UpdateUI();

            if (Node == null)
            {
                OldName = string.Empty;
                textBoxName.Text = string.Empty;
                textBoxDescription.Text = string.Empty;
                textBoxByteLen.Text = "0";
            }
            else
            {
                if (Node.Tag is VdfMessage message)
                {
                    OldName = message.Name;
                    OldByteLen = message.ByteLen;
                    textBoxName.Text = message.Name;
                    textBoxDescription.Text = message.Description;
                    textBoxByteLen.Text = message.ByteLen.ToString();

                    textBoxName.Enabled = (message.Owners.Count == 0);
                }
            }
        }

        private void textBoxName_Validating(object sender, CancelEventArgs e)
        {
            if (TextBoxNameValidating<VdfMessage>(textBoxName, e))
            {
                ((VdfMessage)Node.Tag).Name = textBoxName.Text;
                FireMessageChangedEvent((VdfMessage)Node.Tag);
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (Node != null)
            {
                Node.Text = textBoxName.Text;
            }
        }

        private void textBoxDescription_Validated(object sender, EventArgs e)
        {
            if (Node != null && Node.Tag is VdfMessage message)
            {
                message.Description = textBoxDescription.Text;
                FireMessageChangedEvent((VdfMessage)Node.Tag);
            }
        }

        private void textBoxByteLen_Validating(object sender, CancelEventArgs e)
        {
            if (!UnsignedIntValidate(textBoxByteLen.Text, "字节长度", e)) //校验数据
            {
                textBoxByteLen.Text = OldByteLen.ToString();
                textBoxByteLen.Focus();
            }
            else
            {
                if (Node != null && Node.Tag is VdfMessage message)
                {
                    message.ByteLen = Convert.ToInt32(textBoxByteLen.Text);
                    OldByteLen = message.ByteLen;
                    FireMessageChangedEvent(message);
                }
            }
        }
    }
}

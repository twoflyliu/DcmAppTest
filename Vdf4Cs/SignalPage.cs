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
    public partial class SignalPage : PageBase
    {
        private bool supperssEventResp = false;

        public SignalPage(VdfBox vdfBox) : base(vdfBox)
        {
            InitializeComponent();

            // 初始化字节序
            foreach (string name in Enum.GetNames(typeof(VdfByteOrder)))
            {
                comboBoxByteOrder.Items.Add(name);
            }
            comboBoxByteOrder.Text = VdfByteOrder.Intel.ToString();
        }

        public int OldStartBit { get; set; }
        public int OldBitLen { get; set; }

        protected override void UpdateUI()
        {
            base.UpdateUI();

            supperssEventResp = true;
            RefreshComboBoxValueDescription();
            if (Node != null && Node.Tag is VdfSignal signal)
            {
                textBoxName.Text = signal.Name;
                OldName = textBoxName.Text;

                textBoxStartBit.Text = signal.StartBit.ToString();
                OldStartBit = signal.StartBit;

                textBoxBitLen.Text = signal.BitLen.ToString();
                OldBitLen = signal.BitLen;

                comboBoxByteOrder.Text = signal.ByteOrder.ToString();
                comboBoxValDesc.Text = signal.ValueDesc;
            }
            supperssEventResp = false;
        }

        private void RefreshComboBoxValueDescription()
        {
            if (VdfDocument == null)
            {
                return;
            }

            comboBoxValDesc.Items.Clear();
            foreach (var key in VdfDocument.ValueDescTable.Keys)
            {
                comboBoxValDesc.Items.Add(key);
            }
        }

        protected override void OnVdfDocumentChanged(VdfDocument doc)
        {
            base.OnVdfDocumentChanged(doc);
        }

        private void textBoxName_Validating(object sender, CancelEventArgs e)
        {
            TextBoxNameValidating<VdfSignal>(textBoxName, e, (signal) =>
            {
                signal.Name = textBoxName.Text;
                FireSignalChangedEvent(signal);
            });
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (Node != null)
            {
                Node.Text = textBoxName.Text;
            }
        }

        private void textBoxStartBit_Validating(object sender, CancelEventArgs e)
        {
            int oldStartBit = OldStartBit;
            TextBoxNameValidating<VdfSignal>(textBoxStartBit, "起始位",e, ref oldStartBit, 
                (signal) =>
                {
                    signal.StartBit = Convert.ToInt32(textBoxStartBit.Text);
                    OldStartBit = signal.StartBit;
                    FireSignalChangedEvent(signal);
                });
        }

        private void textBoxBitLen_Validating(object sender, CancelEventArgs e)
        {
            int oldBitLen = OldBitLen;
            TextBoxNameValidating<VdfSignal>(textBoxBitLen, "位长度", e, ref oldBitLen,
                (signal) =>
                {
                    signal.BitLen = Convert.ToInt32(textBoxBitLen.Text);
                    OldBitLen = signal.BitLen;
                    FireSignalChangedEvent(signal);
                });
        }

        private void comboBoxByteOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Enum.TryParse<VdfByteOrder>(comboBoxByteOrder.Text, out VdfByteOrder byteOrder))
            {
                VdfSignal signal = Entity<VdfSignal>();
                if (signal != null)
                {
                    signal.ByteOrder = byteOrder;
                    FireSignalChangedEvent(signal);
                }
            }
        }

        private void comboBoxValDesc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (supperssEventResp)
            {
                return;
            }

            if (!string.IsNullOrEmpty(comboBoxValDesc.Text))
            {
                VdfSignal signal = Entity<VdfSignal>();
                if (signal != null)
                {
                    signal.ValueDesc = comboBoxValDesc.Text;
                    signal.VdfValueDesc = VdfDocument.ValueDescTable[signal.ValueDesc];
                    FireSignalChangedEvent(signal);
                }
            }
        }
    }
}

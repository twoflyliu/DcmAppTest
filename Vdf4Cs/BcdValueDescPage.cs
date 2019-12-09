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
    public partial class BcdValueDescPage : PageBase
    {
        private VdfBcdValueDesc valDesc;

        private int oldFactor;
        private int oldOffset;
        private int oldMaxValue;
        private int oldMinValue;

        private string oldSeperator;

        public BcdValueDescPage(VdfBox vdfBox) : base(vdfBox)
        {
            InitializeComponent();
        }

        protected override void UpdateUI()
        {
            base.UpdateUI();

            valDesc = Entity<VdfBcdValueDesc>();
            if (valDesc != null)
            {
                textBoxName.Text = valDesc.Name;
                OldName = textBoxName.Text;

                checkBoxFill.Checked = valDesc.Fill;
                checkBoxCanFillAlpha.Checked = valDesc.CanFillAlpha;

                textBoxFactor.Text = valDesc.Factor.ToString();
                oldFactor = valDesc.Factor;

                textBoxOffset.Text = valDesc.Offset.ToString();
                oldOffset = valDesc.Offset;

                textBoxMinValue.Text = valDesc.Minimum.ToString();
                oldMinValue = valDesc.Minimum;

                textBoxMaxValue.Text = valDesc.Maximum.ToString();
                oldMaxValue = valDesc.Maximum;

                if (valDesc.Separator != null)
                {
                    textBoxSeperator.Text = valDesc.Separator.ToString();
                    oldSeperator = valDesc.Separator;
                }
                else
                {
                    textBoxSeperator.Text = string.Empty;
                    oldSeperator = string.Empty;
                }

                EnableAllUIByCanFillAlpha(checkBoxCanFillAlpha.Checked);

                textBoxName.Enabled = (valDesc.Owners.Count == 0);
            }
        }

        private void EnableAllUIByCanFillAlpha(bool canFillAlpha)
        {
            checkBoxFill.Enabled = !canFillAlpha;
            textBoxFactor.Enabled = !canFillAlpha;
            textBoxOffset.Enabled = !canFillAlpha;
            textBoxMinValue.Enabled = !canFillAlpha;
            textBoxMaxValue.Enabled = !canFillAlpha;
            textBoxSeperator.Enabled = canFillAlpha;
        }

        private void BcdValueDescPage_Load(object sender, EventArgs e)
        {

        }

        private void checkBoxCanFillAlpha_Click(object sender, EventArgs e)
        {
            EnableAllUIByCanFillAlpha(checkBoxCanFillAlpha.Checked);
            if (valDesc != null)
            {
                valDesc.CanFillAlpha = checkBoxCanFillAlpha.Checked;
                FireValueDescChangedEvent(valDesc);
            }
        }

        private void textBoxName_Validating(object sender, CancelEventArgs e)
        {
            TextBoxNameValidating<VdfValueDesc>(textBoxName, e, (ent) =>
            {
                ent.Name = textBoxName.Text;
                FireValueDescChangedEvent(valDesc);
            });
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (Node != null)
            {
                Node.Text = textBoxName.Text;
            }
        }

        private void textBoxFactor_Validating(object sender, CancelEventArgs e)
        {
            TextBoxUnsignedIntValidating<VdfBcdValueDesc>(textBoxFactor, "精度", e,
                ref oldFactor, (ent) =>
            {
                oldFactor = Convert.ToInt32(textBoxFactor.Text);
                ent.Factor = oldFactor;
                FireValueDescChangedEvent(valDesc);
            });
        }

        private void textBoxOffset_Validating(object sender, CancelEventArgs e)
        {
            TextBoxUnsignedIntValidating<VdfBcdValueDesc>(textBoxOffset, "偏移值", e,
                ref oldOffset, (ent) =>
                {
                    oldOffset = Convert.ToInt32(textBoxOffset.Text);
                    ent.Offset = oldOffset;
                    FireValueDescChangedEvent(valDesc);
                });
        }

        private void textBoxMinValue_Validating(object sender, CancelEventArgs e)
        {
            TextBoxUnsignedIntValidating<VdfBcdValueDesc>(textBoxMinValue, "最小值", e,
                ref oldMinValue, (ent) =>
                {
                    oldMinValue = Convert.ToInt32(textBoxMinValue.Text);
                    ent.Minimum = oldMinValue;
                    FireValueDescChangedEvent(valDesc);
                });
        }

        private void textBoxMaxValue_Validating(object sender, CancelEventArgs e)
        {
            TextBoxUnsignedIntValidating<VdfBcdValueDesc>(textBoxMaxValue, "最大值", e,
                ref oldMaxValue, (ent) =>
                {
                    oldMaxValue = Convert.ToInt32(textBoxMaxValue.Text);
                    ent.Maximum = oldMaxValue;
                    FireValueDescChangedEvent(valDesc);
                });
        }

        private void textBoxSeperator_Validating(object sender, CancelEventArgs e)
        {
            TextBoxStringNotEmptyValidating<VdfBcdValueDesc>(textBoxSeperator, "分隔符", e,
                ref oldSeperator, (ent) =>
                {
                    oldSeperator = textBoxSeperator.Text.ToString();
                    ent.Separator = oldSeperator;
                    FireValueDescChangedEvent(valDesc);
                });
        }
    }
}

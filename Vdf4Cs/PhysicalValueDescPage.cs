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
    public partial class PhysicalValueDescPage : PageBase
    {
        private double oldFactor;
        private double oldOffset;
        private string oldUnit;
        private double oldMaxValue;
        private double oldMinValue;

        public PhysicalValueDescPage(VdfBox vdfBox) : base(vdfBox)
        {
            InitializeComponent();
        }

        protected override void UpdateUI()
        {
            base.UpdateUI();

            VdfPhyValueDesc valDesc = Entity<VdfPhyValueDesc>();
            if (valDesc != null)
            {
                textBoxName.Text = valDesc.Name;
                OldName = valDesc.Name;

                textBoxFactor.Text = valDesc.Factor.ToString();
                oldFactor = valDesc.Factor;

                textBoxOffset.Text = valDesc.Offset.ToString();
                oldOffset = valDesc.Offset;

                textBoxUnit.Text = valDesc.Unit;
                oldUnit = valDesc.Unit;

                textBoxMaxValue.Text = valDesc.Maximum.ToString();
                oldMaxValue = valDesc.Maximum;

                textBoxMinValue.Text = valDesc.Minimum.ToString();
                oldMinValue = valDesc.Minimum;

                textBoxName.Enabled = (valDesc.Owners.Count == 0);
            }
        }

        private void textBoxName_Validating(object sender, CancelEventArgs e)
        {
            TextBoxNameValidating<VdfValueDesc>(textBoxName, e, (valDesc) =>
            {
                if (valDesc is VdfPhyValueDesc phyValDesc)
                {
                    phyValDesc.Name = textBoxName.Text;
                    FireValueDescChangedEvent(valDesc);
                }
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
            TextBoxDoubleValidating<VdfPhyValueDesc>(textBoxFactor, "精度", e, 
                ref oldFactor,  (phyValDesc) =>
            {
                oldFactor = Convert.ToDouble(textBoxFactor.Text);
                phyValDesc.Factor = oldFactor;
                FireValueDescChangedEvent(phyValDesc);
            });
        }

        private void textBoxOffset_Validating(object sender, CancelEventArgs e)
        {
            TextBoxDoubleValidating<VdfPhyValueDesc>(textBoxOffset, "偏移值", e,
            ref oldOffset, (phyValDesc) =>
            {
                oldOffset = Convert.ToDouble(textBoxOffset.Text);
                phyValDesc.Offset = oldOffset;
                FireValueDescChangedEvent(phyValDesc);
            });
        }

        private void textBoxUnit_Validating(object sender, CancelEventArgs e)
        {
            oldUnit = textBoxUnit.Text;
            var ent = Entity<VdfPhyValueDesc>();
            if (ent != null)
            {
                ent.Unit = oldUnit;
                FireValueDescChangedEvent(ent);
            }
        }

        private void textBoxMaxValue_Validating(object sender, CancelEventArgs e)
        {
            TextBoxDoubleValidating<VdfPhyValueDesc>(textBoxMaxValue, "最大值", e,
            ref oldMaxValue, (phyValDesc) =>
            {
                oldMaxValue = Convert.ToDouble(textBoxMaxValue.Text);
                phyValDesc.Maximum = oldMaxValue;
                FireValueDescChangedEvent(phyValDesc);
            });
        }

        private void textBoxMinValue_Validating(object sender, CancelEventArgs e)
        {
            TextBoxDoubleValidating<VdfPhyValueDesc>(textBoxMinValue, "最小值", e,
            ref oldMinValue, (phyValDesc) =>
            {
                oldMinValue = Convert.ToDouble(textBoxMinValue.Text);
                phyValDesc.Minimum = oldMinValue;
                FireValueDescChangedEvent(phyValDesc);
            });
        }
    }
}

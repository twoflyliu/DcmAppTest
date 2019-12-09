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
    public partial class AsciiValueDescPage : PageBase
    {
        private VdfAsciiValueDesc valDesc;

        public AsciiValueDescPage(VdfBox vdfBox) : base(vdfBox)
        {
            InitializeComponent();
        }

        protected override void UpdateUI()
        {
            base.UpdateUI();

            valDesc = Entity<VdfAsciiValueDesc>();
            if (valDesc != null)
            {
                textBoxName.Text = valDesc.Name;
                OldName = textBoxName.Text;

                textBoxName.Enabled = (valDesc.Owners.Count == 0);
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
    }
}

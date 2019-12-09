using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSDcmTest
{
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AboutBox_Load(object sender, EventArgs e)
        {
            Version v = new Version(Application.ProductVersion);
            labelVersion.Text = string.Format("版本：V{0}.{1}", v.Major, v.Minor);
        }
    }
}

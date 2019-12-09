using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSDcmTest.Dialog
{
    public partial class ServiceDialog : Form
    {
        public enum Operation
        {
            New = 0,
            Update
        }


        public ServiceDialog(Operation operation)
        {
            InitializeComponent();
            Text = ((operation == Operation.New) ? "新建服务" : "编辑服务");
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        public string ServiceName
        {
            get
            {
                return textBoxServiceName.Text;
            }
            set
            {
                textBoxServiceName.Text = value;
            }
        }
    }
}

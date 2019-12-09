using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DcmService;

namespace CSDcmTest
{
    public partial class DcmRawWindow : ToolWindow
    {
        public DcmRawWindow()
        {
            InitializeComponent();
        }

        private delegate void OnRawDataIncommingCallback(RawDataIncommingEventArgs e);
        internal void OnRawDataIncomming(RawDataIncommingEventArgs e)
        {
            if (listBox.InvokeRequired)
            {
                OnRawDataIncommingCallback callback = new OnRawDataIncommingCallback(
                    OnRawDataIncomming);
                listBox.Invoke(callback, e);
            }
            else
            {
                string item = string.Format("{0}[0x{1:X3}]: {2} ({3})", 
                    (e.Operation == RawDataIncommingEventArgs.OperationEum.Send) ? "Tx" : "Rx",
                    e.CanId, Utils.HexArrayToString(e.Data), 
                    e.Ok ? "成功" : "失败");
                listBox.Items.Add(string.Format("{0:d2}.{1:d3}->{2}", e.Timestamp.Second,
                    e.Timestamp.Millisecond, item));
                listBox.SelectedIndex = listBox.Items.Count - 1;
            }
        }

        private void toolButtonClear_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
        }

        private void toolButtonSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save Raw Can Data";
                dlg.DefaultExt = "rawlog";
                dlg.Filter = "Raw Can Data(*.rawlog)|*.rawlog|" +
                    "All files(*.*)|*.*";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Save(dlg.FileName);
                }
            }
        }

        private void Save(string fileName)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);

                foreach(string item in listBox.Items)
                {
                    sw.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Save raw can data to file {0} failed: {1}", fileName, ex.Message),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        internal void ClearContent()
        {
            listBox.Items.Clear();
        }
    }
}

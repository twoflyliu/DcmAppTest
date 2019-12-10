using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DcmConfig;
using Vdf4Cs;

namespace CSDcmTest
{
    public partial class DcmContentWindow : ToolWindow
    {
        private DcmConfig.SubFunction subFunction = null;
        private MainForm mainForm = null;

        public DcmConfig.DcmDocument DcmDocument { get; set; }
        public const int ColumnIndexUnit = 2;

        public DcmContentWindow(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            DcmDocument = null;
        }

        public void Update(DcmConfig.SubFunction subFunction)
        {
            dataGridView.SuspendLayout();
            dataGridView.Rows.Clear();
            InitSubFunctionData(subFunction);
            AddFirstRow(subFunction);

            if (subFunction.Message == null || DcmDocument == null ||
                !needSendParsing(subFunction) || subFunction.DataLen == 0)
            {
                AddDataRows(subFunction);
            }
            else //添加带解码编辑功能
            {
                var msg = DcmDocument.VdfDocument.Message(subFunction.Message);
                if (msg == null)
                {
                    AddDataRows(subFunction);
                }
                else
                {
                    foreach (var entry in msg.SignalTable)
                    {
                        var signal = entry.Value;
                        dataGridView.Rows.Add(new string[] { signal.Name, "" });

                        try
                        {
                            dataGridView.Rows[dataGridView.Rows.Count - 1]
                            .Cells["ColumnData"] = NewDataCell(signal, subFunction,
                                dataGridView.Rows.Count - 1);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "更新单元格失败",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            UpdateReadOnly(subFunction);
            dataGridView.ResumeLayout();

            this.subFunction = subFunction;
        }

        private DataGridViewCell NewDataCell(VdfSignal signal, SubFunction subFunction, int row)
        {
            var desc = signal.VdfValueDesc;

            if (desc is VdfBcdValueDesc || desc is VdfAsciiValueDesc)
            {
                DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                cell.Value = VdfEncoder.Encode(subFunction.Data, signal);
                cell.Tag = signal;
                return cell;
            }
            else if (desc is VdfPhyValueDesc)
            {
                DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                cell.Value = VdfEncoder.Encode(subFunction.Data, signal);

                //设置单位
                dataGridView.Rows[row].Cells["ColumnUnit"].Value
                    = (desc as VdfPhyValueDesc).Unit;
                cell.Tag = signal;
                return cell;
            }
            else if (desc is VdfXncodeValueDesc)
            {
                DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
                VdfXncodeValueDesc xncodeDesc = (VdfXncodeValueDesc)desc;

                foreach (var entry in xncodeDesc.EntryTable)
                {
                    cell.Items.Add(entry.Value);
                }
                cell.Value = VdfEncoder.Encode(subFunction.Data, signal); //这儿需要进行解码
                cell.Tag = signal;
                return cell;
            }
            else
            {
                throw new ArgumentException("Unsupported VdfValueDesc Type: " + signal.GetType().Name);
            }
        }

        private void UpdateReadOnly(SubFunction subFunction)
        {
            //所有第一列都是只读的
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                dataGridView[0, i].ReadOnly = true;
            }

            //第二列的首行是只读的
            dataGridView[1, 0].ReadOnly = true;
        }

        private void InitSubFunctionData(DcmConfig.SubFunction subFunction)
        {
            // 初始化subFunction.Data
            if (subFunction.Data == null)
            {
                subFunction.Data = new List<byte>();
                for (int i = 0; i < subFunction.DataLen; i++)
                {
                    subFunction.Data.Add(0);
                }
            }
        }

        private void AddDataRows(SubFunction subFunction)
        {
            string name, value;

            for (int i = 0; i < subFunction.DataLen; i++)
            {
                name = string.Format("Data{0}(Hex)", i);
                value = string.Format("{0:X2}", subFunction.Data[i]);
                dataGridView.Rows.Add(new string[] { name, value });
            }
        }

        private void AddFirstRow(SubFunction subFunction)
        {
            string name = "PDU";
            String value = string.Format("{0} {1}",
                Utils.HexArrayToString(subFunction.Prefix),
                Utils.HexArrayToString(subFunction.Data));
            dataGridView.Rows.Add(new string[] { name, value });

            dataGridView.Rows[0].Frozen = true;
        }

        private void UpdateFirstRow()
        {
            String value = string.Format("{0} {1}",
                Utils.HexArrayToString(subFunction.Prefix),
                Utils.HexArrayToString(subFunction.Data));
            dataGridView[1, 0].Value = value;
        }

        public DcmConfig.SubFunction SubFunction
        {
            get
            {
                return subFunction;
            }

            set
            {
                if (subFunction != value)
                {
                    subFunction = value;
                    Update(subFunction);
                }
            }
        }

        public List<byte> GetPackageData()
        {
            List<byte> package = new List<byte>();

            if (subFunction != null)
            {
                package.AddRange(SubFunction.Prefix);
                package.AddRange(SubFunction.Data);
            }

            return package;
        }

        private bool needSendParsing(SubFunction subFunction)
        {
            if (subFunction.Message == null 
                || DcmDocument.VdfDocument.Message(subFunction.Message) == null)
            {
                return false;
            }
            return subFunction.ParsingDirection == ParsingDirection.Send ||
                subFunction.ParsingDirection == ParsingDirection.Bidirection;
        }

        // 更新数据
        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (SubFunction == null)
            {
                return;
            }

            int row = e.RowIndex;
            int column = e.ColumnIndex;
            var cell = dataGridView[column, row];

            if (cell.ReadOnly)
            {
                return;
            }

            // 不需要解码
            if (!needSendParsing(SubFunction))
            {
                // 只有用户编辑的单元格才会同步到子功能
                var cellValue = cell.Value;
                string value = "00";
                if (cellValue != null)
                {
                    value = cellValue.ToString();
                }
                else
                {
                    dataGridView[column, row].Value = value;
                }
                SubFunction.Data[row - 1] = Utils.HexToByte(value);

                //更新第一行
                UpdateFirstRow();
            }
            else //需要进行解码
            {
                // 获取解码值
                VdfSignal signal = cell.Tag as VdfSignal;
                //Debug.Assert(signal != null);

                // 更新值
                string cellValue = string.Empty;
                if (cell.Value != null)
                {
                    cellValue = cell.Value.ToString();
                }
                VdfDecoder.Decode(SubFunction.Data, signal, cellValue);
                UpdateFirstRow();
            }
        }

        internal void Clear()
        {
            dataGridView.Rows.Clear();
        }

        // 进行校验
        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var cell = dataGridView[e.ColumnIndex, e.RowIndex];
            if (cell.ReadOnly)
            {
                return;
            }

            if (!needSendParsing(SubFunction))
            {
                //保证输入的数值，是16进制值
                try
                {
                    Convert.ToInt32(e.FormattedValue.ToString(), 16);
                }
                catch (Exception)
                {
                    MessageBox.Show("输入的字符必须是Hex字符", "校验出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
            else
            {
                VdfSignal signal = cell.Tag as VdfSignal;
                //signal居然可能为空

                // 这儿只是进行验证，不更新数据
                try
                {
                    var copy = new List<byte>();
                    copy.AddRange(SubFunction.Data);
                    VdfDecoder.Decode(copy, signal, e.FormattedValue.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "校验出错", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
                
            }
        }

        internal void ClearContent()
        {
            dataGridView.Rows.Clear();
        }
    }
}

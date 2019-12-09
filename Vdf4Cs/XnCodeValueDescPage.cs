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
    public partial class XnCodeValueDescPage : PageBase
    {
        public const int ColumnIndexValue = 0;
        public const int ColumnIndexDescription = 1;

        private VdfXncodeValueDesc valDesc;
        private bool SuppressDeleteEvent = false;

        public XnCodeValueDescPage(VdfBox vdfBox) : base(vdfBox)
        {
            InitializeComponent();
        }

        protected override void UpdateUI()
        {
            base.UpdateUI();

            valDesc = Entity<VdfXncodeValueDesc>();

            SuppressDeleteEvent = true;
            dataGridView.Rows.Clear();
            SuppressDeleteEvent = false;

            if (valDesc != null)
            {
                textBoxName.Text = valDesc.Name;
                OldName = valDesc.Name;

                foreach (var ent in valDesc.EntryTable)
                {
                    dataGridView.Rows.Add(new string[] { ent.Key.ToString(),
                     ent.Value.ToString()});
                }

                textBoxName.Enabled = (valDesc.Owners.Count == 0);
            }
        }

        private void textBoxName_Validating(object sender, CancelEventArgs e)
        {
            TextBoxNameValidating<VdfValueDesc>(textBoxName, e, (valDesc) =>
            {
                valDesc.Name = textBoxName.Text.ToString();
                FireValueDescChangedEvent(valDesc);
            });
        }

        private string CellValue(int columnIndex, int rowIndex)
        {
            object o = dataGridView[columnIndex, rowIndex].Value;
            return (o == null) ? string.Empty : o.ToString();
        }

        private string Value(int rowIndex)
        {
            return CellValue(ColumnIndexValue, rowIndex);
        }

        private string Desc(int rowIndex)
        {
            return CellValue(ColumnIndexDescription, rowIndex);
        }

        public void UpdateOrAddEntry(int value, string desc)
        {
            if (valDesc.EntryTable.ContainsKey(value))
            {
                valDesc.EntryTable[value] = desc;
            }
            else
            {
                valDesc.EntryTable.Add(value, desc);
            }
        }

        private void dataGridView_CellValidating(object sender, 
            DataGridViewCellValidatingEventArgs e)
        {
            if (SuppressDeleteEvent)
            {
                return;
            }

            // 执行校验
            if (e.RowIndex < dataGridView.RowCount - 1)
            {
                if (!ValidateUpdate(e))
                {
                    return;
                }
            }
            else
            {
                if (!ValidateAdd(e))
                {
                    return;
                }
            }

            // 更新值
            if (valDesc == null)
            {
                return;
            }

            string oldValue = Value(e.RowIndex);
            string oldDesc = Desc(e.RowIndex);

            if (ColumnIndexValue == e.ColumnIndex) //添加或者更新值
            {
                int value = Convert.ToInt32(e.FormattedValue);
                UpdateOrAddEntry(value, oldDesc);
                FireValueDescChangedEvent(valDesc);
            }
            else if (ColumnIndexDescription == e.ColumnIndex)
            {
                try
                {
                    int value = Convert.ToInt32(oldValue);
                    UpdateOrAddEntry(value, e.FormattedValue.ToString());
                    FireValueDescChangedEvent(valDesc);
                }
                catch (Exception)
                {
                }
            }
        }

        private bool ValidateAdd(DataGridViewCellValidatingEventArgs e)
        {
            string lastValue = null;
            string lastDesc = null;
            var lastRow = dataGridView.Rows[dataGridView.RowCount - 1];

            if (lastRow.Cells[ColumnIndexValue].Value != null)
            {
                lastValue = lastRow.Cells[ColumnIndexValue].Value.ToString();
            }
            if (lastRow.Cells[ColumnIndexDescription].Value != null)
            {
                lastDesc = lastRow.Cells[ColumnIndexDescription].Value.ToString();
            }

            if (string.IsNullOrEmpty(lastValue) && string.IsNullOrEmpty(lastDesc) 
                && string.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                return false;
            }

            return ValidateUpdate(e);
        }

        private bool ValidateUpdate(DataGridViewCellValidatingEventArgs e)
        {
            string errorMsg = string.Empty;
            string newValue = e.FormattedValue.ToString();

            if (e.ColumnIndex == ColumnIndexValue)
            {
                if (!UnsignedIntValidate(newValue))
                {
                    errorMsg = string.Format("[{0}] 不是一个有效的无符号整数", newValue);
                }
                else if (valDesc != null)
                {
                    if (!newValue.Equals(dataGridView[e.ColumnIndex,e.RowIndex].Value))
                    {
                        if (valDesc.EntryTable.TryGetValue(Convert.ToInt32(newValue), out string temp))
                        {
                            errorMsg = string.Format("[{0}] 已经存在，请选择其他的值", newValue);
                        }
                    }
                }
            }
            else if (e.ColumnIndex == ColumnIndexDescription)
            {
                // 强制描述信息不能为空
                if (string.IsNullOrEmpty(newValue.Trim()))
                {
                    errorMsg = "描述信息不能为空";
                }
            }

            if (!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show(errorMsg, "校验失败", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                e.Cancel = true;
                return false;
            }
            return true;
        }

        private void dataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (SuppressDeleteEvent)
            {
                return;
            }

            var cell = e.Row.Cells[ColumnIndexValue];
            if (cell != null)
            {
                try
                {
                    int value = Convert.ToInt32(cell.Value.ToString());
                    Console.WriteLine("Remove Value = " + value);
                    valDesc.EntryTable.Remove(value);
                    FireValueDescChangedEvent(valDesc);
                }
                catch (Exception)
                {
                }
            }
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

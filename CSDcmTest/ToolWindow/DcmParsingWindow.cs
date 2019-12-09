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
using Vdf4Cs;

namespace CSDcmTest
{
    public partial class DcmParsingWindow : ToolWindow
    {
        private const int KeyColumnWidth = 5;
        private const string KeyValueSeperator = ": ";
        private const string CsvSeperator = ",";

        private const string DcmLogHeader = "---DcmAppLog---";
        private const string EmptyCsvContent = " ";

        public Color PostiveResponseForeColor { get; set; }
        public Color NegativeResponseForeColor { get; set; }
        public Color PostiveResponseBackColor { get; set; }
        public Color NegativeResponseBackColor { get; set; }

        public const int ColumnCanIdIndex = 0;
        public const int ColumnDataIndex = 1;
        public const int ColumnCount = 2;

        private DcmConfig.DcmDocument dcmDocument = null;

        public DcmParsingWindow()
        {
            InitializeComponent();

            PostiveResponseForeColor = Color.Green;
            NegativeResponseForeColor = Color.Red;

            PostiveResponseBackColor = SystemColors.Window;
            NegativeResponseBackColor = Color.Yellow;

            dcmDataDGV.CellFormatting += dcmDataDGV_CellFormatting;

            dcmDataDGV.Rows.Clear(); //清空所有应用数据
        }

        private class ResponseDataTag
        {
            public List<KeyValuePair<string,string>> ParsedList { get; set; }
            public bool PostiveResponse { get; set; }
        }

        #region Set response data cell format
        private void dcmDataDGV_CellFormatting(object sender, 
            DataGridViewCellFormattingEventArgs e)
        {
            var cell = dcmDataDGV[e.ColumnIndex, e.RowIndex];
            var tag = cell.Tag as ResponseDataTag;
            if (tag == null)
            {
                return;
            }

            if (!tag.PostiveResponse)
            {
                e.CellStyle.ForeColor = NegativeResponseForeColor;
                e.CellStyle.BackColor = NegativeResponseBackColor;
            }
            else
            {
                e.CellStyle.ForeColor = PostiveResponseForeColor;
                e.CellStyle.BackColor = PostiveResponseBackColor;
            }
            e.FormattingApplied = true;
        }
        #endregion

        public void UpdateDcmDocument(DcmConfig.DcmDocument dcmDocument)
        {
            this.dcmDocument = dcmDocument;
        }

        #region Receive Dcm Service Layer's receiving data and parse to setup ResponseDataTag
        private delegate void AddItemsCallback(ParsingDataIncommingEventArgs e);
        internal void OnParsingDataIncomming(ParsingDataIncommingEventArgs e)
        {
            if (dcmDataDGV.InvokeRequired)
            {
                AddItemsCallback callback = new AddItemsCallback(OnParsingDataIncomming);
                dcmDataDGV.Invoke(callback, e);
            }
            else
            {
                dcmDataDGV.SuspendLayout();

                // 添加请求ID
                if (e.RequestData != null)
                {
                    dcmDataDGV.Rows.Add(new string[] { string.Format("Tx{0:X3}", e.RequestCanId),
                        Utils.HexArrayToString(e.RequestData)});
                }

                // 添加响应ID
                if (e.ResponseData != null)
                {
                    dcmDataDGV.Rows.Add(new string[] { string.Format("Rx{0:X3}", e.ResponseCanId),
                        Utils.HexArrayToString(e.ResponseData)});
                }
                else
                {
                    dcmDataDGV.Rows.Add(new string[] { "<Empty>", "<Empty>" });
                }

                // 设置响应数据前景色
                var cell = dcmDataDGV[ColumnDataIndex, dcmDataDGV.Rows.Count - 1];
                cell.Selected = true;

                // 添加、解析数据
                List<KeyValuePair<string, string>> itemsToAdd 
                    = new List<KeyValuePair<string, string>>();
                itemsToAdd.AddRange(e.EntryList);
                ParseReceiveData(e, itemsToAdd);

                ResponseDataTag tag = new ResponseDataTag
                {
                    ParsedList = itemsToAdd,
                    PostiveResponse = e.PostiveResponse
                };
                cell.Tag = tag;

                dcmDataDGV.ResumeLayout();

                // 手动通知选择变化
                dcmDataDGV_SelectionChanged(dcmDataDGV, EventArgs.Empty);
            }
        }

        private DcmConfig.SubFunction FindFunction(List<byte> reqData)
        {
            DcmConfig.SubFunction subFunction = null;
            for (int len = reqData.Count; len > 0; len--)
            {
                var key = DcmConfig.DcmDocument.GenParsingReceiveKey(reqData, len);
                if (dcmDocument.ParsingReceiveSubFunctionTable
                    .TryGetValue(key, out subFunction))
                {
                    if (subFunction.Prefix.Count == len)
                    {
                        if (subFunction.Message == null)
                        {
                            subFunction = null;
                        }
                        break;
                    }
                    else
                    {
                        subFunction = null;
                    }
                }
            }
            return subFunction;
        }

        private void ParseReceiveData(ParsingDataIncommingEventArgs e,
            List<KeyValuePair<string, string>> itemsToAdd)
        {
            if (!e.PostiveResponse || dcmDocument == null || dcmDocument.VdfDocument == null)
            {
                return;
            }

            DcmConfig.SubFunction subFunction = FindFunction(e.RequestData);
            if (subFunction == null)
            {
                return;
            }

            // 执行解析数据
            List<byte> data = new List<byte>();
            for (int i = subFunction.Prefix.Count; i < e.ResponseData.Count; i++)
            {
                data.Add(e.ResponseData[i]);
            }

            var message = dcmDocument.VdfDocument.Message(subFunction.Message);
            foreach (var entry in message.SignalTable)
            {
                string value = VdfEncoder.Encode(data, entry.Value, true);
                itemsToAdd.Add(new KeyValuePair<string,string>(entry.Value.Name, value));
            }
        }
        #endregion

        private void clearToolButton_Click(object sender, EventArgs e)
        {
            dcmDataDGV.Rows.Clear();
        }

        internal void UpdateSplitterColor(Color color)
        {
            splitContainer1.BackColor = color;
        }

        #region Sync right parsing content data grid view

        private int prevReqRespIndex = -1;
        private void dcmDataDGV_SelectionChanged(object sender, EventArgs e)
        {
            if (dcmDataDGV.SelectedRows.Count == 0)
            {
                prevReqRespIndex = -1;
                parsingDataDGV.Rows.Clear();
                return;
            }

            var row = dcmDataDGV.SelectedRows[0];
            var cell = row.Cells[ColumnDataIndex];
            int rowIndex = row.Index;

            ResponseDataTag tag = cell.Tag as ResponseDataTag;
            if (tag == null)
            {
                if (row.Index + 1 < dcmDataDGV.Rows.Count)
                {
                    tag = dcmDataDGV[ColumnDataIndex, row.Index + 1].Tag as ResponseDataTag;
                    rowIndex = row.Index + 1;
                }
            }

            if (tag != null)
            {
                if (prevReqRespIndex != rowIndex)
                {
                    parsingDataDGV.Rows.Clear();
                    Utils.DataGridViewAddRows(parsingDataDGV, tag.ParsedList);
                    prevReqRespIndex = rowIndex;
                }
            }
        }
        #endregion

        #region IO operator
        private void saveToolButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save Dcm Data";
                dlg.DefaultExt = "csv";
                dlg.Filter = "Raw Can Data(*.csv)|*.csv|" +
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
                sw = new StreamWriter(fs, new UTF8Encoding(true)); //这儿加上BOM头目的是，excel只能识别带有bom头的UTF8内容，不该BOM头的识别不了

                int i = 0;

                //保存头
                sw.WriteLine(DcmLogHeader);

                foreach (DataGridViewRow row in dcmDataDGV.Rows)
                {
                    // 保存报文
                    Utils.DataGridViewSaveRow(sw, dcmDataDGV, row, ",", true);

                    if (++i == 2)
                    {
                        i = 0;

                        // 保存解析结果
                        ResponseDataTag tag = row.Cells[ColumnDataIndex].Tag as ResponseDataTag;
                        SaveResponseDataTag(sw, tag);
                    }
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

        private void SaveResponseDataTag(StreamWriter sw, ResponseDataTag tag)
        {
            foreach (var entry in tag.ParsedList)
            {
                sw.Write(EmptyCsvContent);
                sw.Write(CsvSeperator);
                sw.Write(entry.Key);
                sw.Write(CsvSeperator);
                sw.Write(entry.Value);
                sw.WriteLine();
            }

            sw.Write(EmptyCsvContent);
            sw.Write(CsvSeperator);
            sw.Write(EmptyCsvContent);
            sw.Write(CsvSeperator);
            sw.Write(tag.PostiveResponse);
            sw.WriteLine();
        }

        internal void ClearContent()
        {
            dcmDataDGV.Rows.Clear();
        }

        private void loadToolButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Load Dcm Data";
                dlg.DefaultExt = "csv";
                dlg.Filter = "Raw Can Data(*.csv)|*.csv|" +
                    "All files(*.*)|*.*";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Open(dlg.FileName);
                    dcmDataDGV_SelectionChanged(dcmDataDGV, EventArgs.Empty); //更新右侧的解析窗口
                }
            }
        }

        private void Open(string fileName)
        {
            FileStream fs = null;
            StreamReader sw = null;

            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                sw = new StreamReader(fs);

                string line = sw.ReadLine();
                if (!line.Equals(DcmLogHeader))
                {
                    throw new Exception("File content is valid dcm log");
                }

                dcmDataDGV.Rows.Clear();

                ResponseDataTag tag = null;
                DataGridViewCell cell = null;

                while ((line = sw.ReadLine()) != null)
                {
                    var fields = line.Split(CsvSeperator.ToCharArray());
                    if (fields.Length == 2) //报文数据
                    {
                        if (cell != null)
                        {
                            cell.Tag = tag;
                            tag = null;
                        }

                        dcmDataDGV.Rows.Add(new string[] { fields[0], fields[1]});
                        cell = dcmDataDGV[dcmDataDGV.ColumnCount-1, dcmDataDGV.Rows.Count-1];
                    }
                    else if (fields.Length == 3)
                    {
                        if (tag == null)
                        {
                            tag = new ResponseDataTag();
                            tag.ParsedList = new List<KeyValuePair<string, string>>();
                        }

                        if (fields[0].Equals(EmptyCsvContent) 
                            && fields[1].Equals(EmptyCsvContent))
                        {
                            bool result;
                            if (bool.TryParse(fields[2], out result))
                            {
                                tag.PostiveResponse = result;
                            }
                        }
                        else
                        {
                            tag.ParsedList.Add(
                                new KeyValuePair<string, string>(fields[1], fields[2]));
                        }
                    }

                    if (tag != null)
                    {
                        cell.Tag = tag;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Load dcm log file {0} failed: {1}", fileName, ex.Message),
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
        #endregion
    }
}

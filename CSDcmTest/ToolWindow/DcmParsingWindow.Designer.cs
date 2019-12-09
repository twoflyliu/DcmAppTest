namespace CSDcmTest
{
    partial class DcmParsingWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.clearToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolButton = new System.Windows.Forms.ToolStripButton();
            this.loadToolButton = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dcmDataDGV = new System.Windows.Forms.DataGridView();
            this.ColumnCanId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parsingDataDGV = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dcmDataDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parsingDataDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // ToolBar
            // 
            this.ToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearToolButton,
            this.toolStripSeparator1,
            this.saveToolButton,
            this.loadToolButton});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(800, 27);
            this.ToolBar.TabIndex = 1;
            this.ToolBar.Text = "toolStrip1";
            // 
            // clearToolButton
            // 
            this.clearToolButton.Image = global::CSDcmTest.Properties.Resources.Clear;
            this.clearToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearToolButton.Name = "clearToolButton";
            this.clearToolButton.Size = new System.Drawing.Size(63, 24);
            this.clearToolButton.Text = "清空";
            this.clearToolButton.Click += new System.EventHandler(this.clearToolButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // saveToolButton
            // 
            this.saveToolButton.Image = global::CSDcmTest.Properties.Resources.Save;
            this.saveToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolButton.Name = "saveToolButton";
            this.saveToolButton.Size = new System.Drawing.Size(63, 24);
            this.saveToolButton.Text = "保存";
            this.saveToolButton.Click += new System.EventHandler(this.saveToolButton_Click);
            // 
            // loadToolButton
            // 
            this.loadToolButton.Image = global::CSDcmTest.Properties.Resources.Open;
            this.loadToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadToolButton.Name = "loadToolButton";
            this.loadToolButton.Size = new System.Drawing.Size(63, 24);
            this.loadToolButton.Text = "加载";
            this.loadToolButton.Click += new System.EventHandler(this.loadToolButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(57)))), ((int)(((byte)(85)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dcmDataDGV);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.parsingDataDGV);
            this.splitContainer1.Size = new System.Drawing.Size(800, 423);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 2;
            // 
            // dcmDataDGV
            // 
            this.dcmDataDGV.AllowUserToAddRows = false;
            this.dcmDataDGV.AllowUserToDeleteRows = false;
            this.dcmDataDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dcmDataDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dcmDataDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCanId,
            this.ColumnData});
            this.dcmDataDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dcmDataDGV.Location = new System.Drawing.Point(0, 0);
            this.dcmDataDGV.MultiSelect = false;
            this.dcmDataDGV.Name = "dcmDataDGV";
            this.dcmDataDGV.RowHeadersWidth = 32;
            this.dcmDataDGV.RowTemplate.Height = 27;
            this.dcmDataDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dcmDataDGV.Size = new System.Drawing.Size(400, 423);
            this.dcmDataDGV.TabIndex = 0;
            this.dcmDataDGV.SelectionChanged += new System.EventHandler(this.dcmDataDGV_SelectionChanged);
            // 
            // ColumnCanId
            // 
            this.ColumnCanId.HeaderText = "CanID(Hex)";
            this.ColumnCanId.Name = "ColumnCanId";
            this.ColumnCanId.ReadOnly = true;
            this.ColumnCanId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnData
            // 
            this.ColumnData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ColumnData.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnData.HeaderText = "数据(Hex)";
            this.ColumnData.Name = "ColumnData";
            this.ColumnData.ReadOnly = true;
            this.ColumnData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // parsingDataDGV
            // 
            this.parsingDataDGV.AllowUserToAddRows = false;
            this.parsingDataDGV.AllowUserToDeleteRows = false;
            this.parsingDataDGV.BackgroundColor = System.Drawing.SystemColors.Window;
            this.parsingDataDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parsingDataDGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnValue});
            this.parsingDataDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parsingDataDGV.Location = new System.Drawing.Point(0, 0);
            this.parsingDataDGV.Name = "parsingDataDGV";
            this.parsingDataDGV.RowHeadersWidth = 32;
            this.parsingDataDGV.RowTemplate.Height = 27;
            this.parsingDataDGV.Size = new System.Drawing.Size(394, 423);
            this.parsingDataDGV.TabIndex = 0;
            // 
            // ColumnName
            // 
            this.ColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnName.HeaderText = "名称";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            this.ColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnValue
            // 
            this.ColumnValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnValue.FillWeight = 150F;
            this.ColumnValue.HeaderText = "值";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.ReadOnly = true;
            this.ColumnValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DcmParsingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ToolBar);
            this.Name = "DcmParsingWindow";
            this.Text = "诊断数据";
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dcmDataDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parsingDataDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton clearToolButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView parsingDataDGV;
        private System.Windows.Forms.DataGridView dcmDataDGV;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCanId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton saveToolButton;
        private System.Windows.Forms.ToolStripButton loadToolButton;
    }
}
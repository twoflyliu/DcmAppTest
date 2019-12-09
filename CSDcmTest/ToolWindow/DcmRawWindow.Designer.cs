namespace CSDcmTest
{
    partial class DcmRawWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DcmRawWindow));
            this.listBox = new System.Windows.Forms.ListBox();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.toolButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonSave = new System.Windows.Forms.ToolStripButton();
            this.ToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 15;
            this.listBox.Location = new System.Drawing.Point(0, 27);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(800, 423);
            this.listBox.TabIndex = 0;
            // 
            // ToolBar
            // 
            this.ToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolButtonClear,
            this.toolStripSeparator1,
            this.toolButtonSave});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(800, 27);
            this.ToolBar.TabIndex = 1;
            this.ToolBar.Text = "toolStrip1";
            // 
            // toolButtonClear
            // 
            this.toolButtonClear.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonClear.Image")));
            this.toolButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonClear.Name = "toolButtonClear";
            this.toolButtonClear.Size = new System.Drawing.Size(63, 24);
            this.toolButtonClear.Text = "清空";
            this.toolButtonClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolButtonClear.Click += new System.EventHandler(this.toolButtonClear_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolButtonSave
            // 
            this.toolButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonSave.Image")));
            this.toolButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonSave.Name = "toolButtonSave";
            this.toolButtonSave.Size = new System.Drawing.Size(63, 24);
            this.toolButtonSave.Text = "保存";
            this.toolButtonSave.Click += new System.EventHandler(this.toolButtonSave_Click);
            // 
            // DcmRawWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.ToolBar);
            this.Name = "DcmRawWindow";
            this.Text = "CAN链路层数据";
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        public System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton toolButtonClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolButtonSave;
    }
}
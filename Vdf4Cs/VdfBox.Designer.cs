namespace Vdf4Cs
{
    partial class VdfBox
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VdfBox));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.TreeViewContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuAddMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuRemoveMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuAddSignal = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuRemoveSignal = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuAddValDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuAddXncodeValDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuAddPhysicalValDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuAddBcdValDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuRemoveValDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contentPanel = new System.Windows.Forms.Panel();
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.toolButtonAddMsg = new System.Windows.Forms.ToolStripButton();
            this.toolButtonRemoveMsg = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonAddSignal = new System.Windows.Forms.ToolStripButton();
            this.toolButtonRemoveSignal = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonAddValDesc = new System.Windows.Forms.ToolStripDropDownButton();
            this.dropDownMenuAddXncodeValDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.dropDownMenuAddPhysicalValDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.dropDownMenuAddBcdValDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.toolButtonRemoveValDesc = new System.Windows.Forms.ToolStripButton();
            this.dropDownMenuAddAsciiValDesc = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuAddAsciiValDesc = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.TreeViewContextMenu.SuspendLayout();
            this.ToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.contentPanel);
            this.splitContainer1.Size = new System.Drawing.Size(938, 397);
            this.splitContainer1.SplitterDistance = 311;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.TreeViewContextMenu;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(311, 397);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // TreeViewContextMenu
            // 
            this.TreeViewContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.TreeViewContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuAddMessage,
            this.contextMenuRemoveMessage,
            this.toolStripMenuItem1,
            this.contextMenuAddSignal,
            this.contextMenuRemoveSignal,
            this.toolStripMenuItem2,
            this.contextMenuAddValDesc,
            this.contextMenuRemoveValDesc});
            this.TreeViewContextMenu.Name = "TreeViewContextMenu";
            this.TreeViewContextMenu.Size = new System.Drawing.Size(215, 200);
            this.TreeViewContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.TreeViewContextMenu_Opening);
            // 
            // contextMenuAddMessage
            // 
            this.contextMenuAddMessage.Image = ((System.Drawing.Image)(resources.GetObject("contextMenuAddMessage.Image")));
            this.contextMenuAddMessage.Name = "contextMenuAddMessage";
            this.contextMenuAddMessage.Size = new System.Drawing.Size(214, 26);
            this.contextMenuAddMessage.Text = "添加报文";
            this.contextMenuAddMessage.Click += new System.EventHandler(this.contextMenuAddMessage_Click);
            // 
            // contextMenuRemoveMessage
            // 
            this.contextMenuRemoveMessage.Image = ((System.Drawing.Image)(resources.GetObject("contextMenuRemoveMessage.Image")));
            this.contextMenuRemoveMessage.Name = "contextMenuRemoveMessage";
            this.contextMenuRemoveMessage.Size = new System.Drawing.Size(214, 26);
            this.contextMenuRemoveMessage.Text = "移除报文";
            this.contextMenuRemoveMessage.Click += new System.EventHandler(this.contextMenuRemoveMessage_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(211, 6);
            // 
            // contextMenuAddSignal
            // 
            this.contextMenuAddSignal.Image = ((System.Drawing.Image)(resources.GetObject("contextMenuAddSignal.Image")));
            this.contextMenuAddSignal.Name = "contextMenuAddSignal";
            this.contextMenuAddSignal.Size = new System.Drawing.Size(214, 26);
            this.contextMenuAddSignal.Text = "添加信号";
            this.contextMenuAddSignal.Click += new System.EventHandler(this.contextMenuAddSignal_Click);
            // 
            // contextMenuRemoveSignal
            // 
            this.contextMenuRemoveSignal.Image = ((System.Drawing.Image)(resources.GetObject("contextMenuRemoveSignal.Image")));
            this.contextMenuRemoveSignal.Name = "contextMenuRemoveSignal";
            this.contextMenuRemoveSignal.Size = new System.Drawing.Size(214, 26);
            this.contextMenuRemoveSignal.Text = "移除信号";
            this.contextMenuRemoveSignal.Click += new System.EventHandler(this.contextMenuRemoveSignal_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(211, 6);
            // 
            // contextMenuAddValDesc
            // 
            this.contextMenuAddValDesc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuAddXncodeValDesc,
            this.contextMenuAddPhysicalValDesc,
            this.contextMenuAddBcdValDesc,
            this.contextMenuAddAsciiValDesc});
            this.contextMenuAddValDesc.Image = ((System.Drawing.Image)(resources.GetObject("contextMenuAddValDesc.Image")));
            this.contextMenuAddValDesc.Name = "contextMenuAddValDesc";
            this.contextMenuAddValDesc.Size = new System.Drawing.Size(214, 26);
            this.contextMenuAddValDesc.Text = "添加值描述";
            // 
            // contextMenuAddXncodeValDesc
            // 
            this.contextMenuAddXncodeValDesc.Name = "contextMenuAddXncodeValDesc";
            this.contextMenuAddXncodeValDesc.Size = new System.Drawing.Size(216, 26);
            this.contextMenuAddXncodeValDesc.Text = "添加Xncode值描述";
            this.contextMenuAddXncodeValDesc.Click += new System.EventHandler(this.dropDownMenuAddXncodeValDesc_Click);
            // 
            // contextMenuAddPhysicalValDesc
            // 
            this.contextMenuAddPhysicalValDesc.Name = "contextMenuAddPhysicalValDesc";
            this.contextMenuAddPhysicalValDesc.Size = new System.Drawing.Size(216, 26);
            this.contextMenuAddPhysicalValDesc.Text = "添加Physical值描述";
            this.contextMenuAddPhysicalValDesc.Click += new System.EventHandler(this.dropDownMenuAddPhysicalValDesc_Click);
            // 
            // contextMenuAddBcdValDesc
            // 
            this.contextMenuAddBcdValDesc.Name = "contextMenuAddBcdValDesc";
            this.contextMenuAddBcdValDesc.Size = new System.Drawing.Size(216, 26);
            this.contextMenuAddBcdValDesc.Text = "添加Bcd值描述";
            this.contextMenuAddBcdValDesc.Click += new System.EventHandler(this.dropDownMenuAddBcdValDesc_Click);
            // 
            // contextMenuRemoveValDesc
            // 
            this.contextMenuRemoveValDesc.Image = ((System.Drawing.Image)(resources.GetObject("contextMenuRemoveValDesc.Image")));
            this.contextMenuRemoveValDesc.Name = "contextMenuRemoveValDesc";
            this.contextMenuRemoveValDesc.Size = new System.Drawing.Size(214, 26);
            this.contextMenuRemoveValDesc.Text = "移除值描述";
            this.contextMenuRemoveValDesc.Click += new System.EventHandler(this.contextMenuRemoveValDesc_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Group.bmp");
            this.imageList.Images.SetKeyName(1, "Message.bmp");
            this.imageList.Images.SetKeyName(2, "Signal.bmp");
            this.imageList.Images.SetKeyName(3, "Value.bmp");
            // 
            // contentPanel
            // 
            this.contentPanel.AutoScroll = true;
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 0);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(623, 397);
            this.contentPanel.TabIndex = 0;
            // 
            // ToolBar
            // 
            this.ToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolButtonAddMsg,
            this.toolButtonRemoveMsg,
            this.toolStripSeparator1,
            this.toolButtonAddSignal,
            this.toolButtonRemoveSignal,
            this.toolStripSeparator2,
            this.toolButtonAddValDesc,
            this.toolButtonRemoveValDesc});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(938, 27);
            this.ToolBar.TabIndex = 1;
            this.ToolBar.Text = "toolStrip1";
            // 
            // toolButtonAddMsg
            // 
            this.toolButtonAddMsg.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonAddMsg.Image")));
            this.toolButtonAddMsg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAddMsg.Name = "toolButtonAddMsg";
            this.toolButtonAddMsg.Size = new System.Drawing.Size(93, 24);
            this.toolButtonAddMsg.Text = "添加报文";
            this.toolButtonAddMsg.Click += new System.EventHandler(this.toolButtonAddMsg_Click);
            // 
            // toolButtonRemoveMsg
            // 
            this.toolButtonRemoveMsg.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonRemoveMsg.Image")));
            this.toolButtonRemoveMsg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonRemoveMsg.Name = "toolButtonRemoveMsg";
            this.toolButtonRemoveMsg.Size = new System.Drawing.Size(93, 24);
            this.toolButtonRemoveMsg.Text = "移除报文";
            this.toolButtonRemoveMsg.Click += new System.EventHandler(this.toolButtonRemoveMsg_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolButtonAddSignal
            // 
            this.toolButtonAddSignal.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonAddSignal.Image")));
            this.toolButtonAddSignal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAddSignal.Name = "toolButtonAddSignal";
            this.toolButtonAddSignal.Size = new System.Drawing.Size(93, 24);
            this.toolButtonAddSignal.Text = "添加信号";
            this.toolButtonAddSignal.Click += new System.EventHandler(this.toolButtonAddSignal_Click);
            // 
            // toolButtonRemoveSignal
            // 
            this.toolButtonRemoveSignal.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonRemoveSignal.Image")));
            this.toolButtonRemoveSignal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonRemoveSignal.Name = "toolButtonRemoveSignal";
            this.toolButtonRemoveSignal.Size = new System.Drawing.Size(93, 24);
            this.toolButtonRemoveSignal.Text = "移除信号";
            this.toolButtonRemoveSignal.Click += new System.EventHandler(this.toolButtonRemoveSignal_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolButtonAddValDesc
            // 
            this.toolButtonAddValDesc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dropDownMenuAddXncodeValDesc,
            this.dropDownMenuAddPhysicalValDesc,
            this.dropDownMenuAddBcdValDesc,
            this.dropDownMenuAddAsciiValDesc});
            this.toolButtonAddValDesc.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonAddValDesc.Image")));
            this.toolButtonAddValDesc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAddValDesc.Name = "toolButtonAddValDesc";
            this.toolButtonAddValDesc.Size = new System.Drawing.Size(118, 24);
            this.toolButtonAddValDesc.Text = "添加值描述";
            // 
            // dropDownMenuAddXncodeValDesc
            // 
            this.dropDownMenuAddXncodeValDesc.Name = "dropDownMenuAddXncodeValDesc";
            this.dropDownMenuAddXncodeValDesc.Size = new System.Drawing.Size(216, 26);
            this.dropDownMenuAddXncodeValDesc.Text = "添加Xncode值描述";
            this.dropDownMenuAddXncodeValDesc.Click += new System.EventHandler(this.dropDownMenuAddXncodeValDesc_Click);
            // 
            // dropDownMenuAddPhysicalValDesc
            // 
            this.dropDownMenuAddPhysicalValDesc.Name = "dropDownMenuAddPhysicalValDesc";
            this.dropDownMenuAddPhysicalValDesc.Size = new System.Drawing.Size(216, 26);
            this.dropDownMenuAddPhysicalValDesc.Text = "添加Physical值描述";
            this.dropDownMenuAddPhysicalValDesc.Click += new System.EventHandler(this.dropDownMenuAddPhysicalValDesc_Click);
            // 
            // dropDownMenuAddBcdValDesc
            // 
            this.dropDownMenuAddBcdValDesc.Name = "dropDownMenuAddBcdValDesc";
            this.dropDownMenuAddBcdValDesc.Size = new System.Drawing.Size(216, 26);
            this.dropDownMenuAddBcdValDesc.Text = "添加Bcd值描述";
            this.dropDownMenuAddBcdValDesc.Click += new System.EventHandler(this.dropDownMenuAddBcdValDesc_Click);
            // 
            // toolButtonRemoveValDesc
            // 
            this.toolButtonRemoveValDesc.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonRemoveValDesc.Image")));
            this.toolButtonRemoveValDesc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonRemoveValDesc.Name = "toolButtonRemoveValDesc";
            this.toolButtonRemoveValDesc.Size = new System.Drawing.Size(108, 24);
            this.toolButtonRemoveValDesc.Text = "移除值描述";
            this.toolButtonRemoveValDesc.Click += new System.EventHandler(this.toolButtonRemoveValDesc_Click);
            // 
            // dropDownMenuAddAsciiValDesc
            // 
            this.dropDownMenuAddAsciiValDesc.Name = "dropDownMenuAddAsciiValDesc";
            this.dropDownMenuAddAsciiValDesc.Size = new System.Drawing.Size(216, 26);
            this.dropDownMenuAddAsciiValDesc.Text = "添加Ascii值描述";
            this.dropDownMenuAddAsciiValDesc.Click += new System.EventHandler(this.dropDownMenuAddAsciiValDesc_Click);
            // 
            // contextMenuAddAsciiValDesc
            // 
            this.contextMenuAddAsciiValDesc.Name = "contextMenuAddAsciiValDesc";
            this.contextMenuAddAsciiValDesc.Size = new System.Drawing.Size(216, 26);
            this.contextMenuAddAsciiValDesc.Text = "添加Ascii值描述";
            this.contextMenuAddAsciiValDesc.Click += new System.EventHandler(this.dropDownMenuAddAsciiValDesc_Click);
            // 
            // VdfBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ToolBar);
            this.Name = "VdfBox";
            this.Size = new System.Drawing.Size(938, 424);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.TreeViewContextMenu.ResumeLayout(false);
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.ToolStripButton toolButtonAddMsg;
        public System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton toolButtonRemoveMsg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolButtonAddSignal;
        private System.Windows.Forms.ToolStripButton toolButtonRemoveSignal;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolButtonRemoveValDesc;
        public System.Windows.Forms.ContextMenuStrip TreeViewContextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextMenuAddMessage;
        private System.Windows.Forms.ToolStripMenuItem contextMenuRemoveMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuAddSignal;
        private System.Windows.Forms.ToolStripMenuItem contextMenuRemoveSignal;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem contextMenuAddValDesc;
        private System.Windows.Forms.ToolStripMenuItem contextMenuRemoveValDesc;
        private System.Windows.Forms.ToolStripDropDownButton toolButtonAddValDesc;
        private System.Windows.Forms.ToolStripMenuItem dropDownMenuAddXncodeValDesc;
        private System.Windows.Forms.ToolStripMenuItem dropDownMenuAddPhysicalValDesc;
        private System.Windows.Forms.ToolStripMenuItem dropDownMenuAddBcdValDesc;
        private System.Windows.Forms.ToolStripMenuItem contextMenuAddXncodeValDesc;
        private System.Windows.Forms.ToolStripMenuItem contextMenuAddPhysicalValDesc;
        private System.Windows.Forms.ToolStripMenuItem contextMenuAddBcdValDesc;
        private System.Windows.Forms.ToolStripMenuItem dropDownMenuAddAsciiValDesc;
        private System.Windows.Forms.ToolStripMenuItem contextMenuAddAsciiValDesc;
    }
}

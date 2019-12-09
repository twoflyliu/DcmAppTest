namespace CSDcmTest
{
    partial class DcmTreeWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DcmTreeWindow));
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.ToolBar = new System.Windows.Forms.ToolStrip();
            this.toggleAllToolButton = new System.Windows.Forms.ToolStripButton();
            this.TreeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuAddService = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuEditService = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuRemoveService = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuAddSubFun = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuEditSubFun = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuRemoveSubFun = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolBar.SuspendLayout();
            this.TreeContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.ContextMenuStrip = this.TreeContextMenuStrip;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.FullRowSelect = true;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(0, 27);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(800, 423);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            this.treeView.Validating += new System.ComponentModel.CancelEventHandler(this.treeView_Validating);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Service.bmp");
            this.imageList.Images.SetKeyName(1, "Function.bmp");
            // 
            // ToolBar
            // 
            this.ToolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleAllToolButton});
            this.ToolBar.Location = new System.Drawing.Point(0, 0);
            this.ToolBar.Name = "ToolBar";
            this.ToolBar.Size = new System.Drawing.Size(800, 27);
            this.ToolBar.TabIndex = 1;
            this.ToolBar.Text = "toolStrip1";
            // 
            // toggleAllToolButton
            // 
            this.toggleAllToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toggleAllToolButton.Image = ((System.Drawing.Image)(resources.GetObject("toggleAllToolButton.Image")));
            this.toggleAllToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggleAllToolButton.Name = "toggleAllToolButton";
            this.toggleAllToolButton.Size = new System.Drawing.Size(73, 24);
            this.toggleAllToolButton.Text = "展开所有";
            this.toggleAllToolButton.Click += new System.EventHandler(this.toggleAllToolButton_Click);
            // 
            // ContextMenuStrip
            // 
            this.TreeContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.TreeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuAddService,
            this.contextMenuEditService,
            this.contextMenuRemoveService,
            this.toolStripMenuItem1,
            this.contextMenuAddSubFun,
            this.contextMenuEditSubFun,
            this.contextMenuRemoveSubFun});
            this.TreeContextMenuStrip.Name = "ContextMenuStrip";
            this.TreeContextMenuStrip.Size = new System.Drawing.Size(215, 194);
            this.TreeContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
            // 
            // contextMenuAddService
            // 
            this.contextMenuAddService.Image = global::CSDcmTest.Properties.Resources.Add;
            this.contextMenuAddService.Name = "contextMenuAddService";
            this.contextMenuAddService.Size = new System.Drawing.Size(214, 26);
            this.contextMenuAddService.Text = "添加服务";
            this.contextMenuAddService.Click += new System.EventHandler(this.contextMenuAddService_Click);
            // 
            // contextMenuEditService
            // 
            this.contextMenuEditService.Image = global::CSDcmTest.Properties.Resources.Refresh;
            this.contextMenuEditService.Name = "contextMenuEditService";
            this.contextMenuEditService.Size = new System.Drawing.Size(214, 26);
            this.contextMenuEditService.Text = "编辑服务";
            this.contextMenuEditService.Click += new System.EventHandler(this.contextMenuEditService_Click);
            // 
            // contextMenuRemoveService
            // 
            this.contextMenuRemoveService.Image = global::CSDcmTest.Properties.Resources.Remove;
            this.contextMenuRemoveService.Name = "contextMenuRemoveService";
            this.contextMenuRemoveService.Size = new System.Drawing.Size(214, 26);
            this.contextMenuRemoveService.Text = "移除服务";
            this.contextMenuRemoveService.Click += new System.EventHandler(this.contextMenuRemoveService_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(207, 6);
            // 
            // contextMenuAddSubFun
            // 
            this.contextMenuAddSubFun.Image = global::CSDcmTest.Properties.Resources.Add;
            this.contextMenuAddSubFun.Name = "contextMenuAddSubFun";
            this.contextMenuAddSubFun.Size = new System.Drawing.Size(214, 26);
            this.contextMenuAddSubFun.Text = "添加子功能";
            this.contextMenuAddSubFun.Click += new System.EventHandler(this.contextMenuAddSubFun_Click);
            // 
            // contextMenuEditSubFun
            // 
            this.contextMenuEditSubFun.Image = global::CSDcmTest.Properties.Resources.Refresh;
            this.contextMenuEditSubFun.Name = "contextMenuEditSubFun";
            this.contextMenuEditSubFun.Size = new System.Drawing.Size(214, 26);
            this.contextMenuEditSubFun.Text = "编辑子功能";
            this.contextMenuEditSubFun.Click += new System.EventHandler(this.contextMenuEditSubFun_Click);
            // 
            // contextMenuRemoveSubFun
            // 
            this.contextMenuRemoveSubFun.Image = global::CSDcmTest.Properties.Resources.Remove;
            this.contextMenuRemoveSubFun.Name = "contextMenuRemoveSubFun";
            this.contextMenuRemoveSubFun.Size = new System.Drawing.Size(214, 26);
            this.contextMenuRemoveSubFun.Text = "移除子功能";
            this.contextMenuRemoveSubFun.Click += new System.EventHandler(this.contextMenuRemoveSubFun_Click);
            // 
            // DcmTreeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.ToolBar);
            this.Name = "DcmTreeWindow";
            this.Text = "诊断服务列表";
            this.ToolBar.ResumeLayout(false);
            this.ToolBar.PerformLayout();
            this.TreeContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ImageList imageList;
        public System.Windows.Forms.ToolStrip ToolBar;
        private System.Windows.Forms.ToolStripButton toggleAllToolButton;
        private System.Windows.Forms.ToolStripMenuItem contextMenuAddService;
        private System.Windows.Forms.ToolStripMenuItem contextMenuEditService;
        private System.Windows.Forms.ToolStripMenuItem contextMenuRemoveService;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem contextMenuAddSubFun;
        private System.Windows.Forms.ToolStripMenuItem contextMenuEditSubFun;
        private System.Windows.Forms.ToolStripMenuItem contextMenuRemoveSubFun;
        public System.Windows.Forms.ContextMenuStrip TreeContextMenuStrip;
    }
}
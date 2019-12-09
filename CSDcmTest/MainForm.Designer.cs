namespace CSDcmTest
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuRecentUsedFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.menuConfigVDF = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAddService = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditService = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRemoveService = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuAddSubFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEditSubFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRemoveSubFunction = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCan = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenCan = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCloseCan = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOption = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSwapPhyFunReq = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuVsBlueTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVs2015DarkTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuVs2015LightTheme = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolBar = new System.Windows.Forms.ToolStrip();
            this.newToolButton = new System.Windows.Forms.ToolStripButton();
            this.openToolButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonAddService = new System.Windows.Forms.ToolStripButton();
            this.toolButtonEditService = new System.Windows.Forms.ToolStripButton();
            this.toolButtonRemoveService = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonAddSubFunction = new System.Windows.Forms.ToolStripButton();
            this.toolButtonEditSubFunction = new System.Windows.Forms.ToolStripButton();
            this.toolButtonRemoveSubFunction = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolButtonCan = new System.Windows.Forms.ToolStripButton();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusInst = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusCurrentTreeItem = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusCanState = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.vS2015BlueTheme = new WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme();
            this.visualStudioToolStripExtender1 = new WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender(this.components);
            this.mainMenu.SuspendLayout();
            this.toolBar.SuspendLayout();
            this.statusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainDockPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuEdit,
            this.menuCan,
            this.menuOption,
            this.menuHelp});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(1059, 28);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNew,
            this.menuOpen,
            this.menuSave,
            this.menuSaveAs,
            this.toolStripMenuItem2,
            this.menuRecentUsedFile,
            this.toolStripMenuItem1,
            this.menuExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(69, 24);
            this.menuFile.Text = "文件(&F)";
            // 
            // menuNew
            // 
            this.menuNew.Image = global::CSDcmTest.Properties.Resources.New;
            this.menuNew.Name = "menuNew";
            this.menuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuNew.Size = new System.Drawing.Size(228, 26);
            this.menuNew.Text = "新建";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuOpen
            // 
            this.menuOpen.Image = global::CSDcmTest.Properties.Resources.Open;
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuOpen.Size = new System.Drawing.Size(228, 26);
            this.menuOpen.Text = "打开";
            this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // menuSave
            // 
            this.menuSave.Image = global::CSDcmTest.Properties.Resources.Save;
            this.menuSave.Name = "menuSave";
            this.menuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menuSave.Size = new System.Drawing.Size(228, 26);
            this.menuSave.Text = "保存";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Image = global::CSDcmTest.Properties.Resources.Save;
            this.menuSaveAs.Name = "menuSaveAs";
            this.menuSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.menuSaveAs.Size = new System.Drawing.Size(228, 26);
            this.menuSaveAs.Text = "另存为";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(225, 6);
            // 
            // menuRecentUsedFile
            // 
            this.menuRecentUsedFile.Name = "menuRecentUsedFile";
            this.menuRecentUsedFile.Size = new System.Drawing.Size(228, 26);
            this.menuRecentUsedFile.Text = "最近使用过的文件";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(225, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(228, 26);
            this.menuExit.Text = "退出(&X)";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuEdit
            // 
            this.menuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.menuConfigVDF,
            this.toolStripMenuItem7,
            this.menuAddService,
            this.menuEditService,
            this.menuRemoveService,
            this.toolStripMenuItem3,
            this.menuAddSubFunction,
            this.menuEditSubFunction,
            this.menuRemoveSubFunction});
            this.menuEdit.Name = "menuEdit";
            this.menuEdit.Size = new System.Drawing.Size(69, 24);
            this.menuEdit.Text = "编辑(&E)";
            this.menuEdit.DropDownOpening += new System.EventHandler(this.menuEdit_DropDownOpening);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(213, 6);
            // 
            // menuConfigVDF
            // 
            this.menuConfigVDF.Name = "menuConfigVDF";
            this.menuConfigVDF.Size = new System.Drawing.Size(216, 26);
            this.menuConfigVDF.Text = "配置VDF";
            this.menuConfigVDF.Click += new System.EventHandler(this.menuConfigVDF_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(213, 6);
            // 
            // menuAddService
            // 
            this.menuAddService.Image = global::CSDcmTest.Properties.Resources.Add;
            this.menuAddService.Name = "menuAddService";
            this.menuAddService.Size = new System.Drawing.Size(216, 26);
            this.menuAddService.Text = "添加服务";
            this.menuAddService.Click += new System.EventHandler(this.menuAddService_Click);
            // 
            // menuEditService
            // 
            this.menuEditService.Image = global::CSDcmTest.Properties.Resources.Refresh;
            this.menuEditService.Name = "menuEditService";
            this.menuEditService.Size = new System.Drawing.Size(216, 26);
            this.menuEditService.Text = "编辑服务";
            this.menuEditService.Click += new System.EventHandler(this.menuEditService_Click);
            // 
            // menuRemoveService
            // 
            this.menuRemoveService.Image = global::CSDcmTest.Properties.Resources.Remove;
            this.menuRemoveService.Name = "menuRemoveService";
            this.menuRemoveService.Size = new System.Drawing.Size(216, 26);
            this.menuRemoveService.Text = "移除服务";
            this.menuRemoveService.Click += new System.EventHandler(this.menuRemoveService_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(213, 6);
            // 
            // menuAddSubFunction
            // 
            this.menuAddSubFunction.Image = global::CSDcmTest.Properties.Resources.Add;
            this.menuAddSubFunction.Name = "menuAddSubFunction";
            this.menuAddSubFunction.Size = new System.Drawing.Size(216, 26);
            this.menuAddSubFunction.Text = "添加子功能";
            this.menuAddSubFunction.Click += new System.EventHandler(this.menuAddSubFunction_Click);
            // 
            // menuEditSubFunction
            // 
            this.menuEditSubFunction.Image = global::CSDcmTest.Properties.Resources.Refresh;
            this.menuEditSubFunction.Name = "menuEditSubFunction";
            this.menuEditSubFunction.Size = new System.Drawing.Size(216, 26);
            this.menuEditSubFunction.Text = "编辑子功能";
            this.menuEditSubFunction.Click += new System.EventHandler(this.menuEditSubFunction_Click);
            // 
            // menuRemoveSubFunction
            // 
            this.menuRemoveSubFunction.Image = global::CSDcmTest.Properties.Resources.Remove;
            this.menuRemoveSubFunction.Name = "menuRemoveSubFunction";
            this.menuRemoveSubFunction.Size = new System.Drawing.Size(216, 26);
            this.menuRemoveSubFunction.Text = "移除子功能";
            this.menuRemoveSubFunction.Click += new System.EventHandler(this.menuRemoveSubFunction_Click);
            // 
            // menuCan
            // 
            this.menuCan.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenCan,
            this.menuCloseCan});
            this.menuCan.Name = "menuCan";
            this.menuCan.Size = new System.Drawing.Size(72, 24);
            this.menuCan.Text = "设备(&D)";
            this.menuCan.DropDownOpening += new System.EventHandler(this.menuCan_DropDownOpening);
            // 
            // menuOpenCan
            // 
            this.menuOpenCan.Image = ((System.Drawing.Image)(resources.GetObject("menuOpenCan.Image")));
            this.menuOpenCan.Name = "menuOpenCan";
            this.menuOpenCan.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.O)));
            this.menuOpenCan.Size = new System.Drawing.Size(249, 26);
            this.menuOpenCan.Text = "打开CAN";
            this.menuOpenCan.Click += new System.EventHandler(this.menuOpenCan_Click);
            // 
            // menuCloseCan
            // 
            this.menuCloseCan.Image = ((System.Drawing.Image)(resources.GetObject("menuCloseCan.Image")));
            this.menuCloseCan.Name = "menuCloseCan";
            this.menuCloseCan.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.menuCloseCan.Size = new System.Drawing.Size(249, 26);
            this.menuCloseCan.Text = "关闭CAN";
            this.menuCloseCan.Click += new System.EventHandler(this.menuCloseCan_Click);
            // 
            // menuOption
            // 
            this.menuOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSwapPhyFunReq,
            this.toolStripMenuItem4,
            this.menuVsBlueTheme,
            this.menuVs2015DarkTheme,
            this.menuVs2015LightTheme});
            this.menuOption.Name = "menuOption";
            this.menuOption.Size = new System.Drawing.Size(73, 24);
            this.menuOption.Text = "选项(&O)";
            this.menuOption.DropDownOpening += new System.EventHandler(this.menuOption_DropDownOpening);
            // 
            // menuSwapPhyFunReq
            // 
            this.menuSwapPhyFunReq.Name = "menuSwapPhyFunReq";
            this.menuSwapPhyFunReq.Size = new System.Drawing.Size(210, 26);
            this.menuSwapPhyFunReq.Text = "交换物理/功能请求";
            this.menuSwapPhyFunReq.Click += new System.EventHandler(this.menuSwapPhyFunReq_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(207, 6);
            // 
            // menuVsBlueTheme
            // 
            this.menuVsBlueTheme.Checked = true;
            this.menuVsBlueTheme.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuVsBlueTheme.Name = "menuVsBlueTheme";
            this.menuVsBlueTheme.Size = new System.Drawing.Size(210, 26);
            this.menuVsBlueTheme.Text = "VS2015 Blue主题";
            this.menuVsBlueTheme.Click += new System.EventHandler(this.menuVsBlueTheme_Click);
            // 
            // menuVs2015DarkTheme
            // 
            this.menuVs2015DarkTheme.Name = "menuVs2015DarkTheme";
            this.menuVs2015DarkTheme.Size = new System.Drawing.Size(210, 26);
            this.menuVs2015DarkTheme.Text = "VS2015 Dark主题";
            this.menuVs2015DarkTheme.Click += new System.EventHandler(this.menuVs2015DarkTheme_Click);
            // 
            // menuVs2015LightTheme
            // 
            this.menuVs2015LightTheme.Name = "menuVs2015LightTheme";
            this.menuVs2015LightTheme.Size = new System.Drawing.Size(210, 26);
            this.menuVs2015LightTheme.Text = "VS2015 Light主题";
            this.menuVs2015LightTheme.Click += new System.EventHandler(this.menuVs2015LightTheme_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(73, 24);
            this.menuHelp.Text = "帮助(&H)";
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(147, 26);
            this.menuAbout.Text = "关于...(&A)";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // toolBar
            // 
            this.toolBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolButton,
            this.openToolButton,
            this.saveToolButton,
            this.toolStripSeparator1,
            this.toolButtonAddService,
            this.toolButtonEditService,
            this.toolButtonRemoveService,
            this.toolStripSeparator2,
            this.toolButtonAddSubFunction,
            this.toolButtonEditSubFunction,
            this.toolButtonRemoveSubFunction,
            this.toolStripSeparator3,
            this.toolButtonCan});
            this.toolBar.Location = new System.Drawing.Point(0, 28);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(1059, 27);
            this.toolBar.TabIndex = 2;
            this.toolBar.Text = "toolStrip1";
            // 
            // newToolButton
            // 
            this.newToolButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolButton.Image")));
            this.newToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolButton.Name = "newToolButton";
            this.newToolButton.Size = new System.Drawing.Size(63, 24);
            this.newToolButton.Text = "新建";
            this.newToolButton.Click += new System.EventHandler(this.newToolButton_Click);
            // 
            // openToolButton
            // 
            this.openToolButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolButton.Image")));
            this.openToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolButton.Name = "openToolButton";
            this.openToolButton.Size = new System.Drawing.Size(63, 24);
            this.openToolButton.Text = "打开";
            this.openToolButton.Click += new System.EventHandler(this.openToolButton_Click);
            // 
            // saveToolButton
            // 
            this.saveToolButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolButton.Image")));
            this.saveToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolButton.Name = "saveToolButton";
            this.saveToolButton.Size = new System.Drawing.Size(63, 24);
            this.saveToolButton.Text = "保存";
            this.saveToolButton.Click += new System.EventHandler(this.saveToolButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolButtonAddService
            // 
            this.toolButtonAddService.Image = global::CSDcmTest.Properties.Resources.Add;
            this.toolButtonAddService.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAddService.Name = "toolButtonAddService";
            this.toolButtonAddService.Size = new System.Drawing.Size(93, 24);
            this.toolButtonAddService.Text = "添加服务";
            this.toolButtonAddService.Click += new System.EventHandler(this.toolButtonAddService_Click);
            // 
            // toolButtonEditService
            // 
            this.toolButtonEditService.Enabled = false;
            this.toolButtonEditService.Image = global::CSDcmTest.Properties.Resources.Refresh;
            this.toolButtonEditService.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonEditService.Name = "toolButtonEditService";
            this.toolButtonEditService.Size = new System.Drawing.Size(93, 24);
            this.toolButtonEditService.Text = "编辑服务";
            this.toolButtonEditService.Click += new System.EventHandler(this.toolButtonEditService_Click);
            // 
            // toolButtonRemoveService
            // 
            this.toolButtonRemoveService.Enabled = false;
            this.toolButtonRemoveService.Image = global::CSDcmTest.Properties.Resources.Remove;
            this.toolButtonRemoveService.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonRemoveService.Name = "toolButtonRemoveService";
            this.toolButtonRemoveService.Size = new System.Drawing.Size(93, 24);
            this.toolButtonRemoveService.Text = "移除服务";
            this.toolButtonRemoveService.Click += new System.EventHandler(this.toolButtonRemoveService_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolButtonAddSubFunction
            // 
            this.toolButtonAddSubFunction.Enabled = false;
            this.toolButtonAddSubFunction.Image = global::CSDcmTest.Properties.Resources.Add;
            this.toolButtonAddSubFunction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonAddSubFunction.Name = "toolButtonAddSubFunction";
            this.toolButtonAddSubFunction.Size = new System.Drawing.Size(108, 24);
            this.toolButtonAddSubFunction.Text = "添加子功能";
            this.toolButtonAddSubFunction.Click += new System.EventHandler(this.toolButtonAddSubFunction_Click);
            // 
            // toolButtonEditSubFunction
            // 
            this.toolButtonEditSubFunction.Enabled = false;
            this.toolButtonEditSubFunction.Image = global::CSDcmTest.Properties.Resources.Refresh;
            this.toolButtonEditSubFunction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonEditSubFunction.Name = "toolButtonEditSubFunction";
            this.toolButtonEditSubFunction.Size = new System.Drawing.Size(108, 24);
            this.toolButtonEditSubFunction.Text = "编辑子功能";
            this.toolButtonEditSubFunction.Click += new System.EventHandler(this.toolButtonEditSubFunction_Click);
            // 
            // toolButtonRemoveSubFunction
            // 
            this.toolButtonRemoveSubFunction.Enabled = false;
            this.toolButtonRemoveSubFunction.Image = global::CSDcmTest.Properties.Resources.Remove;
            this.toolButtonRemoveSubFunction.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonRemoveSubFunction.Name = "toolButtonRemoveSubFunction";
            this.toolButtonRemoveSubFunction.Size = new System.Drawing.Size(108, 24);
            this.toolButtonRemoveSubFunction.Text = "移除子功能";
            this.toolButtonRemoveSubFunction.Click += new System.EventHandler(this.toolButtonRemoveSubFunction_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // toolButtonCan
            // 
            this.toolButtonCan.Image = ((System.Drawing.Image)(resources.GetObject("toolButtonCan.Image")));
            this.toolButtonCan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolButtonCan.Name = "toolButtonCan";
            this.toolButtonCan.Size = new System.Drawing.Size(96, 24);
            this.toolButtonCan.Text = "启动CAN";
            this.toolButtonCan.Click += new System.EventHandler(this.toolButtonCan_Click);
            // 
            // statusBar
            // 
            this.statusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusInst,
            this.statusCurrentTreeItem,
            this.statusCanState});
            this.statusBar.Location = new System.Drawing.Point(0, 492);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1059, 25);
            this.statusBar.TabIndex = 3;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusInst
            // 
            this.statusInst.Name = "statusInst";
            this.statusInst.Size = new System.Drawing.Size(867, 20);
            this.statusInst.Spring = true;
            this.statusInst.Text = "就绪 ";
            this.statusInst.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusCurrentTreeItem
            // 
            this.statusCurrentTreeItem.Name = "statusCurrentTreeItem";
            this.statusCurrentTreeItem.Size = new System.Drawing.Size(41, 20);
            this.statusCurrentTreeItem.Text = "        ";
            // 
            // statusCanState
            // 
            this.statusCanState.Name = "statusCanState";
            this.statusCanState.Size = new System.Drawing.Size(136, 20);
            this.statusCanState.Text = "Can State: Closed";
            // 
            // mainDockPanel
            // 
            this.mainDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDockPanel.DockBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(57)))), ((int)(((byte)(85)))));
            this.mainDockPanel.Location = new System.Drawing.Point(0, 55);
            this.mainDockPanel.Name = "mainDockPanel";
            this.mainDockPanel.Padding = new System.Windows.Forms.Padding(6);
            this.mainDockPanel.ShowAutoHideContentOnHover = false;
            this.mainDockPanel.Size = new System.Drawing.Size(1059, 437);
            this.mainDockPanel.TabIndex = 4;
            this.mainDockPanel.Theme = this.vS2015BlueTheme;
            // 
            // visualStudioToolStripExtender1
            // 
            this.visualStudioToolStripExtender1.DefaultRenderer = null;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1059, 517);
            this.Controls.Add(this.mainDockPanel);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.toolBar);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainForm";
            this.Text = "车载诊断测试";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.toolBar.ResumeLayout(false);
            this.toolBar.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainDockPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem menuCan;
        private System.Windows.Forms.ToolStripMenuItem menuOpenCan;
        private System.Windows.Forms.ToolStripMenuItem menuCloseCan;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripButton newToolButton;
        private System.Windows.Forms.ToolStripButton openToolButton;
        private System.Windows.Forms.ToolStripButton saveToolButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusInst;
        private System.Windows.Forms.ToolStripStatusLabel statusCurrentTreeItem;
        private System.Windows.Forms.ToolStripStatusLabel statusCanState;
        private WeifenLuo.WinFormsUI.Docking.DockPanel mainDockPanel;
        private WeifenLuo.WinFormsUI.Docking.VS2015BlueTheme vS2015BlueTheme;
        private WeifenLuo.WinFormsUI.Docking.VisualStudioToolStripExtender visualStudioToolStripExtender1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuNew;
        private System.Windows.Forms.ToolStripMenuItem menuOpen;
        private System.Windows.Forms.ToolStripMenuItem menuSave;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuOption;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripMenuItem menuSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuRecentUsedFile;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem menuSwapPhyFunReq;
        private System.Windows.Forms.ToolStripMenuItem menuVsBlueTheme;
        private System.Windows.Forms.ToolStripMenuItem menuVs2015DarkTheme;
        private System.Windows.Forms.ToolStripMenuItem menuVs2015LightTheme;
        internal System.Windows.Forms.ToolStripMenuItem menuAddService;
        internal System.Windows.Forms.ToolStripMenuItem menuRemoveService;
        internal System.Windows.Forms.ToolStripMenuItem menuAddSubFunction;
        internal System.Windows.Forms.ToolStripMenuItem menuRemoveSubFunction;
        internal System.Windows.Forms.ToolStripMenuItem menuEditService;
        internal System.Windows.Forms.ToolStripMenuItem menuEditSubFunction;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem menuConfigVDF;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripButton toolButtonAddService;
        private System.Windows.Forms.ToolStripButton toolButtonEditService;
        private System.Windows.Forms.ToolStripButton toolButtonRemoveService;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolButtonAddSubFunction;
        private System.Windows.Forms.ToolStripButton toolButtonEditSubFunction;
        private System.Windows.Forms.ToolStripButton toolButtonRemoveSubFunction;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolButtonCan;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DcmConfig;
using SecurityAccessContract;
using WeifenLuo.WinFormsUI.Docking;

namespace CSDcmTest
{
    public partial class MainForm : Form
    {
        private DcmTreeWindow dcmTreeWindow = null;
        private DcmContentWindow dcmContentWindow = null;
        private DcmConfigWindow dcmConfigWindow = null;
        private DcmParsingWindow dcmParsingWindow = null;
        private DcmRawWindow dcmRawWindow = null;
        private DcmVdfWindow dcmVdfWindow = null;

        public const string DocumentExtension = ".dcmproj";
        private const string AppName = "车载诊断测试";
        public const string AppEnglishName = "CSDcmTest";

        public const string DockLayoutFile = "DockLayout.xml";
        public const string ThemeFile = "Theme.dat";

        private bool contentChanged;
        public bool ContentChanged
        {
            get { return contentChanged; }
            set
            {
                const string changedEndStr = "*";

                contentChanged = value;
                if (contentChanged)
                {
                    if (!Text.EndsWith(changedEndStr))
                    {
                        Text += changedEndStr;
                    }
                }
                else
                {
                    if (Text.EndsWith(changedEndStr))
                    {
                        Text = Text.TrimEnd(changedEndStr.ToCharArray());
                    }
                }
            }
        }

        private string appDockLayoutFile;
        public string AppDockLayoutFile
        {
            get
            {
                if (appDockLayoutFile == null)
                {
                    appDockLayoutFile = Path.Combine(AppDocumentFolder, DockLayoutFile);
                }
                return appDockLayoutFile;
            }
        }

        private string appThemeFile;
        public string AppThemeFile
        {
            get
            {
                if (appThemeFile == null)
                {
                    appThemeFile = Path.Combine(AppDocumentFolder, ThemeFile);
                }
                return appThemeFile;
            }
        }

        private string appDocumentFolder;
        public string AppDocumentFolder
        {
            get
            {
                if (appDocumentFolder == null)
                {
                    appDocumentFolder = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                        AppEnglishName);
                    if (!Directory.Exists(appDocumentFolder))
                    {
                        Directory.CreateDirectory(appDocumentFolder);
                    }
                }
                return appDocumentFolder;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            InitThemeMenuGroup();

            InitAllToolWindow();

            LoadTheme();
            InitDockStyleRender();

            if (!LoadDockLayout())
            {
                SetupDockLayout();
            }

            BindingIncommingEvent();

            // 打开最后一次打开的文件
            if (dcmConfigWindow.RecentFiles.Count > 0)
            {
                dcmTreeWindow.LoadDocument(dcmConfigWindow.RecentFiles[0]);
            }
            else
            {
                dcmTreeWindow.New(); //默认打开是一个空白的文档
            }

            this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 设置当前文档显示Content窗口
            //dcmContentWindow.Activate();
        }

        private List<ToolStripMenuItem> themeMenuList = new List<ToolStripMenuItem>();
        private void InitThemeMenuGroup()
        {
            themeMenuList.Add(menuVsBlueTheme);
            themeMenuList.Add(menuVs2015DarkTheme);
            themeMenuList.Add(menuVs2015LightTheme);
        }

        private void BindingIncommingEvent()
        {
            DcmService.DcmService.ParsingDataIncomming += dcmParsingWindow.OnParsingDataIncomming;
            DcmService.DcmService.ParsingDataIncomming += dcmTreeWindow.OnParsingDataIncomming;
            DcmService.DcmService.RawDataIncomming += dcmRawWindow.OnRawDataIncomming;
            DcmService.DcmService.SessionChanged += dcmConfigWindow.OnSessionChanged;

            dcmVdfWindow.VdfBox.OnMessageChanged += VdfBox_OnMessageChanged;
            dcmVdfWindow.VdfBox.OnSignalChanged += VdfBox_OnSignalChanged;
            dcmVdfWindow.VdfBox.OnValueDescriptionChanged += VdfBox_OnValueDescriptionChanged;
        }

        private void VdfBox_OnValueDescriptionChanged(Vdf4Cs.ValueDescriptionChangedEventArgs e)
        {
            ContentChanged = true;
            dcmTreeWindow.OnVdfValueDescriptionChanged(e.ValueDesc);
        }

        private void VdfBox_OnSignalChanged(Vdf4Cs.SignalChangedEventArgs e)
        {
            ContentChanged = true;
            dcmTreeWindow.OnVdfSignalChanged(e.Signal);
        }

        private void VdfBox_OnMessageChanged(Vdf4Cs.MessageChangedEventArgs e)
        {
            ContentChanged = true;
            dcmTreeWindow.OnMessageChanged(e.Message);
        }

        private void UnbindingIncommingEvent()
        {
            DcmService.DcmService.ParsingDataIncomming -= dcmParsingWindow.OnParsingDataIncomming;
            DcmService.DcmService.ParsingDataIncomming -= dcmTreeWindow.OnParsingDataIncomming;
            DcmService.DcmService.RawDataIncomming -= dcmRawWindow.OnRawDataIncomming;
            DcmService.DcmService.SessionChanged -= dcmConfigWindow.OnSessionChanged;

        }

        private void InitAllToolWindow()
        {
            dcmTreeWindow = new DcmTreeWindow(this);
            dcmContentWindow = new DcmContentWindow(this);
            dcmConfigWindow = new DcmConfigWindow(this);
            dcmParsingWindow = new DcmParsingWindow();
            dcmRawWindow = new DcmRawWindow();

            dcmVdfWindow = new DcmVdfWindow();
        }

        private void InitDockStyleRender()
        {
            this.mainDockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015,
                mainDockPanel.Theme);
        }

        private void EnableVSRenderer(VisualStudioToolStripExtender.VsVersion version, 
            ThemeBase theme)
        {
            visualStudioToolStripExtender1.SetStyle(mainMenu, version, theme);
            visualStudioToolStripExtender1.SetStyle(toolBar, version, theme);
            visualStudioToolStripExtender1.SetStyle(dcmTreeWindow.ToolBar, version, theme);
            visualStudioToolStripExtender1.SetStyle(dcmTreeWindow.TreeContextMenuStrip, version, theme);
            visualStudioToolStripExtender1.SetStyle(dcmRawWindow.ToolBar, version, theme);
            visualStudioToolStripExtender1.SetStyle(dcmParsingWindow.ToolBar, version, theme);
            visualStudioToolStripExtender1.SetStyle(dcmVdfWindow.ToolBar, version, theme);
            visualStudioToolStripExtender1.SetStyle(dcmVdfWindow.TreeViewContextMenu, version, theme);
            visualStudioToolStripExtender1.SetStyle(statusBar, version, theme);
        }

        internal void SetInstStatus(string msg)
        {
            statusInst.Text = msg;
        }

        private void SetupDockLayout()
        {
            dcmTreeWindow.Show(mainDockPanel, DockState.DockLeft);
            dcmContentWindow.Show(mainDockPanel, DockState.Document);
            dcmVdfWindow.Show(mainDockPanel, DockState.Document);
            dcmConfigWindow.Show(mainDockPanel, DockState.DockRight);

            dcmParsingWindow.Show(dcmContentWindow.Pane, DockAlignment.Bottom, 0.40);
            dcmRawWindow.Show(dcmConfigWindow.Pane, DockAlignment.Bottom, 0.40);
        }

        delegate void UpdateSubFunctionFn(DcmConfig.SubFunction subFunction);
        public void UpdateSubFunction(DcmConfig.SubFunction subFunction)
        {
            if (InvokeRequired)
            {
                UpdateSubFunctionFn fn = UpdateSubFunction;
                this.Invoke(fn, subFunction);
            }
            else
            {
                dcmContentWindow.Update(subFunction);

                statusCurrentTreeItem.Text = 
                    string.Format("Current SubFunction: {0}", subFunction.Name);
                UpdateToolBarEnabled(false);
            }
        }

        public void UpdateDcmDocument(DcmDocument dcmDocument)
        {
            dcmContentWindow.DcmDocument = dcmDocument;
            dcmParsingWindow.UpdateDcmDocument(dcmDocument);
            dcmConfigWindow.DcmDocument = dcmDocument;
            dcmVdfWindow.DcmDocument = dcmDocument;
        }

        internal void UpdateService(Service service)
        {
            dcmContentWindow.Clear();
            statusCurrentTreeItem.Text =
                string.Format("Current Service: {0}", service.Name);
            UpdateToolBarEnabled(true);
        }

        public void UpdateToolBarEnabled(bool serviceSelected)
        {
            toolButtonEditService.Enabled = serviceSelected;

            // 移除服务使能
            toolButtonRemoveService.Enabled = serviceSelected;

            // 添加子功能使能
            toolButtonAddSubFunction.Enabled = serviceSelected;

            // 编辑子功能使能
            toolButtonEditSubFunction.Enabled = !serviceSelected;

            // 移除子功能使能
            toolButtonRemoveSubFunction.Enabled = !serviceSelected;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!AskUserSaveChanged())
            {
                e.Cancel = true;
                return;
            }

            UnbindingIncommingEvent();
            
            SaveTheme();
            SaveLayout();

            DcmService.DcmService.CloseCan();
            base.OnClosing(e);
        }

        private void SaveLayout()
        {
            try
            {
                mainDockPanel.SaveAsXml(AppDockLayoutFile);
            }
            catch (Exception)
            {
            }
        }

        public bool LoadTheme()
        {
            try
            {
                string typeName = File.ReadAllText(AppThemeFile);
                Type type = Assembly.Load("WeifenLuo.WinFormsUI.Docking.ThemeVS2015")
                    .GetType(typeName);
                mainDockPanel.Theme = System.Activator.CreateInstance(type) as ThemeBase;
                InitThemeByThemeType(mainDockPanel.Theme.GetType());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        private void SaveTheme()
        {
            File.WriteAllText(AppThemeFile, mainDockPanel.Theme.GetType().ToString());
        }

        private bool LoadDockLayout()
        {
            DeserializeDockContent deserializeDockContent =
                new DeserializeDockContent(GetContentFromPersistsString);

            // 加载布局
            try
            {
                mainDockPanel.LoadFromXml(AppDockLayoutFile, deserializeDockContent);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal void OnLoadDocumentDone(string configFile, bool ok)
        {
            //if (ok)
            {
                // 更新窗口标题
                if (!string.IsNullOrEmpty(configFile))
                {
                    Text = string.Format("{0} - {1}", AppName, 
                        Path.GetFileNameWithoutExtension(configFile));
                }
                else
                {
                    Text = string.Format("{0} - {1}", AppName, "New Dcm Document");
                }
            }
        }

        private void CloseAllContents()
        {
            // close all tool windows
            dcmTreeWindow.DockPanel = null;
            dcmContentWindow.DockPanel = null;
            dcmConfigWindow.DockPanel = null;
            dcmParsingWindow.DockPanel = null;
            dcmRawWindow.DockPanel = null;
            dcmVdfWindow.DockPanel = null;

            //close all other document window
            CloseAllDocuments();

            //IMPORTANT: dispose all float window
            foreach (var window in mainDockPanel.FloatWindows.ToList())
            {
                window.Dispose();
            }

            Debug.Assert(mainDockPanel.Panes.Count == 0);
            Debug.Assert(mainDockPanel.Contents.Count == 0);
            Debug.Assert(mainDockPanel.FloatWindows.Count == 0);
        }

        private void CloseAllDocuments()
        {
            if (mainDockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (var form in MdiChildren)
                {
                    form.Close();
                }
            }
            else
            {
                foreach (var document in mainDockPanel.DocumentsToArray())
                {
                    //IMPORTANT: dispose all panes
                    document.DockHandler.DockPanel = null;
                    document.DockHandler.Close();
                }
            }
        }

        internal List<byte> GetPackageData()
        {
            return dcmContentWindow.GetPackageData();
        }

        internal ISecurityAccessAlgorithm GetSecurityAccessAlgorithm()
        {
            return dcmConfigWindow.GetSecurityAccessAlgorithm();
        }

        private IDockContent GetContentFromPersistsString(string persistString)
        {
            if (persistString == typeof(DcmTreeWindow).ToString())
            {
                return dcmTreeWindow;
            }
            else if (persistString == typeof(DcmContentWindow).ToString())
            {
                return dcmContentWindow;
            }
            else if (persistString == typeof(DcmParsingWindow).ToString())
            {
                return dcmParsingWindow;
            }
            else if (persistString == typeof(DcmRawWindow).ToString())
            {
                return dcmRawWindow;
            }
            else if (persistString == typeof(DcmVdfWindow).ToString())
            {
                return dcmVdfWindow;
            }
            else if (persistString.StartsWith(typeof(DcmConfigWindow).ToString()))
            {
                var fields = 
                    persistString.Split(new char[] { DcmConfigWindow.RecentFileCountSplitChar });

                dcmConfigWindow.RecentFiles.Clear();
                for (int i = 1; i < fields.Length; i++)
                {
                    var recentFile = fields[i];
                    dcmConfigWindow.RecentFiles.Add(recentFile);
                }
                UpdateRecentFilesMenu();
                return dcmConfigWindow;
            }
            else
            {
                throw new Exception(string.Format("Invalid persit string: {0}", persistString));
            }
        }

        void UpdateRecentFilesMenu()
        {
            menuRecentUsedFile.DropDownItems.Clear();
            foreach (var recentFile in dcmConfigWindow.RecentFiles)
            {
                menuRecentUsedFile.DropDownItems.Add(NewRecentFileMenu(recentFile));
            }
        }

        private ToolStripMenuItem NewRecentFileMenu(string fileName)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = fileName;
            item.Click += new EventHandler(OpenRecentFile);
            return item;
        }

        private void OpenRecentFile(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            Open(item.Text);
        }

        // 更新CAN状态
        private void menuCan_DropDownOpening(object sender, EventArgs e)
        {
            menuOpenCan.Checked = DcmService.DcmService.CanIsOpened();
            menuCloseCan.Checked = !menuOpenCan.Checked;
        }

        public void OpenCan()
        {
            if (DcmService.DcmService.OpenCan())
            {
                statusCanState.Text = "Can State: Opened";

                toolButtonCan.Text = "停止CAN";
                toolButtonCan.Image = global::CSDcmTest.Properties.Resources.Stop;

                //更新和Can相关的状态(文件）
                UpdateFileMenuTool();
            }
        }

        private void menuOpenCan_Click(object sender, EventArgs e)
        {
            OpenCan();
        }

        public void CloseCan()
        {
            DcmService.DcmService.CloseCan();
            statusCanState.Text = "Can State: Closed";

            toolButtonCan.Text = "启动CAN";
            toolButtonCan.Image = global::CSDcmTest.Properties.Resources.Start;

            UpdateFileMenuTool();
        }

        private void UpdateFileMenuTool()
        {
            var canOpened = DcmService.DcmService.CanIsOpened();
            menuNew.Enabled = !canOpened;
            menuOpen.Enabled = !canOpened;

            newToolButton.Enabled = !canOpened;
            openToolButton.Enabled = !canOpened;
        }

        private void menuCloseCan_Click(object sender, EventArgs e)
        {
            CloseCan();
        }

        private void menuCanSend_Click(object sender, EventArgs e)
        {
            ECAN.CAN_OBJ obj = new ECAN.CAN_OBJ();
            obj.ID = 0x433;
            obj.TimeStamp = 0;
            obj.TimeFlag = 0;
            obj.SendType = 0;
            obj.RemoteFlag = 0;
            obj.ExternFlag = 0;
            obj.DataLen = 8;

            obj.Data = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                obj.Data[i] = 0xff;
            }

            obj.Reserved = new byte[8];
            for (int i = 0; i < 3; i++)
            {
                obj.Reserved[i] = 0;
            }

            if (DcmService.UsbCanUtil.Instance().Send(ref obj))
            {
                Console.WriteLine("Send data successfully");
            }
            else
            {
                Console.WriteLine("Send data failed");
            }
        }

        internal void ClearAllToolWindow()
        {
            dcmTreeWindow.ClearContent();
            dcmContentWindow.ClearContent();
            dcmConfigWindow.ClearContent();
            dcmRawWindow.ClearContent();
            dcmParsingWindow.ClearContent();
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Load Dcm Data";
                dlg.DefaultExt = DocumentExtension;
                dlg.Filter = string.Format("Raw Can Data(*{0})|*{0}|", DocumentExtension) +
                    "All files(*.*)|*.*";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Open(dlg.FileName);
                }
            }
        }

        // true - 表示用户已经进行确认，false, 表示取消
        private bool AskUserSaveChanged()
        {
            bool ret = true;
            if (ContentChanged)
            {
                var result = MessageBox.Show("文件内容已经被修改，是否需要保存变化?", "提醒",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                {
                    ret = false;
                }
                if (result == DialogResult.OK)
                {
                    menuSave.PerformClick();
                }
            }

            if (ret)
            {
                ContentChanged = false;
            }

            return ret;
        }

        private void Open(string fileName)
        {
            if (fileName.Equals(dcmTreeWindow.DocumentFile))
            {
                MessageBox.Show("文档[" + fileName + "]已经被打开", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!AskUserSaveChanged())
            {
                return;
            }

            dcmTreeWindow.LoadDocument(fileName);
            dcmConfigWindow.AddRecentFile(fileName);
            UpdateRecentFilesMenu();
        }

        private void openToolButton_Click(object sender, EventArgs e)
        {
            menuOpen.PerformClick();
        }

        private void saveToolButton_Click(object sender, EventArgs e)
        {
            menuSave.PerformClick();
        }

        private void menuSave_Click(object sender, EventArgs e)
        {
            dcmTreeWindow.Save();
        }

        private void menuSaveAs_Click(object sender, EventArgs e)
        {
            dcmTreeWindow.SaveAs();
        }

        private void newToolButton_Click(object sender, EventArgs e)
        {
            menuNew.PerformClick();
        }

        private void menuNew_Click(object sender, EventArgs e)
        {
            if (!AskUserSaveChanged())
            {
                return;
            }
            dcmTreeWindow.New();
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            var aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        #region Edit Dcm Function

        // 更新菜单状态
        private void menuEdit_DropDownOpening(object sender, EventArgs e)
        {
            dcmTreeWindow.menuEdit_DropDownOpening(sender, e);
        }

        private void menuAddService_Click(object sender, EventArgs e)
        {
            dcmTreeWindow.menuAddService_Click(sender, e);
        }

        private void menuEditService_Click(object sender, EventArgs e)
        {
            dcmTreeWindow.menuEditService_Click(sender, e);
        }

        private void menuRemoveService_Click(object sender, EventArgs e)
        {
            dcmTreeWindow.menuRemoveService_Click(sender, e);
        }

        private void menuAddSubFunction_Click(object sender, EventArgs e)
        {
            dcmTreeWindow.menuAddSubFunction_Click(sender, e);
        }

        private void menuEditSubFunction_Click(object sender, EventArgs e)
        {
            dcmTreeWindow.menuEditSubFunction_Click(sender, e);
        }

        private void menuRemoveSubFunction_Click(object sender, EventArgs e)
        {
            dcmTreeWindow.menuRemoveSubFunction_Click(sender, e);
        }
        #endregion

        private void toolButtonAddService_Click(object sender, EventArgs e)
        {
            menuAddService_Click(sender, e);
        }

        private void toolButtonEditService_Click(object sender, EventArgs e)
        {
            menuEditService_Click(sender, e);
        }

        private void toolButtonRemoveService_Click(object sender, EventArgs e)
        {
            menuRemoveService_Click(sender, e);
        }

        private void toolButtonAddSubFunction_Click(object sender, EventArgs e)
        {
            menuAddSubFunction_Click(sender, e);
        }

        private void toolButtonEditSubFunction_Click(object sender, EventArgs e)
        {
            menuEditSubFunction_Click(sender, e);
        }

        private void toolButtonRemoveSubFunction_Click(object sender, EventArgs e)
        {
            menuRemoveSubFunction_Click(sender, e);
        }

        private void toolButtonCan_Click(object sender, EventArgs e)
        {
            if (DcmService.DcmService.CanIsOpened())
            {
                menuCloseCan_Click(sender, e);
            }
            else
            {
                menuOpenCan_Click(sender, e);
            }
        }

        private void menuSwapPhyFunReq_Click(object sender, EventArgs e)
        {
            menuSwapPhyFunReq.Checked = !menuSwapPhyFunReq.Checked;
            
            // CAN是关闭状态下才允许交换
            if (!DcmService.DcmService.CanIsOpened())
            {
                dcmConfigWindow.SwapPhysicalFunctionRequest(menuSwapPhyFunReq.Checked);
            }
        }

        private void menuOption_DropDownOpening(object sender, EventArgs e)
        {
            menuSwapPhyFunReq.Enabled = !DcmService.DcmService.CanIsOpened();
        }

        private void menuVs2015DarkTheme_Click(object sender, EventArgs e)
        {
            SetTheme<VS2015DarkTheme>(sender, Color.Black);
        }

        private void menuVs2015LightTheme_Click(object sender, EventArgs e)
        {
            SetTheme<VS2015LightTheme>(sender, SystemColors.Control);
        }

        private void menuVsBlueTheme_Click(object sender, EventArgs e)
        {
            SetTheme<VS2015BlueTheme>(sender, Color.FromArgb(255, 41, 57, 85));
        }

        private void InitThemeByThemeType(Type themeType)
        {
            if (themeType == typeof(VS2015BlueTheme))
            {
                dcmParsingWindow.UpdateSplitterColor(Color.FromArgb(255, 41, 57, 85));
                SelectThemeMenu(menuVsBlueTheme);
            }
            else if (themeType == typeof(VS2015DarkTheme))
            {
                dcmParsingWindow.UpdateSplitterColor(Color.Black);
                SelectThemeMenu(menuVs2015DarkTheme);
            }
            else if (themeType == typeof(VS2015LightTheme))
            {
                dcmParsingWindow.UpdateSplitterColor(SystemColors.Control);
                SelectThemeMenu(menuVs2015LightTheme);
            }
        }

        private void SelectThemeMenu(object sender)
        {
            foreach (var menuItem in themeMenuList)
            {
                menuItem.Checked = (sender == menuItem);
            }
        }

        private void SetTheme<T>(object sender, Color splitterColor)
            where T : ThemeBase, new()
        {
            if (!(mainDockPanel.Theme is T))
            {
                mainDockPanel.SaveAsXml(AppDockLayoutFile);
                CloseAllContents();
                CloseAllDocuments();
                mainDockPanel.Theme = new T();
                EnableVSRenderer(VisualStudioToolStripExtender.VsVersion.Vs2015,
                    mainDockPanel.Theme);
                LoadDockLayout();
                dcmParsingWindow.UpdateSplitterColor(splitterColor);
                SelectThemeMenu(sender);
            }
        }

        private void menuConfigVDF_Click(object sender, EventArgs e)
        {
            dcmVdfWindow.Activate();
        }
    }
}

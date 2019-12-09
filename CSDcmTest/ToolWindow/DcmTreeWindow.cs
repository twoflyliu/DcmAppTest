using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSDcmTest.Dialog;
using DcmConfig;
using DcmService;
using SecurityAccessContract;
using Vdf4Cs;

namespace CSDcmTest
{
    public partial class DcmTreeWindow : ToolWindow
    {
        public const string DefaultConfigFile = "Config/DcmTest.cfg";
        private DcmConfig.DcmDocument dcmDocument = null;
        public string DocumentFile { get; set; }

        public const int ImageIndexService = 0;
        public const int ImageIndexSubFunction = 1;

        private MainForm mainForm = null;

        public DcmTreeWindow(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        protected override void OnLoad(EventArgs e)
        {
            //LoadDocument(DefaultConfigFile);
            treeView.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;
            base.OnLoad(e); 
        }

        private void UpdateDcmTree()
        {
            if (dcmDocument == null)
            {
                return;
            }

            treeView.BeginUpdate();
            treeView.Nodes.Clear();

            TreeNode selectedNode = null;

            foreach (Service service in dcmDocument.Services)
            {
                // 构建顶层节点
                TreeNode serviceNode = new TreeNode(service.Name, ImageIndexService
                    , ImageIndexService,
                    new TreeNode[]
                    {
                        new TreeNode("Children")
                    });
                serviceNode.Tag = service;
                treeView.Nodes.Add(serviceNode);

                if (selectedNode == null)
                {
                    selectedNode = serviceNode;
                }
            }

            treeView.EndUpdate();
            //treeView.ExpandAll();

            if (selectedNode != null)
            {
                previousSelectedNode = selectedNode;
                treeView.SelectedNode = selectedNode;
                treeView_Validating(treeView, new CancelEventArgs());
            }
        }

        delegate void OnParsingDataIncommingCallback(ParsingDataIncommingEventArgs e);
        internal void OnParsingDataIncomming(ParsingDataIncommingEventArgs e)
        {
            if (InvokeRequired)
            {
                OnParsingDataIncommingCallback callback = OnParsingDataIncomming;
                Invoke(callback, e);
            }
            else
            {
                if (e.ResponseData == null || e.ResponseData.Count < 2)
                {
                    return;
                }

                // 更新Key
                int sid = e.ResponseData[0];
                const int securitySid = 0x67;
                int securityLevel = e.ResponseData[1];

                if ((sid == securitySid) && (securityLevel % 2 == 1))
                {
                    List<byte> seed = new List<byte>();
                    for (int i = 2; i < e.ResponseData.Count; i++)
                    {
                        seed.Add(e.ResponseData[i]);
                    }
                    UpdateSecurityKey(securityLevel, seed);
                }
            }
        }

        private void UpdateSecurityKey(int securityLevel, List<byte> seed)
        {
            foreach (var service in dcmDocument.Services)
            {
                foreach (var subFunction in service.SubFunctions)
                {
                    if (subFunction.Prefix.Count >= 2
                        && subFunction.Prefix[0] == 0x27
                        && subFunction.Prefix[1] == (securityLevel+1))
                    {
                        var algorithm = GetSecurityAccessAlgorithm();
                        if (algorithm != null)
                        {
                            var key = algorithm.Encrypt(securityLevel, seed);
                            if (key != null)
                            {
                                if (subFunction.Data == null)
                                {
                                    subFunction.Data = new List<byte>();
                                }
                                else
                                {
                                    subFunction.Data.Clear();
                                }
                                subFunction.Data.AddRange(key);
                            }
                        }
                    }
                }
            }
        }

        delegate void onLoadDocumentDoneFn(bool loadStatus, string configFile, string errMsg);
        private void onLoadDocumentDone(bool loadStatus, string configFile, string errMsg)
        {
            if (mainForm.InvokeRequired)
            {
                onLoadDocumentDoneFn fn = onLoadDocumentDone;
                mainForm.Invoke(fn, loadStatus, configFile, errMsg);
            }
            else
            {
                if (loadStatus) //加载成功
                {
                    UpdateDcmTree();
                    mainForm.SetInstStatus("完毕");

                    mainForm.OnLoadDocumentDone(configFile, true);
                }
                else
                {
                    MessageBox.Show(
                                string.Format("Cannot load file {0}: {1}", configFile, errMsg),
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mainForm.OnLoadDocumentDone(configFile, false);
                    New(); //加载失败，则新建一个文档
                }
            }
        }

        public /*async*/ void LoadDocument(string configFile)
        {
            mainForm.SetInstStatus(string.Format("正在加载文档: [{0}]", configFile));

            bool loadStatus = true;
            string errMsg = null;

            // 这儿相对于普通版本是修改过的程序
            // 个人有点不太愿意编写.net 4.5之前的版本
            Task.Factory.StartNew(() =>
            {
                try
                {
                    dcmDocument = DcmConfig.DcmConfig.LoadFile(configFile);
                    mainForm.UpdateDcmDocument(dcmDocument);
                    DocumentFile = configFile;
                }
                catch (DcmConfig.DcmFileFormatException ex)
                {
                    loadStatus = false;
                    errMsg = ex.Message;
                }
                finally
                {
                    onLoadDocumentDone(loadStatus, configFile, errMsg);
                }
            }/*, TaskCreationOptions.LongRunning*/);
        }

        public T SelectedEntity<T>()
            where T: class
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode == null)
            {
                return null;
            }
            return selectedNode.Tag as T;
        }

        internal void OnMessageChanged(VdfMessage message)
        {
            var subFunction = SelectedEntity<SubFunction>();
            if (subFunction == null)
            {
                return;
            }
            if (message == subFunction.VdfMessage)
            {
                mainForm.UpdateSubFunction(subFunction);
            }
        }

        internal void OnVdfSignalChanged(VdfSignal signal)
        {
            var subFunction = SelectedEntity<SubFunction>();
            if (subFunction != null && subFunction.VdfMessage != null)
            {
                bool ok = subFunction.VdfMessage.SignalTable.ContainsValue(signal);
                if (ok)
                {
                    mainForm.UpdateSubFunction(subFunction);
                }
            }
        }

        internal void OnVdfValueDescriptionChanged(VdfValueDesc valueDesc)
        {
            var subFunction = SelectedEntity<SubFunction>();
            if (subFunction != null && subFunction.VdfMessage != null)
            {
                bool ok = false;
                foreach (var signalEntry in subFunction.VdfMessage.SignalTable)
                {
                    if (signalEntry.Value.VdfValueDesc == valueDesc)
                    {
                        ok = true;
                        break;
                    }
                }
                if (ok)
                {
                    mainForm.UpdateSubFunction(subFunction);
                }
            }
        }

        private TreeNode previousSelectedNode = null;

        //Validating失去焦点的时候，会被调用
        private void treeView_Validating(object sender, CancelEventArgs e)
        {
            previousSelectedNode = treeView.SelectedNode;
            if (treeView.SelectedNode != null)
            {
                treeView.SelectedNode.BackColor = SystemColors.Highlight;
                treeView.SelectedNode.ForeColor = Color.White;
            }
        }

        // 相当于选择变化
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is SubFunction)
            {
                //更新Content
                mainForm.UpdateSubFunction((SubFunction)e.Node.Tag);
            }
            else if (e.Node.Tag is Service)
            {
                // 清除内容
                mainForm.UpdateService((Service)e.Node.Tag);
            }

            // 恢复之前节点的颜色设置
            if (previousSelectedNode != null)
            {
                previousSelectedNode.BackColor = treeView.BackColor;
                previousSelectedNode.ForeColor = treeView.ForeColor;
            }
        }

        // 节点双击事件
        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is SubFunction)
            {
                if (!DcmService.DcmService.CanIsOpened())
                {
                    MessageBox.Show("Can设备未打开，请先打开CAN", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SubFunction subFunction = (SubFunction)e.Node.Tag;
                Console.WriteLine("Sub function {0} double clicked.", subFunction.Name);

                switch (subFunction.CanAddressType)
                {
                    case CanAddressType.Functional:
                        DcmService.DcmService.SendFunctionalRequest(mainForm.GetPackageData());
                        break;
                    case CanAddressType.Physical:
                        DcmService.DcmService.SendPhysicalRequest(mainForm.GetPackageData());
                        break;
                    default:
                        throw new InvalidOperationException("Unknown can id type");
                }
            }
        }

        //加载孩子
        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is DcmConfig.Service)
            {
                DcmConfig.Service service = (DcmConfig.Service)e.Node.Tag;

                // 子项未更新的时候，才会执行更新
                if (e.Node.Nodes[0].Tag == null)
                {
                    treeView.BeginUpdate();
                    e.Node.Nodes.Clear();

                    foreach (SubFunction subFunction in service.SubFunctions)
                    {
                        TreeNode subFunctionNode = new TreeNode(string.Format("{0} - {1}",
                            Utils.HexArrayToString(subFunction.Prefix), subFunction.Name), 
                            ImageIndexSubFunction, ImageIndexSubFunction);

                        subFunctionNode.Tag = subFunction;
                        e.Node.Nodes.Add(subFunctionNode);
                    }

                    treeView.EndUpdate();
                }
            }
        }

        ISecurityAccessAlgorithm GetSecurityAccessAlgorithm()
        {
            return mainForm.GetSecurityAccessAlgorithm();
        }

        private void toggleAllToolButton_Click(object sender, EventArgs e)
        {
            if (toggleAllToolButton.Text.Equals("展开所有"))
            {
                treeView.ExpandAll();
                toggleAllToolButton.Text = "收缩所有";
            }
            else
            {
                treeView.CollapseAll();
                toggleAllToolButton.Text = "展开所有";
            }
        }

        internal void Save()
        {
            if (string.IsNullOrEmpty(DocumentFile))
            {
                SaveAs();
            }
            Save(DocumentFile);
        }

        private void Save(string documentFile)
        {
            if (documentFile == null || dcmDocument == null)
            {
                return;
            }
            DcmConfig.DcmConfig.Save(dcmDocument, documentFile);
            mainForm.ContentChanged = false;
            DocumentFile = documentFile;
        }

        internal void SaveAs()
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Load Dcm Data";
                dlg.DefaultExt = MainForm.DocumentExtension;
                dlg.Filter = string.Format("Raw Can Data(*{0})|*{0}|", MainForm.DocumentExtension) +
                    "All files(*.*)|*.*";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Save(dlg.FileName);
                }
            }
        }

        internal void New()
        {
            mainForm.ClearAllToolWindow();
            dcmDocument = new DcmDocument();
            DocumentFile = null;
            mainForm.UpdateDcmDocument(dcmDocument);
            mainForm.OnLoadDocumentDone(null, true);
        }

        internal void ClearContent()
        {
            treeView.Nodes.Clear();
        }

        internal void menuEdit_DropDownOpening(object sender, EventArgs e)
        {
            // 添加服务使能

            // 编辑服务使能
            var node = treeView.SelectedNode;
            mainForm.menuEditService.Enabled = (node != null && node.Tag is Service);

            // 移除服务使能
            mainForm.menuRemoveService.Enabled = (node != null && node.Tag is Service);

            // 添加子功能使能
            mainForm.menuAddSubFunction.Enabled = (node != null && node.Tag is Service);

            // 编辑子功能使能
            mainForm.menuEditSubFunction.Enabled = (node != null && node.Tag is SubFunction);

            // 移除子功能使能
            mainForm.menuRemoveSubFunction.Enabled = (node != null && node.Tag is SubFunction);
        }



        // 添加服务
        internal void menuAddService_Click(object sender, EventArgs e)
        {
            // 弹出对话框，构建服务
            using (ServiceDialog dlg = new ServiceDialog(ServiceDialog.Operation.New))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    var service = new Service();
                    service.Name = dlg.ServiceName;
                    dcmDocument.Services.Add(service);

                    var node = new TreeNode(service.Name, ImageIndexService,
                        ImageIndexService);
                    node.Tag = service;
                    treeView.Nodes.Add(node);
                    treeView.SelectedNode = node; //选择刚添加的节点
                    mainForm.ContentChanged = true;
                }
            }
        }

        internal void menuEditService_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;
            if (node != null)
            {
                if (node.Tag is Service service) //模式匹配
                {
                    using (ServiceDialog dlg = new ServiceDialog(ServiceDialog.Operation.Update))
                    {
                        dlg.ServiceName = service.Name;
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            service.Name = dlg.ServiceName;
                            node.Text = service.Name;
                            mainForm.ContentChanged = true;
                        }
                    }
                }
            }
        }

        internal void menuRemoveService_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;
            if (node != null)
            {
                if (node.Tag is Service service) //模式匹配
                {
                    if (MessageBox.Show("请确认是否需要移除[" + service.Name + "]服务",
                        "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
                        == DialogResult.Yes)
                    {
                        dcmDocument.Services.Remove(service);
                        treeView.Nodes.Remove(node);
                        mainForm.ContentChanged = true;
                    }
                }
            }
        }

        // 添加子功能
        internal void menuAddSubFunction_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;
            if (node != null)
            {
                if (node.Tag is Service service)
                {
                    using (var dlg = new SubFunctionDialog(SubFunctionDialog.Operation.New,
                        dcmDocument.VdfDocument))
                    {
                        var subFunction = new SubFunction();
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            subFunction.Name = dlg.SubFunctionName;
                            subFunction.Prefix = dlg.PrefixData;
                            subFunction.DataLen = dlg.DataLen;
                            subFunction.CanAddressType = dlg.AddressType;
                            subFunction.Message = dlg.Message;

                            subFunction.DataType = DataType.Hex; //这儿属性已经作废了，因为引入Vdf

                            subFunction.Data = Utils.NewInitializedList(subFunction.DataLen,
                                subFunction.Data);
                            

                            mainForm.ContentChanged = true;

                            var subFunctionNode = new TreeNode(string.Format("{0} - {1}",
                                Utils.HexArrayToString(subFunction.Prefix), subFunction.Name),
                                ImageIndexSubFunction, ImageIndexSubFunction);
                            service.SubFunctions.Add(subFunction);

                            subFunctionNode.Tag = subFunction;
                            node.Nodes.Add(subFunctionNode);
                            node.Expand();
                        }
                    }
                }
            }
        }

        internal void menuEditSubFunction_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;
            if (node != null)
            {
                if (node.Tag is SubFunction subFunction)
                {
                    using (var dlg = new SubFunctionDialog(SubFunctionDialog.Operation.Update,
                        dcmDocument.VdfDocument))
                    {
                        dlg.SubFunctionName = subFunction.Name;
                        dlg.PrefixData = subFunction.Prefix;
                        dlg.DataLen = subFunction.DataLen;
                        dlg.AddressType = subFunction.CanAddressType;
                        dlg.Message = subFunction.Message;
                        dlg.ParsingDirection = subFunction.ParsingDirection;

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            subFunction.Name = dlg.SubFunctionName;
                            subFunction.Prefix = dlg.PrefixData;
                            subFunction.DataLen = dlg.DataLen;
                            subFunction.CanAddressType = dlg.AddressType;

                            subFunction.Message = dlg.Message;
                            subFunction.VdfMessage =
                                dcmDocument.VdfDocument.Message(subFunction.Message);

                            subFunction.Data = Utils.NewInitializedList(subFunction.DataLen,
                                subFunction.Data);

                            if (subFunction.ParsingDirection != dlg.ParsingDirection)
                            {
                                subFunction.ParsingDirection = dlg.ParsingDirection;
                                dcmDocument.UpdateReceiveSubFunctionTable();
                            }

                            mainForm.ContentChanged = true;
                            node.Text = string.Format("{0} - {1}",
                                Utils.HexArrayToString(subFunction.Prefix), subFunction.Name);

                            mainForm.UpdateSubFunction(subFunction);
                        }
                    }
                }
            }
        }

        internal void menuRemoveSubFunction_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;
            if (node != null)
            {
                if (node.Tag is SubFunction subFunction)
                {
                    if (MessageBox.Show("请确认是否需要移除[" + subFunction.Name + "]子功能",
                        "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            // 移除subFunction
                            dcmDocument.RemoveSubFunction(subFunction);

                            // 移除Tree
                            node.Remove();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "移除子功能" + subFunction.Name + "失败",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            // 添加服务使能

            // 编辑服务使能
            var node = treeView.SelectedNode;
            contextMenuEditService.Enabled = (node != null && node.Tag is Service);

            // 移除服务使能
            contextMenuRemoveService.Enabled = (node != null && node.Tag is Service);

            // 添加子功能使能
            contextMenuAddSubFun.Enabled = (node != null && node.Tag is Service);

            // 编辑子功能使能
            contextMenuEditSubFun.Enabled = (node != null && node.Tag is SubFunction);

            // 移除子功能使能
            contextMenuRemoveSubFun.Enabled = (node != null && node.Tag is SubFunction);
        }

        private void contextMenuAddService_Click(object sender, EventArgs e)
        {
            menuAddService_Click(sender, e);
        }

        private void contextMenuEditService_Click(object sender, EventArgs e)
        {
            menuEditService_Click(sender, e);
        }

        private void contextMenuRemoveService_Click(object sender, EventArgs e)
        {
            menuRemoveService_Click(sender, e);
        }

        private void contextMenuAddSubFun_Click(object sender, EventArgs e)
        {
            menuAddSubFunction_Click(sender, e);
        }

        private void contextMenuEditSubFun_Click(object sender, EventArgs e)
        {
            menuEditSubFunction_Click(sender, e);
        }

        private void contextMenuRemoveSubFun_Click(object sender, EventArgs e)
        {
            menuRemoveSubFunction_Click(sender, e);
        }
    }
}
 
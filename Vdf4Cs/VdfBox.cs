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
    public partial class VdfBox : UserControl
    {
        private Dictionary<Type, PageBase> pages = new Dictionary<Type, PageBase>();

        public VdfBox()
        {
            InitializeComponent();

            UpdateTreeView();
            this.Load += VdfBox_Load;
        }

        private void VdfBox_Load(object sender, EventArgs e)
        {
            UpdateAllToolButtonEnabled();
        }

        public const int ImageIndexGroup = 0;
        public const int ImageIndexMessage = 1;
        public const int ImageIndexSignal = 2;
        public const int ImageIndexValueDesc = 3;

        private int NewMessageIndex = 0;
        private const string DefaultNewMessageNamePrefix = "Default New Message ";

        public string NewMessageName
        {
            get
            {
                return string.Format("{0} - {1}", DefaultNewMessageNamePrefix,
                    NewMessageIndex++);
            }
        }

        private int NewSignalIndex = 0;
        private const string DefaultNewSignalNamePrefix = "Default New Signal";
        public string NewSignalName
        {
            get
            {
                return string.Format("{0} - {1}", 
                    DefaultNewSignalNamePrefix, NewSignalIndex++);
            }
        }

        private int NewValDescIndex = 0;
        private const string DefaultValDescNamePrefix = "Default New Value Description";
        public string NewValDescName
        {
            get
            {
                return string.Format("{0} - {1}", DefaultValDescNamePrefix,
                    NewValDescIndex++);
            }
        }

        private VdfDocument vdfDocument = null;
        public VdfDocument VdfDocument
        {
            get { return vdfDocument; }
            set
            {
                if (vdfDocument != value)
                {
                    vdfDocument = value;
                    UpdateTreeView();
                }
            }
        }

        private TreeNode msgGrpNode;
        private TreeNode valDescGrpNode;

        private VdfMessage CurrMessage { get; set; }

        delegate void UpdateTreeViewFn();
        public void UpdateTreeView()
        {
            if (InvokeRequired)
            {
                UpdateTreeViewFn fn = UpdateTreeView;
                this.Invoke(fn);
            }
            else
            {
                treeView.Nodes.Clear();

                msgGrpNode = new TreeNode("报文", ImageIndexGroup, ImageIndexGroup);
                treeView.Nodes.Add(msgGrpNode);

                valDescGrpNode = new TreeNode("值描述", ImageIndexGroup, ImageIndexGroup);
                treeView.Nodes.Add(valDescGrpNode);

                if (vdfDocument == null) return;

                msgGrpNode.Tag = vdfDocument.MessageTable;
                valDescGrpNode.Tag = vdfDocument.ValueDescTable;

                // 添加所有报文
                foreach (var entry in vdfDocument.MessageTable)
                {
                    var msgNode = new TreeNode(entry.Key, ImageIndexMessage, ImageIndexMessage);
                    msgGrpNode.Nodes.Add(msgNode);
                    msgNode.Tag = entry.Value;

                    foreach (var sigEntry in entry.Value.SignalTable)
                    {
                        var sigNode = new TreeNode(sigEntry.Key, 
                            ImageIndexSignal, ImageIndexSignal);
                        msgNode.Nodes.Add(sigNode);
                        sigNode.Tag = sigEntry.Value;
                    }
                }

                // 添加所有值描述
                foreach (var entry in vdfDocument.ValueDescTable)
                {
                    var valDescNode = new TreeNode(entry.Key,
                        ImageIndexValueDesc, ImageIndexValueDesc);
                    valDescGrpNode.Nodes.Add(valDescNode);
                    valDescNode.Tag = entry.Value;
                }

                msgGrpNode.Expand();
                valDescGrpNode.Expand();
            }
        }

        public PageBase GetPage(object tag)
        {
            if (tag == null)
            {
                return null;
            }

            if (pages.TryGetValue(tag.GetType(), out PageBase userControl))
            {
                return userControl;
            }
            else
            {
                var type = tag.GetType();
                PageBase page = null;

                Console.WriteLine("Left:" + type.ToString());
                Console.WriteLine("Right:" + typeof(Dictionary<string, VdfSignal>).ToString());

                if (type == typeof(Dictionary<string, VdfMessage>))
                {
                    page = new MessageGroupPage(this);
                }
                else if (type == typeof(Dictionary<string, VdfValueDesc>))
                {
                    page = new ValueDescGroupPage(this);
                }
                else if (type == typeof(VdfMessage))
                {
                    page = new MessagePage(this);
                }
                else if (type == typeof(VdfSignal))
                {
                    page = new SignalPage(this);
                }
                else if (type == typeof(VdfBcdValueDesc))
                {
                    page = new BcdValueDescPage(this);
                }
                else if (type == typeof(VdfXncodeValueDesc))
                {
                    page = new XnCodeValueDescPage(this);
                }
                else if (type == typeof(VdfPhyValueDesc))
                {
                    page = new PhysicalValueDescPage(this);
                }
                else if (type == typeof(VdfAsciiValueDesc))
                {
                    page = new AsciiValueDescPage(this);
                }
                else
                {
                    throw new Exception("不支持的实体类型");
                }

                contentPanel.Controls.Add(page);
                pages.Add(type, page);
                page.Dock = DockStyle.Fill;
                return page;
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectPage(e.Node);
            UpdateCurrMessage();
            UpdateAllToolButtonEnabled();
        }

        private void UpdateCurrMessage()
        {
            object tag = SelectedNodeTag();
            if (tag == null)
            {
                return;
            }

            if (tag is Dictionary<string, VdfMessage>)
            {
                //page = new MessageGroupPage();
            }
            else if (tag is Dictionary<string, VdfValueDesc>)
            {
                //page = new ValueDescGroupPage();
            }
            else if (tag is VdfMessage)
            {
                //page = new MessagePage();
                CurrMessage = tag as VdfMessage;
            }
            else if (tag is VdfSignal)
            {
                //page = new SignalPage();
                CurrMessage = treeView.SelectedNode.Parent.Tag as VdfMessage;
            }
            else if (tag is VdfBcdValueDesc)
            {
                //page = new BcdValueDescPage();
            }
            else if (tag is VdfXncodeValueDesc)
            {
                //page = new XnCodeValueDescPage();
            }
            else if (tag is VdfPhyValueDesc)
            {
                //page = new PhysicalValueDescPage();
            }
            else if (tag is VdfAsciiValueDesc)
            {

            }
            else
            {
                throw new Exception("不支持的实体类型");
            }
        }

        private PageBase prevPage = null;
        private void SelectPage(TreeNode node)
        {
            if (prevPage != null)
            {
                prevPage.Visible = false;
            }
            var page = GetPage(node.Tag);
            if (page != null)
            {
                page.Visible = true;
                page.VdfDocument = VdfDocument;
                //page.VdfBox = this;
                page.Node = node;
                prevPage = page;
            }
        }

        object SelectedNodeTag()
        {
            if (treeView.SelectedNode == null)
            {
                return null;
            }
            else
            {
                return treeView.SelectedNode.Tag;
            }
        }

        #region Help to ensure editable state
        public bool CanAddMessage()
        {
            return true;
        }

        public bool CanRemoveMessage()
        {
            object tag = SelectedNodeTag();
            return tag != null && tag is VdfMessage;
        }

        public bool CanAddSignal()
        {
            object tag = SelectedNodeTag();
            return tag != null && 
                (tag is VdfMessage || tag is VdfSignal);
        }

        public bool CanRemoveSignal()
        {
            object tag = SelectedNodeTag();
            return tag != null && tag is VdfSignal;
        }

        public bool CanAddValDesc()
        {
            return true;
        }

        public bool CanRemoveValDesc()
        {
            object tag = SelectedNodeTag();
            return tag != null && typeof(VdfValueDesc).IsInstanceOfType(tag);
        }
        #endregion

        void UpdateAllToolButtonEnabled()
        {
            //添加/移除报文
            toolButtonAddMsg.Enabled = CanAddMessage();// 添加报文什么时候都可以
            // 移除报文，确保当前节点是报文界面
            toolButtonRemoveMsg.Enabled = CanRemoveMessage();

            //添加信号/移除信号
            toolButtonAddSignal.Enabled = CanAddSignal();
            toolButtonRemoveSignal.Enabled = CanRemoveSignal();

            //添加值描述/移除值描述
            toolButtonAddValDesc.Enabled = CanAddValDesc();
            toolButtonRemoveValDesc.Enabled = CanRemoveValDesc();
        }

        private void TreeViewContextMenu_Opening(object sender, CancelEventArgs e)
        {
            contextMenuAddMessage.Enabled = CanAddMessage();
            contextMenuRemoveMessage.Enabled = CanRemoveMessage();

            contextMenuAddSignal.Enabled = CanAddSignal();
            contextMenuRemoveSignal.Enabled = CanRemoveSignal();

            contextMenuAddValDesc.Enabled = CanAddValDesc();
            contextMenuRemoveValDesc.Enabled = CanRemoveValDesc();
        }

        #region Context Menu
        private void contextMenuAddMessage_Click(object sender, EventArgs e)
        {
            AddMessage();
        }

        private void contextMenuRemoveMessage_Click(object sender, EventArgs e)
        {
            RemoveMessage();
        }

        private void contextMenuAddSignal_Click(object sender, EventArgs e)
        {
            AddSignal();
        }

        private void contextMenuRemoveSignal_Click(object sender, EventArgs e)
        {
            RemoveSignal();
        }

        private void contextMenuRemoveValDesc_Click(object sender, EventArgs e)
        {
            RemoveValDesc();
        }

        private void dropDownMenuAddXncodeValDesc_Click(object sender, EventArgs e)
        {
            AddXncodeValDesc();
        }

        private void dropDownMenuAddPhysicalValDesc_Click(object sender, EventArgs e)
        {
            AddPhysicalValDesc();
        }

        private void dropDownMenuAddBcdValDesc_Click(object sender, EventArgs e)
        {
            AddBcdValDesc();
        }
        #endregion

        #region ToolBar
        private void toolButtonAddMsg_Click(object sender, EventArgs e)
        {
            AddMessage();
        }

        private void toolButtonRemoveMsg_Click(object sender, EventArgs e)
        {
            RemoveMessage();
        }

        private void toolButtonAddSignal_Click(object sender, EventArgs e)
        {
            AddSignal();
        }

        private void toolButtonRemoveSignal_Click(object sender, EventArgs e)
        {
            RemoveSignal();
        }

        private void toolButtonRemoveValDesc_Click(object sender, EventArgs e)
        {
            RemoveValDesc();
        }
        #endregion

        public void AddMessage()
        {
            var name = NewMessageName;
            var msgNode = new TreeNode(name, 
                ImageIndexMessage, ImageIndexMessage);
            msgGrpNode.Nodes.Add(msgNode);

            var msg = new VdfMessage();
            msg.Name = name;
            vdfDocument.MessageTable.Add(name, msg);
            msgNode.Tag = msg;

            treeView.SelectedNode = msgNode;
        }

        public void RemoveMessage()
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null && selectedNode.Tag is VdfMessage message)
            {
                if (MessageBox.Show("请确认是否需要移除报文[" + message.Name + "]?", "移除报文确认",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                try
                {
                    vdfDocument.RemoveMessage(message.Name);
                    msgGrpNode.Nodes.Remove(treeView.SelectedNode);
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, "移除报文" + message.Name + "失败",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void AddSignal()
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode == null)
            {
                return;
            }

            TreeNode currMsgNode = null;
            if (selectedNode.Tag is VdfMessage message)
            {
                currMsgNode = selectedNode;
            }
            else if (selectedNode.Tag is VdfSignal)
            {
                currMsgNode = selectedNode.Parent;
            }

            //if (selectedNode != null && selectedNode.Tag is VdfMessage message)
            if (currMsgNode != null)
            {
                var name = NewSignalName;
                var sigNode = new TreeNode(name, ImageIndexSignal, ImageIndexSignal);

                var signal = new VdfSignal();
                signal.Name = name;
                sigNode.Tag = signal;

                currMsgNode.Nodes.Add(sigNode);
                treeView.SelectedNode = sigNode;
            }
        }

        public void RemoveSignal()
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null && selectedNode.Tag is VdfSignal signal)
            {
                if (MessageBox.Show("请确认是否需要移除信号[" + signal.Name + "]?", "移除信号确认",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                if (CurrMessage != null)
                {
                    selectedNode.Parent.Nodes.Remove(selectedNode);
                    signal.VdfValueDesc = null; //非常主要，取消引用
                    CurrMessage.SignalTable.Remove(signal.Name);
                }
            }
        }

        public void RemoveValDesc()
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null && selectedNode.Tag is VdfValueDesc valDesc)
            {
                if (MessageBox.Show("请确认是否需要移除值描述[" + valDesc.Name + "]?", "移除值描述确认",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                try
                {
                    vdfDocument.RemoveValueDescription(valDesc.Name);
                    selectedNode.Parent.Nodes.Remove(selectedNode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Remove Value Description " + valDesc.Name + " Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }

        private void AddValDesc<T>()
            where T: VdfValueDesc, new()
        {
            var name = NewValDescName;

            var valDescNode = new TreeNode(name,
                    ImageIndexValueDesc, ImageIndexValueDesc);
            valDescGrpNode.Nodes.Add(valDescNode);

            var valDesc = new T();
            valDesc.Name = name;
            vdfDocument.ValueDescTable.Add(name, valDesc);

            valDescNode.Tag = valDesc;
            treeView.SelectedNode = valDescNode;
        }

        private void AddXncodeValDesc()
        {
            AddValDesc<VdfXncodeValueDesc>();
        }

        private void AddPhysicalValDesc()
        {
            AddValDesc<VdfPhyValueDesc>();
        }

        private void AddBcdValDesc()
        {
            AddValDesc<VdfBcdValueDesc>();
        }

        public event ValueDescriptionChangedEventHandler OnValueDescriptionChanged;
        public void FireValueDescriptionChangedEvent(VdfValueDesc valDesc)
        {
            ValueDescriptionChangedEventArgs e = new ValueDescriptionChangedEventArgs();
            e.ValueDesc = valDesc;
            OnValueDescriptionChanged?.Invoke(e);
        }

        public event MessageChangedEventHandler OnMessageChanged;
        public void FireMessageChangedEvent(VdfMessage message)
        {
            MessageChangedEventArgs e = new MessageChangedEventArgs();
            e.Message = message;
            OnMessageChanged?.Invoke(e);
        }

        public event SignalChangedEventHandler OnSignalChanged;
        public void FireSignalChangedEvent(VdfSignal signal)
        {
            SignalChangedEventArgs e = new SignalChangedEventArgs();
            e.Signal = signal;
            OnSignalChanged?.Invoke(e);
        }

        private void dropDownMenuAddAsciiValDesc_Click(object sender, EventArgs e)
        {
            AddValDesc<VdfAsciiValueDesc>();
        }
    }
}

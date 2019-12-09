using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vdf4Cs
{
    public class PageBase : UserControl
    {
        public PageBase(VdfBox vdfBox)
        {
            VdfBox = vdfBox;
        }

        public PageBase()
        {

        }

        private TreeNode node;
        public TreeNode Node
        {
            get { return node; }
            set
            {
                //if (node != value)
                {
                    node = value;
                    Debug.Assert(node.Tag != null, "节点没有附加任何数据");
                    UpdateUI();
                }
            }
        }


        private VdfDocument vdfDocument;
        public VdfDocument VdfDocument
        {
            get { return vdfDocument; }
            set
            {
                if (vdfDocument != value)
                {
                    vdfDocument = value;
                    OnVdfDocumentChanged(vdfDocument);
                }
            }
        }

        public VdfBox VdfBox { get; set; }

        public string OldName { get; set; }

        public T Entity<T>()
            where T: class
        {
            if (Node != null && Node.Tag is T entity)
            {
                return entity;
            }
            return null;
        }

        protected void FireValueDescChangedEvent(VdfValueDesc valDesc)
        {
            if (VdfBox != null)
            {
                VdfBox.FireValueDescriptionChangedEvent(valDesc);
            }
        }

        protected void FireMessageChangedEvent(VdfMessage message)
        {
            if (VdfBox != null)
            {
                VdfBox.FireMessageChangedEvent(message);
            }
        }

        protected void FireSignalChangedEvent(VdfSignal signal)
        {
            if (VdfBox != null)
            {
                VdfBox.FireSignalChangedEvent(signal);
            }
        }

        protected virtual void UpdateUI()
        {

        }

        protected virtual void OnVdfDocumentChanged(VdfDocument doc)
        {

        }

        protected bool TextBoxNameValidating<T>(TextBox textBoxName,
            CancelEventArgs e, Action<T> OnSuccess = null)
            where T: class
        {
            if (!NameNotEmptyValidate(textBoxName.Text, e)) //校验数据
            {
                textBoxName.Text = OldName;
                textBoxName.Focus();
                return false;
            }
            else //数据没有问题，则更新内容和显示
            {
                var parent = Node.Parent;
                if (parent != null)
                {
                    if (parent.Tag is Dictionary<string, T> tbl)
                    {
                        try
                        {
                            if (!OldName.Equals(textBoxName.Text))
                            {
                                tbl.Remove(OldName);
                                tbl.Add(textBoxName.Text, Node.Tag as T);
                                OldName = textBoxName.Text;
                            }
                        }
                        catch (Exception)
                        {
                            string prefix = (typeof(VdfMessage) == typeof(T)) ? "报文" : "值描述";
                            MessageBox.Show(prefix + "[" + textBoxName.Text + "]已经存在", "错误",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);

                            tbl.Add(OldName, Node.Tag as T);
                            textBoxName.Text = OldName;
                            textBoxName.Focus();
                        }
                    }
                    else if (parent.Tag is VdfMessage message)
                    {
                        var sigTbl = message.SignalTable;
                        try
                        {
                            sigTbl.Remove(OldName);
                            sigTbl.Add(textBoxName.Text, Node.Tag as VdfSignal);
                            OldName = textBoxName.Text;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("信号名称" + textBoxName.Text + "已经存在",
                                "修改信号名称出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            sigTbl.Add(OldName, Node.Tag as VdfSignal); //把现场还原回去
                            textBoxName.Text = OldName;
                            textBoxName.Focus();
                            return false;
                        }
                    }
                    else
                    {
                        throw new Exception("不支持此中类型的实体");
                    }
                    if (Node.Tag is T ent && ent != null)
                    {
                        OnSuccess?.Invoke(ent);
                    }
                    return true;
                }
                return false;
            }
        }

        protected bool TextBoxNameValidating<T>(TextBox textBox, string name,
            CancelEventArgs e, ref int oldValue, Action<T> OnSuccess = null)
            where T: class
        {
            if (!UnsignedIntValidate(textBox.Text, name, e))
            {
                textBox.Text = oldValue.ToString();
                textBox.Focus();
                return false;
            }
            else
            {
                if (Node != null && Node.Tag is T ent)
                {
                    OnSuccess.Invoke(ent);
                    return true;
                }
                return false;
            }
        }

        protected bool TextBoxDoubleValidating<T>(TextBox textBox, string name,
            CancelEventArgs e, ref double oldValue, Action<T> OnSuccess = null)
        {
            if (!DoubleValidate(textBox.Text, name, e))
            {
                textBox.Text = oldValue.ToString();
                textBox.Focus();
                return false;
            }
            else
            {
                if (Node != null && Node.Tag is T ent)
                {
                    OnSuccess.Invoke(ent);
                    return true;
                }
                return false;
            }
        }

        protected bool TextBoxUnsignedIntValidating<T>(TextBox textBox, string name,
            CancelEventArgs e, ref int oldValue, Action<T> OnSuccess = null)
        {
            if (!UnsignedIntValidate(textBox.Text, name, e))
            {
                textBox.Text = oldValue.ToString();
                textBox.Focus();
                return false;
            }
            else
            {
                if (Node != null && Node.Tag is T ent)
                {
                    OnSuccess.Invoke(ent);
                    return true;
                }
                return false;
            }
        }

        protected bool TextBoxStringNotEmptyValidating<T>(TextBox textBox, string name,
            CancelEventArgs e, ref string oldValue, Action<T> OnSuccess = null)
        {
            if (!StringNotEmptyValidate(textBox.Text, name, e))
            {
                textBox.Text = oldValue.ToString();
                textBox.Focus();
                return false;
            }
            else
            {
                if (Node != null && Node.Tag is T ent)
                {
                    OnSuccess.Invoke(ent);
                    return true;
                }
                return false;
            }
        }

        protected bool StringNotEmptyValidate(string input, string name, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(input))
            {
                MessageBox.Show(name + "不能为空", "校验失败", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                e.Cancel = false;
                return false;
            }
            return true;
        }

        protected bool UnsignedIntValidate(string input)
        {
            Regex regex = new Regex(@"^\d+$");
            return (regex.IsMatch(input));
        }

        protected bool UnsignedIntValidate(string input, string name, CancelEventArgs e)
        {
            Regex regex = new Regex(@"^\d+$");
            if (!regex.IsMatch(input))
            {
                MessageBox.Show(name + "只有包含有效的无符号整数0-9", "校验失败", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = false;
                return false;
            }
            return true;
        }

        protected bool DoubleValidate(string input, string name, CancelEventArgs e)
        {
            Regex regex = new Regex(@"^([+-]?\d*.\d+)|([+-]?\d+.?\d*)$");
            if (!regex.IsMatch(input))
            {
                MessageBox.Show(name + "只有包含有效的小数", "校验失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = false;
                return false;
            }
            return true;
        }

        protected bool NameNotEmptyValidate(string input, CancelEventArgs e)
        {
            return StringNotEmptyValidate(input, "名称", e);
        }

        protected bool DescriptionNotEmptyValidate(string input, CancelEventArgs e)
        {
            return StringNotEmptyValidate(input, "描述", e);
        }
    }
}

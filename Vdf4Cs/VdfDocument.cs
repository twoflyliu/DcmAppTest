using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdf4Cs
{
    public class VdfDocument
    {
        private string vdfFile = null;
        private bool Loaded { get; set; }

        public const string Version = "1.0";

        public VdfDocument()
        {
            ValueDescTable = new Dictionary<string, VdfValueDesc>();
            MessageTable = new Dictionary<string, VdfMessage>();
            vdfFile = string.Empty;
        }

        public Dictionary<string, VdfValueDesc> ValueDescTable { get; set; }
        public Dictionary<string, VdfMessage> MessageTable { get; set; }

        public VdfMessage Message(string msgName)
        {
            // 下面这种写法叫做内联变量
            if (msgName != null && MessageTable.TryGetValue(msgName, out VdfMessage msg))
            {
                return msg;
            }
            return null;
        }

        public void RemoveValueDescription(string name)
        {
            VdfValueDesc valDesc;
            if (ValueDescTable.TryGetValue(name, out valDesc))
            {
                if (valDesc.Owners.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Value Description {0} has following owners:\n", name);
                    foreach (var obj in valDesc.Owners)
                    {
                        sb.Append('\t');
                        sb.AppendLine(obj.ToString());
                    }
                    throw new VdfException(sb.ToString());
                }
                else
                {
                    ValueDescTable.Remove(name);
                }
            }
            else
            {
                throw new VdfException("Named " + name + " Value Description Does Not Exist!");
            }
        }

        public void RemoveMessage(string name)
        {
            VdfMessage message;
            if (MessageTable.TryGetValue(name, out message))
            {
                if (message.Owners.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("Message {0} has following owners:\n", name);
                    foreach (var obj in message.Owners)
                    {
                        sb.Append('\t');
                        sb.AppendLine(obj.ToString());
                    }
                    throw new VdfException(sb.ToString());
                }
                else
                {
                    foreach (var sigEntry in message.SignalTable)
                    {
                        sigEntry.Value.VdfValueDesc = null;
                    }
                    MessageTable.Remove(name);
                }
            }
            else
            {
                throw new VdfException("Named " + name + " Message Does Not Exist!");
            }
        }

        public void RemoveValueDescription(VdfValueDesc valDesc)
        {
            RemoveValueDescription(valDesc.Name);
        }

        public VdfValueDesc ValueDesc(string vdName)
        {
            if (ValueDescTable.TryGetValue(vdName, out VdfValueDesc desc))
            {
                return desc;
            }
            return null;
        }

        public void Load()
        {
            Load(vdfFile);
        }

        public void Load(string file)
        {
            if (!Loaded || !vdfFile.Equals(file))
            {
                if (string.IsNullOrEmpty(file))
                {
                    throw new VdfException("Vdf file name cannot be null or empty.");
                }

                vdfFile = file;
                try
                {
                    VdfDocumentLoader.Load(vdfFile, this);
                    Loaded = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void Save()
        {
            Save(vdfFile);
        }

        public void Save(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return;
            }

            //if (!Loaded)
            //{
            //    throw new VdfException(string.Format("Vdf File {0} is unloaded", file));
            //}
            VdfDocumentSaver.Save(file, this);
        }
    }
}

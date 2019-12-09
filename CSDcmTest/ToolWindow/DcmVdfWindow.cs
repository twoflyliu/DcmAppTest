using DcmConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vdf4Cs;

namespace CSDcmTest
{
    public partial class DcmVdfWindow : ToolWindow
    {
        private VdfBox vdfBox = null;

        public VdfBox VdfBox
        {
            get { return vdfBox; }
        }


        public DcmVdfWindow()
        {
            InitializeComponent();

            vdfBox = new VdfBox();
            vdfBox.Dock = DockStyle.Fill;

            Controls.Add(vdfBox);
        }

        public ToolStrip ToolBar
        {
            get
            {
                return vdfBox.ToolBar;
            }
        }

        public ContextMenuStrip TreeViewContextMenu
        {
            get
            {
                return vdfBox.TreeViewContextMenu;
            }
        }

        private DcmDocument dcmDocument;
        public DcmDocument DcmDocument
        {
            get { return dcmDocument; }
            set
            {
                if (dcmDocument != value)
                {
                    dcmDocument = value;
                    vdfBox.VdfDocument = dcmDocument.VdfDocument;
                }
            }
        }
    }
}

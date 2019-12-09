using DcmConfig;
using DcmService;
using SecurityAccessContract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace CSDcmTest
{
    public partial class DcmConfigWindow : ToolWindow
    {
        private DcmDocument dcmDocument;
        public DcmDocument DcmDocument
        {
            get
            {
                return dcmDocument;
            }

            set
            {
                if (value != dcmDocument)
                {
                    dcmDocument = value;
                    var dcmCanCfg = new DcmCanConfig();
                    var dcmDocCfg = dcmDocument.Config;

                    if (SwapPhyFunReq)
                    {
                        dcmCanCfg.CanPhysicalRequestId = dcmDocCfg.FunctionRequestId;
                        dcmCanCfg.CanFunctionRequestId = dcmDocCfg.PhysicalRequestId;
                    }
                    else
                    {
                        dcmCanCfg.CanPhysicalRequestId = dcmDocCfg.PhysicalRequestId;
                        dcmCanCfg.CanFunctionRequestId = dcmDocCfg.FunctionRequestId;
                    }

                    dcmCanCfg.CanResponseId = dcmDocCfg.ResponseId;
                    dcmCanCfg.CanTickEnabled = dcmDocCfg.CanTickEnabled;
                    dcmCanCfg.CanTickPeriod = (uint)dcmDocCfg.CanTickPeriod;
                    dcmCanCfg.SuppressResponse = dcmDocCfg.SuppressTickResponse;
                    dcmCanCfg.SecurityAccessType = dcmDocCfg.SecurityAccessType;

                    FullRefreshPropertyGrid(dcmCanCfg);
                }
            }
        }

        // 存放最近打开列表
        public List<string> RecentFiles { get; set; }

        // 交换物理/功能请求
        public bool SwapPhyFunReq { get; set; }

        delegate void FullRefreshPropertyGridCallback(DcmCanConfig dcmCanConfig);
        void FullRefreshPropertyGrid(DcmCanConfig dcmCanConfig)
        {
            if (propertyGrid.InvokeRequired)
            {
                FullRefreshPropertyGridCallback callback = FullRefreshPropertyGrid;
                propertyGrid.Invoke(callback, dcmCanConfig);
            }
            else
            {
                propertyGrid.SelectedObject = dcmCanConfig;
                UpdateCanConfig();
            }
        }

        private MainForm mainForm;
        public DcmConfigWindow(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            RecentFiles = new List<string>();
        }

        public const int MaxRecentFileCount = 10;
        // 添加最近打开
        public void AddRecentFile(string file)
        {
            int i;
            for (i = 0; i < RecentFiles.Count; i++)
            {
                if (RecentFiles[i].Equals(file))
                {
                    break;
                }
            }
            if (i != RecentFiles.Count)
            {
                RecentFiles.RemoveAt(i);
            }
            RecentFiles.Insert(0, file);

            if (RecentFiles.Count > 10)
            {
                RecentFiles.RemoveAt(RecentFiles.Count - 1);
            }
        }

        public const char RecentFileCountSplitChar = ';';
        protected override string GetPersistString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetType().ToString());
            if (RecentFiles.Count > 0)
            {
                foreach (var recentFile in RecentFiles)
                {
                    sb.Append(RecentFileCountSplitChar);
                    sb.Append(recentFile);
                }
            }
            return sb.ToString();
        }

        private void UpdateCanConfig()
        {
            DcmCanConfig config = propertyGrid.SelectedObject as DcmCanConfig;
            DcmService.DcmService.PhysicalRequestId = config.CanPhysicalRequestId;
            DcmService.DcmService.FunctionRequestId = config.CanFunctionRequestId;
            DcmService.DcmService.ResponseId = config.CanResponseId;
            DcmService.DcmService.CanTickEnabled = config.CanTickEnabled;
            DcmService.DcmService.CanTickPeriod = config.CanTickPeriod;
            DcmService.DcmService.SuppressResponse = config.SuppressResponse;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            DcmCanConfig config = propertyGrid.SelectedObject as DcmCanConfig;
            Debug.Assert(dcmDocument != null);
            switch (e.ChangedItem.Label)
            {
                case "CanPhysicalRequestId":
                    DcmService.DcmService.PhysicalRequestId = config.CanPhysicalRequestId;
                    dcmDocument.Config.PhysicalRequestId = config.CanPhysicalRequestId;
                    mainForm.ContentChanged = true;
                    break;
                case "CanFunctionRequestId":
                    DcmService.DcmService.FunctionRequestId = config.CanFunctionRequestId;
                    dcmDocument.Config.FunctionRequestId = config.CanFunctionRequestId;
                    mainForm.ContentChanged = true;
                    break;
                case "CanResponseId":
                    DcmService.DcmService.ResponseId = config.CanResponseId;
                    dcmDocument.Config.ResponseId = config.CanResponseId;
                    mainForm.ContentChanged = true;
                    break;
                case "CanTickPeriod":
                    DcmService.DcmService.CanTickPeriod = config.CanTickPeriod;
                    dcmDocument.Config.CanTickPeriod = (int)config.CanTickPeriod;
                    mainForm.ContentChanged = true;
                    break;
                case "CanTickEnabled":
                    DcmService.DcmService.CanTickEnabled = config.CanTickEnabled;
                    dcmDocument.Config.CanTickEnabled = config.CanTickEnabled;
                    mainForm.ContentChanged = true;
                    break;
                case "SuppressResponse":
                    DcmService.DcmService.SuppressResponse = config.SuppressResponse;
                    dcmDocument.Config.SuppressTickResponse = config.SuppressResponse;
                    mainForm.ContentChanged = true;
                    break;
                case "SecurityAccessType":
                    // 不需要通知Dcm
                    dcmDocument.Config.SecurityAccessType = config.SecurityAccessType;
                    mainForm.ContentChanged = true;
                    break;
                default:
                    break;
            }
        }

        internal void OnSessionChanged(SessionChangedEventArgs e)
        {
            DcmCanConfig config = propertyGrid.SelectedObject as DcmCanConfig;
            if (config != null)
            {
                bool newEnabled = !e.DefaultSession;

                if (config.CanTickEnabled != newEnabled)
                {
                    config.CanTickEnabled = newEnabled;
                    RefreshPropertyGrid(config);
                }
            }
        }

        delegate void RefreshPropertyGridCallback(DcmCanConfig config);
        void RefreshPropertyGrid(DcmCanConfig config)
        {
            RefreshPropertyGridCallback callback = RefreshPropertyGrid;
            if (InvokeRequired)
            {
                Invoke(callback, config);
            }
            else
            {
                propertyGrid.SelectedObject = null;
                propertyGrid.SelectedObject = config;

                DcmService.DcmService.CanTickEnabled = config.CanTickEnabled;
            }
        }

        public ISecurityAccessAlgorithm GetSecurityAccessAlgorithm()
        {
            DcmCanConfig config = propertyGrid.SelectedObject as DcmCanConfig;
            return SecurityAccessAlgorithManager.Instance().
                GetSecurityAccessAlgorithm(config.SecurityAccessType);
        }

        internal void ClearContent()
        {
            propertyGrid.SelectedObject = null;
        }

        internal void SwapPhysicalFunctionRequest(bool swap)
        {
            if (propertyGrid.SelectedObject is DcmCanConfig config)
            {
                var temp = config.CanPhysicalRequestId;
                config.CanPhysicalRequestId = config.CanFunctionRequestId;
                config.CanFunctionRequestId = temp;

                DcmService.DcmService.FunctionRequestId = config.CanFunctionRequestId;
                DcmService.DcmService.PhysicalRequestId = config.CanPhysicalRequestId;

                RefreshPropertyGrid(config);
                SwapPhyFunReq = swap;
            }
        }
    }
}

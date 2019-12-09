using SecurityAccessContract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSDcmTest
{
    public class SecurityAccessAlgorithManager
    {
        [ImportMany]
        public IEnumerable<ISecurityAccessAlgorithm> SecurityAccessAlgorithmList { get; set; }

        private Dictionary<string, ISecurityAccessAlgorithm> SecurityAccessAlgorithm;

        public void LoadAllAddins()
        {
            string addins = Properties.Settings.Default.AddInDirectory;
            
            var catalog = new DirectoryCatalog(Path.Combine(System.Environment.CurrentDirectory, addins));
            var container = new CompositionContainer(catalog);
            try
            {
                container.ComposeParts(this);

                SecurityAccessAlgorithm = new Dictionary<string, ISecurityAccessAlgorithm>();
                foreach (var algorithm in SecurityAccessAlgorithmList)
                {
                    SecurityAccessAlgorithm.Add(algorithm.Name, algorithm);
                }
            }
            catch (ChangeRejectedException ex)
            {
                MessageBox.Show("加载安全算法插件失败: " + ex.Message, "错误", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public ICollection<string> GetSecurityAccessAlgorithNames()
        {
            if (SecurityAccessAlgorithm == null)
            {
                LoadAllAddins();
            }
            return SecurityAccessAlgorithm.Keys;
        }

        public ISecurityAccessAlgorithm GetSecurityAccessAlgorithm(string name)
        {
            ISecurityAccessAlgorithm result;
            if (SecurityAccessAlgorithm == null)
            {
                LoadAllAddins();
            }
            if (SecurityAccessAlgorithm.TryGetValue(name, out result))
            {
                return result;
            }
            return null;
        }

        #region Singleton implement
        private static SecurityAccessAlgorithManager instance;
        private static object syncRoot = new object();

        public static SecurityAccessAlgorithManager Instance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new SecurityAccessAlgorithManager();
                    }
                }
            }
            return instance;
        }

        private SecurityAccessAlgorithManager() { }
        #endregion

    }
}

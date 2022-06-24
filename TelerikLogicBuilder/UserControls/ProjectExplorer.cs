using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ProjectExplorer : UserControl
    {
        //controls
        private readonly ConfigurationExplorer _configurationExplorer;
        private readonly DocumentsExplorer _documentsExplorer;
        private readonly RulesExplorer _rulesExplorer;
        public ProjectExplorer(ConfigurationExplorer configurationExplorer, DocumentsExplorer documentsExplorer, RulesExplorer rulesExplorer)
        {
            _configurationExplorer = configurationExplorer;
            _documentsExplorer = documentsExplorer;
            _rulesExplorer = rulesExplorer;
            InitializeComponent();
            Initialize();
        }

        public void CreateProfile()
        {
            _documentsExplorer.CreateProfile();
        }

        private void Initialize()
        {
            this.radPageView1.SuspendLayout();
            this.radPageViewDocuments.SuspendLayout();
            this.radPageViewRules.SuspendLayout();
            this.radPageViewConfiguration.SuspendLayout();

            _documentsExplorer.Dock = DockStyle.Fill;
            _rulesExplorer.Dock = DockStyle.Fill;
            _configurationExplorer.Dock = DockStyle.Fill;
            this.radPageViewDocuments.Controls.Add(_documentsExplorer);
            this.radPageViewRules.Controls.Add(_rulesExplorer);
            this.radPageViewConfiguration.Controls.Add(_configurationExplorer);

            this.radPageViewDocuments.ResumeLayout(false);
            this.radPageViewRules.ResumeLayout(false);
            this.radPageViewConfiguration.ResumeLayout(false);
            this.radPageView1.ResumeLayout(false);

            this.radPageView1.SelectedPage = this.radPageViewDocuments;
        }
    }
}

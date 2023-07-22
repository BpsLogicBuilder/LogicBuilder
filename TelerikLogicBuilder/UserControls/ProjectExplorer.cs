using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Factories;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ProjectExplorer : UserControl, IProjectExplorer
    {
        private readonly IMainWindow _mainWindow;

        //controls
        private readonly IConfigurationExplorer _configurationExplorer;
        private readonly DocumentsExplorer _documentsExplorer;
        private readonly RulesExplorer _rulesExplorer;
        public ProjectExplorer(IMainWindow mainWindow, IUserControlFactory userControlFactory)
        {
            _mainWindow = mainWindow;
            _configurationExplorer = userControlFactory.GetConfigurationExplorer();
            _documentsExplorer = userControlFactory.GetDocumentsExplorer();
            _rulesExplorer = userControlFactory.GetRulesExplorer();
            InitializeComponent();
            Initialize();
            InitializeEventHandlers();
        }

        bool IProjectExplorer.Visible
        {
            set
            {
                ChangeVisibility(value);
            }
        }

        public IConfigurationExplorer ConfigurationExplorer => _configurationExplorer;

        public IDocumentsExplorer DocumentsExplorer => _documentsExplorer;

        public IRulesExplorer RulesExplorer => _rulesExplorer;

        private void ChangeVisibility(bool isVisible)
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            mdiParent.SplitPanelExplorer.Collapsed = !isVisible;
            base.Visible = isVisible;
        }

        public void ClearProfile()
        {
            _configurationExplorer.ClearProfile();
            _documentsExplorer.ClearProfile();
            _rulesExplorer.ClearProfile();
            this.radPageView1.SelectedPage = this.radPageViewDocuments;
        }

        public void CreateProfile()
        {
            _configurationExplorer.CreateProfile();
            _documentsExplorer.CreateProfile();
            _rulesExplorer.CreateProfile();
        }

        private void Initialize()
        {
            this.radPageView1.SuspendLayout();
            this.radPageViewDocuments.SuspendLayout();
            this.radPageViewRules.SuspendLayout();
            this.radPageViewConfiguration.SuspendLayout();

            ((Telerik.WinControls.UI.RadOfficeNavigationBarElement)this.radPageView1.ViewElement).OverflowItem.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;

            _documentsExplorer.Dock = DockStyle.Fill;
            _rulesExplorer.Dock = DockStyle.Fill;
            _configurationExplorer.Dock = DockStyle.Fill;
            this.radPageViewDocuments.Controls.Add(_documentsExplorer);
            this.radPageViewRules.Controls.Add(_rulesExplorer);
            this.radPageViewConfiguration.Controls.Add((Control)_configurationExplorer);

            this.radPageViewDocuments.ResumeLayout(false);
            this.radPageViewRules.ResumeLayout(false);
            this.radPageViewConfiguration.ResumeLayout(false);
            this.radPageView1.ResumeLayout(false);

            this.radPageView1.SelectedPage = this.radPageViewDocuments;
        }

        private void InitializeEventHandlers()
        {
            this.titleBar1.CloseClick += TitleBar1_CloseClick;
        }

        #region Event Handlers
        private void TitleBar1_CloseClick()
        {
            ChangeVisibility(false);
        }
#       endregion Event Handlers
    }
}

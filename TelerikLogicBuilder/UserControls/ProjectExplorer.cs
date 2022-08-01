using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ProjectExplorer : UserControl, IProjectExplorer
    {
        private readonly IMainWindow _mainWindow;

        //controls
        private readonly ConfigurationExplorer _configurationExplorer;
        private readonly DocumentsExplorer _documentsExplorer;
        private readonly RulesExplorer _rulesExplorer;
        public ProjectExplorer(IMainWindow mainWindow, ConfigurationExplorer configurationExplorer, DocumentsExplorer documentsExplorer, RulesExplorer rulesExplorer)
        {
            _mainWindow = mainWindow;
            _configurationExplorer = configurationExplorer;
            _documentsExplorer = documentsExplorer;
            _rulesExplorer = rulesExplorer;
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
            this.radPageViewConfiguration.Controls.Add(_configurationExplorer);

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

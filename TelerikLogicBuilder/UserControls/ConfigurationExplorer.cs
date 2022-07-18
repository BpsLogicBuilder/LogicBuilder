using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class ConfigurationExplorer : UserControl
    {
        private readonly IConfigurationExplorerTreeViewBuilder _configurationExplorerTreeViewBuilder;

        public ConfigurationExplorer(
            IConfigurationExplorerTreeViewBuilder configurationExplorerTreeViewBuilder)
        {
            _configurationExplorerTreeViewBuilder = configurationExplorerTreeViewBuilder;

            InitializeComponent();
            Initialize();
        }

        public void ClearProfile()
        {
            radTreeView1.Nodes.Clear();
        }

        public void CreateProfile()
        {
            BuildTreeView();
        }

        private void BuildTreeView()
            => _configurationExplorerTreeViewBuilder.Build(radTreeView1);

        private void Initialize()
        {
        }

        #region Event Handlers
        #endregion Event Handlers
    }
}

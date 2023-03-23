using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules
{
    internal partial class ConfigureExcludedModulesForm : RadForm, IConfigureExcludedModulesForm
    {
        private readonly IConfigureExcludedModulesCommandFactory _configureExcludedModulesCommandFactory;
        private readonly IExcludedModulesTreeViewBuilder _excludedModulesTreeViewBuilder;
        private readonly IFormInitializer _formInitializer;
        private readonly IGetAllCheckedNodes _getAllCheckedNodes;
        private readonly ITreeViewService _treeViewService;

        public ConfigureExcludedModulesForm(
            IConfigureExcludedModulesCommandFactory configureExcludedModulesCommandFactory,
            IExcludedModulesTreeViewBuilder excludedModulesTreeViewBuilder,
            IFormInitializer formInitializer,
            IGetAllCheckedNodes getAllCheckedNodes,
            ITreeViewService treeViewService,
            IList<string> excludedModules)
        {
            InitializeComponent();
            _configureExcludedModulesCommandFactory = configureExcludedModulesCommandFactory;
            _excludedModulesTreeViewBuilder = excludedModulesTreeViewBuilder;
            _formInitializer = formInitializer;
            _getAllCheckedNodes = getAllCheckedNodes;
            _treeViewService = treeViewService;
            existingExcludedModules = excludedModules;

            Initialize();
        }

        private readonly IList<string> existingExcludedModules;

        public IList<string> ExcludedModules => _getAllCheckedNodes
                    .GetNodes(radTreeView1.Nodes[0])
                    .Select(n => n.Text)
                    .OrderBy(n => n)
                    .ToArray();

        public RadListControl ListControl => radListControl1;

        public RadTreeView TreeView => radTreeView1;

        public RadButton OkButton => btnOk;

        private void BuildTreeView()
        {
            _excludedModulesTreeViewBuilder.Build(radTreeView1, existingExcludedModules);
        }

        private void Initialize()
        {
            ControlsLayoutUtility.LayoutGroupBox(this, radGroupBoxMain);
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons);
            _formInitializer.SetFormDefaults(this, 685);
            _formInitializer.SetToConfigSize(this);
            InitializeNodeCheckedChangedCommand
            (
                radTreeView1,
                _configureExcludedModulesCommandFactory.GetUpdateExcludedModulesCommand(this)
            );
            radTreeView1.NodeExpandedChanged += RadTreeView1_NodeExpandedChanged;

            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            BuildTreeView();
            btnOk.Enabled = false;
        }

        private static void InitializeNodeCheckedChangedCommand(RadTreeView treeView, IClickCommand command)
        {
            treeView.NodeCheckedChanged += (sender, args) => command.Execute();
        }

        #region Event Handlers
        private void RadTreeView1_NodeExpandedChanged(object sender, RadTreeViewEventArgs e)
        {
            if (_treeViewService.IsRootNode(e.Node)
                || _treeViewService.IsFileNode(e.Node))/*NodeExpandedChanged runs for file nodes on double click*/
                return;

            e.Node.ImageIndex = e.Node.Expanded
                ? ImageIndexes.OPENEDFOLDERIMAGEINDEX
                : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
        }
        #endregion Event Handlers
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Forms
{
    internal partial class ConfigureExcludedModules : RadForm, IConfigureExcludedModules
    {
        private readonly IConfigureExcludedModulesCommandFactory _configureExcludedModulesCommandFactory;
        private readonly IExcludedModulesTreeViewBuilder _excludedModulesTreeViewBuilder;
        private readonly IFormInitializer _formInitializer;
        private readonly ITreeViewService _treeViewService;

        public ConfigureExcludedModules(
            IConfigureExcludedModulesCommandFactory configureExcludedModulesCommandFactory,
            IExcludedModulesTreeViewBuilder excludedModulesTreeViewBuilder,
            IFormInitializer formInitializer,
            ITreeViewService treeViewService,
            IList<string> excludedModules)
        {
            InitializeComponent();
            _configureExcludedModulesCommandFactory = configureExcludedModulesCommandFactory;
            _excludedModulesTreeViewBuilder = excludedModulesTreeViewBuilder;
            _formInitializer = formInitializer;
            _treeViewService = treeViewService;
            ExcludedModules = new List<string>(excludedModules);

            Initialize();
        }

        public List<string> ExcludedModules { get; }

        public RadListControl ListControl => radListControl1;

        public RadTreeView TreeView => radTreeView1;

        public RadButton OkButton => btnOk;

        private void BuildTreeView()
        {
            _excludedModulesTreeViewBuilder.Build(radTreeView1, ExcludedModules);
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 685);
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

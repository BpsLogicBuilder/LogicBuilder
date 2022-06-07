using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal partial class SelectRulesResourcesPairForm : Telerik.WinControls.UI.RadForm
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFormInitializer _formInitializer;
        private readonly IGetAllCheckedNodes _getAllCheckedNodeNames;
        private readonly ISelectModulesForDeploymentTreeViewBuilder _selectModulesForDeploymentTreeViewBuilder;

        public SelectRulesResourcesPairForm(IConfigurationService configurationService, IFormInitializer formInitializer, IGetAllCheckedNodes getAllCheckedNodeNames, ISelectModulesForDeploymentTreeViewBuilder selectModulesForDeploymentTreeViewBuilder)
        {
            _configurationService = configurationService;
            _formInitializer = formInitializer;
            _getAllCheckedNodeNames = getAllCheckedNodeNames;
            _selectModulesForDeploymentTreeViewBuilder = selectModulesForDeploymentTreeViewBuilder;
            InitializeComponent();
            Initialize();
        }

        internal IList<RulesResourcesPair> SourceFiles => _getAllCheckedNodeNames.GetNodes(radTreeView.Nodes[0])
                                                                .Select(n => (RulesResourcesPair)n.Tag)
                                                                .ToArray();

        internal void SetTitle(string title)
        {
            this.Text = title;
            this.radGroupBoxTop.Text = title;
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 648);
            _selectModulesForDeploymentTreeViewBuilder.Build(radTreeView, _configurationService.GetSelectedApplication().Name);

            radButtonOk.DialogResult = DialogResult.OK;
            radButtonCancel.DialogResult = DialogResult.Cancel;
            radButtonOk.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radButtonCancel.Anchor = AnchorConstants.AnchorsLeftTopRight;
        }
    }
}

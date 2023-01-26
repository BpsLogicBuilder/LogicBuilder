using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class ConfigureVariablesCutCommand : ClickCommandBase
    {
        private readonly IConfigureVariablesCutImageHelper _configureVariablesCutImageHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesCutCommand(
            IConfigureVariablesCutImageHelper configureVariablesCutImageHelper,
            ITreeViewService treeViewService,
            IConfigureVariablesForm configureVariablesForm)
        {
            _configureVariablesCutImageHelper = configureVariablesCutImageHelper;
            _treeViewService = treeViewService;
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureVariablesForm.TreeView);
            if (selectedNodes.Count == 0
                || configureVariablesForm.TreeView.Nodes[0].Selected)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                throw new ArgumentException($"{nameof(selectedNodes)}: {{B4E1253B-2DE5-4E0F-B000-B0AB40E7F253}}");

            foreach (RadTreeNode node in configureVariablesForm.CutTreeNodes)
                _configureVariablesCutImageHelper.SetNormalImage(node);

            configureVariablesForm.CutTreeNodes.Clear();
            foreach (RadTreeNode node in selectedNodes)
            {
                configureVariablesForm.CutTreeNodes.Add(node);
                _configureVariablesCutImageHelper.SetCutImage(node);
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsCutCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionsCutImageHelper _configureFunctionsCutImageHelper;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsCutCommand(
            IConfigureFunctionsCutImageHelper configureFunctionsCutImageHelper,
            ITreeViewService treeViewService,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _configureFunctionsCutImageHelper = configureFunctionsCutImageHelper;
            _treeViewService = treeViewService;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public override void Execute()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureFunctionsForm.TreeView);
            if (selectedNodes.Count == 0
                || configureFunctionsForm.TreeView.Nodes[0].Selected)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                throw new ArgumentException($"{nameof(selectedNodes)}: {{5EEB3A2D-A242-49F1-88DC-1E546817F23C}}");

            foreach (RadTreeNode node in configureFunctionsForm.CutTreeNodes)
                _configureFunctionsCutImageHelper.SetNormalImage(node);

            configureFunctionsForm.CutTreeNodes.Clear();
            foreach (RadTreeNode node in selectedNodes)
            {
                configureFunctionsForm.CutTreeNodes.Add(node);
                _configureFunctionsCutImageHelper.SetCutImage(node);
            }
        }
    }
}

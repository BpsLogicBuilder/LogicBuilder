using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsCutCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsCutImageHelper _configureConstructorsCutImageHelper;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsCutCommand(
            IConfigureConstructorsCutImageHelper configureConstructorsCutImageHelper,
            ITreeViewService treeViewService, 
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _configureConstructorsCutImageHelper = configureConstructorsCutImageHelper;
            _treeViewService = treeViewService;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public override void Execute()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureConstructorsForm.TreeView);
            if (selectedNodes.Count == 0
                || configureConstructorsForm.TreeView.Nodes[0].Selected)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                throw new ArgumentException($"{nameof(selectedNodes)}: {{4499527F-BB0A-4D43-87BC-6A1C125DB52A}}");

            foreach (RadTreeNode node in configureConstructorsForm.CutTreeNodes)
                _configureConstructorsCutImageHelper.SetNormalImage(node);

            configureConstructorsForm.CutTreeNodes.Clear();
            foreach (RadTreeNode node in selectedNodes)
            {
                configureConstructorsForm.CutTreeNodes.Add(node);
                _configureConstructorsCutImageHelper.SetCutImage(node);
            }
        }
    }
}

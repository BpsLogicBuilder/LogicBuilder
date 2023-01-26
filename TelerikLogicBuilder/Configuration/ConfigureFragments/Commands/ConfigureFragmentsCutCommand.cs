using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands
{
    internal class ConfigureFragmentsCutCommand : ClickCommandBase
    {
        private readonly IConfigureFragmentsCutImageHelper _configureFragmentsCutImageHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsCutCommand(
            IConfigureFragmentsCutImageHelper configureFragmentsCutImageHelper,
            ITreeViewService treeViewService,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _configureFragmentsCutImageHelper = configureFragmentsCutImageHelper;
            _treeViewService = treeViewService;
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public override void Execute()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureFragmentsForm.TreeView);
            if (selectedNodes.Count == 0
                || configureFragmentsForm.TreeView.Nodes[0].Selected)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                throw new ArgumentException($"{nameof(selectedNodes)}: {{89A833FC-84AC-4B59-B9A7-CED6EE5CB0D7}}");

            foreach (RadTreeNode node in configureFragmentsForm.CutTreeNodes)
                _configureFragmentsCutImageHelper.SetNormalImage(node);

            configureFragmentsForm.CutTreeNodes.Clear();
            foreach (RadTreeNode node in selectedNodes)
            {
                configureFragmentsForm.CutTreeNodes.Add(node);
                _configureFragmentsCutImageHelper.SetCutImage(node);
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class ConfigureVariablesPasteCommand : ClickCommandBase
    {
        private readonly IConfigureVariablesXmlTreeViewSynchronizer _configureVariablesXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesPasteCommand(
            ITreeViewService treeViewService,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureVariablesForm configureVariablesForm)
        {
            _configureVariablesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureVariablesXmlTreeViewSynchronizer
            (
                configureVariablesForm
            );
            _treeViewService = treeViewService;
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureVariablesForm.TreeView);
            if (selectedNodes.Count != 1)
                return;

            if (configureVariablesForm.CutTreeNodes.Count == 0)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(configureVariablesForm.CutTreeNodes))
                throw new ArgumentException($"{nameof(configureVariablesForm.CutTreeNodes)}: {{0CE9A9B1-F71D-446C-8D8A-29567F0D975F}}");

            RadTreeNode destinationNode = _treeViewService.IsVariableTypeNode(selectedNodes[0])
                                            ? selectedNodes[0].Parent
                                            : selectedNodes[0];

            try
            {
                _configureVariablesXmlTreeViewSynchronizer.MoveFoldersAndVariables
                (
                    (StateImageRadTreeNode)destinationNode,
                    configureVariablesForm.CutTreeNodes.Cast<StateImageRadTreeNode>().ToArray()
                );
            }
            catch (LogicBuilderException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

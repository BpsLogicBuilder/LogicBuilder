using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal class ConfigureVariablesDragDropHandler : IConfigureVariablesDragDropHandler
    {
        private readonly IConfigureVariablesXmlTreeViewSynchronizer _configureVariablesXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesDragDropHandler(
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

        public void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes)
        {
            if (draggingTreeNodes.Count == 0)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(draggingTreeNodes))
                throw new ArgumentException($"{nameof(draggingTreeNodes)}: {{C45085F5-9509-4BA2-B39C-FFF633B301FC}}");

            if (_treeViewService.IsVariableTypeNode(destinationNode))
                destinationNode = destinationNode.Parent;

            try
            {
                _configureVariablesXmlTreeViewSynchronizer.MoveFoldersAndVariables
                (
                    (StateImageRadTreeNode)destinationNode,
                    draggingTreeNodes.Cast<StateImageRadTreeNode>().ToArray()
                );
            }
            catch (LogicBuilderException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers
{
    internal class ConfigureConstructorsDragDropHandler : IConfigureConstructorsDragDropHandler
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsDragDropHandler(
            ITreeViewService treeViewService,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _configureConstructorsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureConstructorsXmlTreeViewSynchronizer
            (
                configureConstructorsForm
            );
            _treeViewService = treeViewService;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes)
        {
            if (draggingTreeNodes.Count == 0)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(draggingTreeNodes))
                throw new ArgumentException($"{nameof(draggingTreeNodes)}: {{19A5F096-0C4F-4C02-B399-8DAF07D314E3}}");

            if (_treeViewService.IsConstructorNode(destinationNode) && draggingTreeNodes.Any(n => !_treeViewService.IsParameterNode(n)))
                destinationNode = destinationNode.Parent;
            else if (_treeViewService.IsParameterNode(destinationNode) && draggingTreeNodes.Any(n => !_treeViewService.IsParameterNode(n)))
                destinationNode = destinationNode.Parent.Parent;

            try
            {
                _configureConstructorsXmlTreeViewSynchronizer.MoveFoldersConstructorsAndParameters
                (
                    (StateImageRadTreeNode)destinationNode,
                    draggingTreeNodes.Cast<StateImageRadTreeNode>().ToArray()
                );
            }
            catch (LogicBuilderException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers
{
    internal class ConfigureFunctionsDragDropHandler : IConfigureFunctionsDragDropHandler
    {
        private readonly IConfigureFunctionsXmlTreeViewSynchronizer _configureFunctionsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsDragDropHandler(
            ITreeViewService treeViewService,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _configureFunctionsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFunctionsXmlTreeViewSynchronizer
            (
                configureFunctionsForm
            );
            _treeViewService = treeViewService;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes)
        {
            if (draggingTreeNodes.Count == 0)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(draggingTreeNodes))
                throw new ArgumentException($"{nameof(draggingTreeNodes)}: {{C3FC2E5D-11EF-40DB-A1C1-3B3E0AB7276D}}");

            if (_treeViewService.IsMethodNode(destinationNode) && draggingTreeNodes.Any(n => !_treeViewService.IsParameterNode(n)))
                destinationNode = destinationNode.Parent;
            else if (_treeViewService.IsParameterNode(destinationNode) && draggingTreeNodes.Any(n => !_treeViewService.IsParameterNode(n)))
                destinationNode = destinationNode.Parent.Parent;
            try
            {
                _configureFunctionsXmlTreeViewSynchronizer.MoveFoldersFunctionsAndParameters
                (
                    (StateImageRadTreeNode)destinationNode,
                    draggingTreeNodes.Cast<StateImageRadTreeNode>().ToArray()
                );
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

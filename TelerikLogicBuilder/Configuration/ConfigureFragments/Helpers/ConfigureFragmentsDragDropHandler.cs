using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers
{
    internal class ConfigureFragmentsDragDropHandler : IConfigureFragmentsDragDropHandler
    {
        private readonly IConfigureFragmentsXmlTreeViewSynchronizer _configureFragmentsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsDragDropHandler(
            ITreeViewService treeViewService,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _configureFragmentsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFragmentsXmlTreeViewSynchronizer
            (
                configureFragmentsForm
            );
            _treeViewService = treeViewService;
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public void DragDrop(RadTreeNode destinationNode, IList<RadTreeNode> draggingTreeNodes)
        {
            if (draggingTreeNodes.Count == 0)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(draggingTreeNodes))
                throw new ArgumentException($"{nameof(draggingTreeNodes)}: {{456504A4-330C-4563-A731-70EB6638544E}}");

            if (_treeViewService.IsFileNode(destinationNode))
                destinationNode = destinationNode.Parent;

            try
            {
                _configureFragmentsXmlTreeViewSynchronizer.MoveFoldersAndFragments
                (
                    destinationNode,
                    draggingTreeNodes
                );
            }
            catch (LogicBuilderException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

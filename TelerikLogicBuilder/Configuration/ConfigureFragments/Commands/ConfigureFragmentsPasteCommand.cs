using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands
{
    internal class ConfigureFragmentsPasteCommand : ClickCommandBase
    {
        private readonly IConfigureFragmentsCutImageHelper _configureFragmentsCutImageHelper;
        private readonly IConfigureFragmentsXmlTreeViewSynchronizer _configureFragmentsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;
        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsPasteCommand(
            IConfigureFragmentsCutImageHelper configureFragmentsCutImageHelper,
            ITreeViewService treeViewService,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _configureFragmentsCutImageHelper = configureFragmentsCutImageHelper;
            _configureFragmentsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFragmentsXmlTreeViewSynchronizer
            (
                configureFragmentsForm
            );
            _treeViewService = treeViewService;
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public override void Execute()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureFragmentsForm.TreeView);
            if (selectedNodes.Count != 1)
                return;

            if (configureFragmentsForm.CutTreeNodes.Count == 0)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(configureFragmentsForm.CutTreeNodes))
                throw new ArgumentException($"{nameof(configureFragmentsForm.CutTreeNodes)}: {{0CE9A9B1-F71D-446C-8D8A-29567F0D975F}}");

            RadTreeNode destinationNode = _treeViewService.IsFileNode(selectedNodes[0])
                                            ? selectedNodes[0].Parent
                                            : selectedNodes[0];

            try
            {
                _configureFragmentsXmlTreeViewSynchronizer.MoveFoldersAndFragments
                (
                    destinationNode,
                    configureFragmentsForm.CutTreeNodes
                );
            }
            catch (LogicBuilderException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                foreach (RadTreeNode node in configureFragmentsForm.CutTreeNodes)
                    _configureFragmentsCutImageHelper.SetNormalImage(node);
            }
        }
    }
}

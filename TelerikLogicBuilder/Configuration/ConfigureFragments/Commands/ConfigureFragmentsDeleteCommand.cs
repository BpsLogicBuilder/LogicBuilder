using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands
{
    internal class ConfigureFragmentsDeleteCommand : ClickCommandBase
    {
        private readonly IConfigureFragmentsXmlTreeViewSynchronizer _configureFragmentsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;
        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsDeleteCommand(
            ITreeViewService treeViewService,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _treeViewService = treeViewService;
            _configureFragmentsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFragmentsXmlTreeViewSynchronizer
            (
                configureFragmentsForm
            );
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public override void Execute()
        {
            try
            {
                IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureFragmentsForm.TreeView);
                if (selectedNodes.Count == 0
                    || configureFragmentsForm.TreeView.Nodes[0].Selected)
                    return;

                if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                    throw new ArgumentException($"{nameof(selectedNodes)}: {{8CE9A4D5-FBDE-4A28-8043-C518506A7E71}}");

                DialogResult dialogResult = DisplayMessage.ShowQuestion
                (
                    (Control)configureFragmentsForm,
                    Strings.deleteSelectedItems,
                    ((Control)configureFragmentsForm).RightToLeft,
                    RadMessageIcon.Exclamation
                );

                if (dialogResult != DialogResult.OK)
                    return;

                foreach (RadTreeNode selectedNode in selectedNodes)
                    _configureFragmentsXmlTreeViewSynchronizer.DeleteNode(selectedNode);

                configureFragmentsForm.CheckEnableImportButton();
            }
            catch (LogicBuilderException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

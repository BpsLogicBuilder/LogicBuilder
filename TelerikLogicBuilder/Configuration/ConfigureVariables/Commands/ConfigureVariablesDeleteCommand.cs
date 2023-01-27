using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class ConfigureVariablesDeleteCommand : ClickCommandBase
    {
        private readonly IConfigureVariablesXmlTreeViewSynchronizer _configureVariablesXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesDeleteCommand(
            ITreeViewService treeViewService,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureVariablesForm configureVariablesForm)
        {
            _treeViewService = treeViewService;
            _configureVariablesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureVariablesXmlTreeViewSynchronizer
            (
                configureVariablesForm
            );
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            try
            {
                configureVariablesForm.TreeView.BeginUpdate();
                Delete();
            }
            catch (LogicBuilderException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                configureVariablesForm.TreeView.EndUpdate();
            }
        }

        private void Delete()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureVariablesForm.TreeView);
            if (selectedNodes.Count == 0
                || configureVariablesForm.TreeView.Nodes[0].Selected)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                throw new ArgumentException($"{nameof(selectedNodes)}: {{DB1748D2-3EFD-4CA2-9205-DAF077A3C1B7}}");

            DialogResult dialogResult = DisplayMessage.ShowQuestion
            (
                (Control)configureVariablesForm,
                Strings.deleteSelectedItems,
                ((Control)configureVariablesForm).RightToLeft,
                RadMessageIcon.Exclamation
            );

            if (dialogResult != DialogResult.OK)
                return;

            foreach (RadTreeNode selectedNode in selectedNodes)
                _configureVariablesXmlTreeViewSynchronizer.DeleteNode((StateImageRadTreeNode)selectedNode);

            configureVariablesForm.CheckEnableImportButton();
        }
    }
}

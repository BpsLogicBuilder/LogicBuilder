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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsDeleteCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionsXmlTreeViewSynchronizer _configureFunctionsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsDeleteCommand(
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

        public override void Execute()
        {
            try
            {
                configureFunctionsForm.TreeView.BeginUpdate();
                Delete();
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                configureFunctionsForm.TreeView.EndUpdate();
            }
        }

        private void Delete()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureFunctionsForm.TreeView);
            if (selectedNodes.Count == 0
                || configureFunctionsForm.TreeView.Nodes[0].Selected)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                throw new ArgumentException($"{nameof(selectedNodes)}: {{FCAF3516-839A-41D6-80AB-511C3A3B7DAE}}");

            DialogResult dialogResult = DisplayMessage.ShowQuestion
            (
                (Control)configureFunctionsForm,
                Strings.deleteSelectedItems,
                ((Control)configureFunctionsForm).RightToLeft,
                RadMessageIcon.Exclamation
            );

            if (dialogResult != DialogResult.OK)
                return;

            foreach (RadTreeNode selectedNode in selectedNodes)
            {
                if (_treeViewService.IsFolderNode(selectedNode) || _treeViewService.IsMethodNode(selectedNode))
                    _configureFunctionsXmlTreeViewSynchronizer.DeleteNode((StateImageRadTreeNode)selectedNode);
                else if (_treeViewService.IsParameterNode(selectedNode))
                    _configureFunctionsXmlTreeViewSynchronizer.DeleteParameterNode((StateImageRadTreeNode)selectedNode);
            }

            configureFunctionsForm.CheckEnableImportButton();
        }
    }
}

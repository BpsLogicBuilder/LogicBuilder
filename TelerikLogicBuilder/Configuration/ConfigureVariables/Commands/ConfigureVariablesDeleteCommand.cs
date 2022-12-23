using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Globalization;
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
                if (configureVariablesForm.TreeView.SelectedNode == null)
                    return;

                RadTreeNode selectedNode = configureVariablesForm.TreeView.SelectedNode;

                if (_treeViewService.IsRootNode(selectedNode))
                    return;
                else if (_treeViewService.IsVariableTypeNode(selectedNode))
                    DeleteVariable(selectedNode);
                else if (_treeViewService.IsFolderNode(selectedNode))
                    DeleteFolder(selectedNode);
            }
            catch (LogicBuilderException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
        }

        private void DeleteVariable(RadTreeNode selectedNode)
        {
            DeleteItem
            (
                selectedNode, 
                string.Format(CultureInfo.CurrentCulture, Strings.deleteVariableQuestionFormat, selectedNode.Text)
            );
        }

        private void DeleteFolder(RadTreeNode selectedNode)
        {
            DeleteItem
            (
                selectedNode,
                string.Format(CultureInfo.CurrentCulture, Strings.deleteFolderQuestion, selectedNode.Text)
            );
        }

        private void DeleteItem(RadTreeNode selectedNode, string deleteMessage)
        {
            DialogResult dialogResult = DisplayMessage.ShowQuestion
            (
                (Control)configureVariablesForm,
                deleteMessage,
                ((Control)configureVariablesForm).RightToLeft,
                RadMessageIcon.Exclamation
            );

            if (dialogResult != DialogResult.OK)
                return;

            _configureVariablesXmlTreeViewSynchronizer.DeleteNode((StateImageRadTreeNode)selectedNode);
            configureVariablesForm.CheckEnableImportButton();
        }
    }
}

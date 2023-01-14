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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsDeleteCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsDeleteCommand(
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

        public override void Execute()
        {
            try
            {
                IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureConstructorsForm.TreeView);
                if (selectedNodes.Count == 0
                    || configureConstructorsForm.TreeView.Nodes[0].Selected)
                    return;

                if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                    throw new ArgumentException($"{nameof(selectedNodes)}: {{428BE87F-4A55-4C7F-822F-4233941FD88A}}");

                DialogResult dialogResult = DisplayMessage.ShowQuestion
                (
                    (Control)configureConstructorsForm,
                    Strings.deleteSelectedItems,
                    ((Control)configureConstructorsForm).RightToLeft,
                    RadMessageIcon.Exclamation
                );

                if (dialogResult != DialogResult.OK)
                    return;

                foreach (RadTreeNode selectedNode in selectedNodes)
                {
                    if (_treeViewService.IsFolderNode(selectedNode) || _treeViewService.IsConstructorNode(selectedNode))
                        _configureConstructorsXmlTreeViewSynchronizer.DeleteNode((StateImageRadTreeNode)selectedNode);
                    else if (_treeViewService.IsParameterNode(selectedNode))
                        _configureConstructorsXmlTreeViewSynchronizer.DeleteParameterNode((StateImageRadTreeNode)selectedNode);
                }

                configureConstructorsForm.CheckEnableImportButton();
            }
            catch (LogicBuilderException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

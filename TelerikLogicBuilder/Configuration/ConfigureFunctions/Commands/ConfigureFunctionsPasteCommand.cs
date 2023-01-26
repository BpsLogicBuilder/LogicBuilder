using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsPasteCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionsCutImageHelper _configureFunctionsCutImageHelper;
        private readonly IConfigureFunctionsXmlTreeViewSynchronizer _configureFunctionsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsPasteCommand(
            IConfigureFunctionsCutImageHelper configureFunctionsCutImageHelper,
            ITreeViewService treeViewService,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _configureFunctionsCutImageHelper = configureFunctionsCutImageHelper;
            _configureFunctionsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFunctionsXmlTreeViewSynchronizer
            (
                configureFunctionsForm
            );
            _treeViewService = treeViewService;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public override void Execute()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureFunctionsForm.TreeView);
            if (selectedNodes.Count != 1)
                return;

            if (configureFunctionsForm.CutTreeNodes.Count == 0)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(configureFunctionsForm.CutTreeNodes))
                throw new ArgumentException($"{nameof(configureFunctionsForm.CutTreeNodes)}: {{042B4D21-5E79-40E6-9AEF-5A9596C0018D}}");

            RadTreeNode destinationNode = selectedNodes[0];

            if (_treeViewService.IsMethodNode(destinationNode) && configureFunctionsForm.CutTreeNodes.Any(n => !_treeViewService.IsParameterNode(n)))
                destinationNode = destinationNode.Parent;
            else if (_treeViewService.IsParameterNode(destinationNode) && configureFunctionsForm.CutTreeNodes.Any(n => !_treeViewService.IsParameterNode(n)))
                destinationNode = destinationNode.Parent.Parent;

            try
            {
                _configureFunctionsXmlTreeViewSynchronizer.MoveFoldersFunctionsAndParameters
                (
                    (StateImageRadTreeNode)destinationNode,
                    configureFunctionsForm.CutTreeNodes.Cast<StateImageRadTreeNode>().ToArray()
                );
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                foreach (RadTreeNode node in configureFunctionsForm.CutTreeNodes)
                    _configureFunctionsCutImageHelper.SetNormalImage(node);
            }
        }
    }
}

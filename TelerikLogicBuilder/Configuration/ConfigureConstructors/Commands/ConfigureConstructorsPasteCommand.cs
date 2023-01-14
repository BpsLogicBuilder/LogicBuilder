using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsPasteCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsPasteCommand(
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
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureConstructorsForm.TreeView);
            if (selectedNodes.Count != 1)
                return;

            if (configureConstructorsForm.CutTreeNodes.Count == 0)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(configureConstructorsForm.CutTreeNodes))
                throw new ArgumentException($"{nameof(configureConstructorsForm.CutTreeNodes)}: {{086397BC-3D74-4AFB-AA4F-FCD71CEFF202}}");

            RadTreeNode destinationNode = selectedNodes[0];

            if (_treeViewService.IsConstructorNode(destinationNode) && configureConstructorsForm.CutTreeNodes.Any(n => !_treeViewService.IsParameterNode(n)))
                destinationNode = destinationNode.Parent;
            else if (_treeViewService.IsParameterNode(destinationNode) && configureConstructorsForm.CutTreeNodes.Any(n => !_treeViewService.IsParameterNode(n)))
                destinationNode = destinationNode.Parent.Parent;

            try
            {
                _configureConstructorsXmlTreeViewSynchronizer.MoveFoldersConstructorsAndParameters
                (
                    (StateImageRadTreeNode)destinationNode,
                    configureConstructorsForm.CutTreeNodes.Cast<StateImageRadTreeNode>().ToArray()
                );
            }
            catch (LogicBuilderException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

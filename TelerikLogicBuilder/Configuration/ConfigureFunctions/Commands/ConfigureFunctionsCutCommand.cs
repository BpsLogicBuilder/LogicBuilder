using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsCutCommand : ClickCommandBase
    {
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsCutCommand(
            ITreeViewService treeViewService,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _treeViewService = treeViewService;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public override void Execute()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureFunctionsForm.TreeView);
            if (selectedNodes.Count == 0
                || configureFunctionsForm.TreeView.Nodes[0].Selected)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                throw new ArgumentException($"{nameof(selectedNodes)}: {{5EEB3A2D-A242-49F1-88DC-1E546817F23C}}");

            foreach (RadTreeNode node in configureFunctionsForm.CutTreeNodes)
                SetNormalImage(node);

            configureFunctionsForm.CutTreeNodes.Clear();
            foreach (RadTreeNode node in selectedNodes)
            {
                configureFunctionsForm.CutTreeNodes.Add(node);
                SetCutImage(node);
            }
        }

        private void SetCutImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX;
            }
            if (_treeViewService.IsMethodNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTMETHODIMAGEINDEX;
            }
            else if (_treeViewService.IsLiteralTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTLITERALPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsObjectTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTOBJECTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsGenericTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTGENERICPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfLiteralsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTLITERALLISTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfObjectsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTOBJECTLISTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfGenericsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTGENERICLISTPARAMETERIMAGEINDEX;
            }
        }

        private void SetNormalImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
            }
            if (_treeViewService.IsMethodNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.METHODIMAGEINDEX;
            }
            else if (_treeViewService.IsLiteralTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsObjectTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.OBJECTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsGenericTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.GENERICPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfLiteralsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfObjectsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfGenericsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.GENERICLISTPARAMETERIMAGEINDEX;
            }
        }
    }
}

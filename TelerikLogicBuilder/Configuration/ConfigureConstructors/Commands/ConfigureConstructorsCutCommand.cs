using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsCutCommand : ClickCommandBase
    {
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsCutCommand(
            ITreeViewService treeViewService, 
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _treeViewService = treeViewService;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public override void Execute()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(configureConstructorsForm.TreeView);
            if (selectedNodes.Count == 0
                || configureConstructorsForm.TreeView.Nodes[0].Selected)
                return;

            if (_treeViewService.CollectionIncludesNodeAndDescendant(selectedNodes))
                throw new ArgumentException($"{nameof(selectedNodes)}: {{4499527F-BB0A-4D43-87BC-6A1C125DB52A}}");

            foreach (RadTreeNode node in configureConstructorsForm.CutTreeNodes)
                SetNormalImage(node);

            configureConstructorsForm.CutTreeNodes.Clear();
            foreach (RadTreeNode node in selectedNodes)
            {
                configureConstructorsForm.CutTreeNodes.Add(node);
                SetCutImage(node);
            }
        }

        private void SetCutImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX;
            }
            if (_treeViewService.IsConstructorNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTCONSTRUCTORIMAGEINDEX;
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
            if (_treeViewService.IsConstructorNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CONSTRUCTORIMAGEINDEX;
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

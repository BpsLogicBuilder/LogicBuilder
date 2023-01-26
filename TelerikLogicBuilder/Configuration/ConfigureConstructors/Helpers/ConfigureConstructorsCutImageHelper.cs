using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers
{
    internal class ConfigureConstructorsCutImageHelper : IConfigureConstructorsCutImageHelper
    {
        private readonly ITreeViewService _treeViewService;

        public ConfigureConstructorsCutImageHelper(ITreeViewService treeViewService)
        {
            _treeViewService = treeViewService;
        }

        public void SetCutImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = cutTreeNode.Expanded ? ImageIndexes.CUTOPENEDFOLDERIMAGEINDEX : ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX;
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

        public void SetNormalImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = cutTreeNode.Expanded ? ImageIndexes.OPENEDFOLDERIMAGEINDEX : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
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

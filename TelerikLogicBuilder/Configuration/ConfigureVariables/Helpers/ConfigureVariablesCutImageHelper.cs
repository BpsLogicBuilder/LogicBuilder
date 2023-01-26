using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal class ConfigureVariablesCutImageHelper : IConfigureVariablesCutImageHelper
    {
        private readonly ITreeViewService _treeViewService;

        public ConfigureVariablesCutImageHelper(ITreeViewService treeViewService)
        {
            _treeViewService = treeViewService;
        }

        public void SetCutImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = cutTreeNode.Expanded ? ImageIndexes.CUTOPENEDFOLDERIMAGEINDEX : ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX;
            }
            else if (_treeViewService.IsLiteralTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTLITERALPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsObjectTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTOBJECTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfLiteralsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTLITERALLISTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfObjectsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTOBJECTLISTPARAMETERIMAGEINDEX;
            }
        }

        public void SetNormalImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = cutTreeNode.Expanded ? ImageIndexes.OPENEDFOLDERIMAGEINDEX : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
            }
            else if (_treeViewService.IsLiteralTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.LITERALPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsObjectTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.OBJECTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfLiteralsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.LITERALLISTPARAMETERIMAGEINDEX;
            }
            else if (_treeViewService.IsListOfObjectsTypeNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.OBJECTLISTPARAMETERIMAGEINDEX;
            }
        }
    }
}

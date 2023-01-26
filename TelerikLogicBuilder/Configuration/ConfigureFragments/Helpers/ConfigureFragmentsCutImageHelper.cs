using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers
{
    internal class ConfigureFragmentsCutImageHelper : IConfigureFragmentsCutImageHelper
    {
        private readonly ITreeViewService _treeViewService;

        public ConfigureFragmentsCutImageHelper(ITreeViewService treeViewService)
        {
            _treeViewService = treeViewService;
        }

        public void SetCutImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = cutTreeNode.Expanded ? ImageIndexes.CUTOPENEDFOLDERIMAGEINDEX : ImageIndexes.CUTCLOSEDFOLDERIMAGEINDEX;
            }
            else if (_treeViewService.IsFileNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.CUTFILEIMAGEINDEX;
            }
        }

        public void SetNormalImage(RadTreeNode cutTreeNode)
        {
            if (_treeViewService.IsFolderNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = cutTreeNode.Expanded ? ImageIndexes.OPENEDFOLDERIMAGEINDEX : ImageIndexes.CLOSEDFOLDERIMAGEINDEX;
            }
            else if (_treeViewService.IsFileNode(cutTreeNode))
            {
                cutTreeNode.ImageIndex = ImageIndexes.FILEIMAGEINDEX;
            }
        }
    }
}

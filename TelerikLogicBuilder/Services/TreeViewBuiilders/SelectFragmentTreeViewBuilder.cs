using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class SelectFragmentTreeViewBuilder : ISelectFragmentTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IImageListService _imageListService;
        private readonly ITreeViewService _treeViewService;

        private readonly ISelectFragmentControl selectFragmentControl;

        public SelectFragmentTreeViewBuilder(
            IConfigurationService configurationService,
            IImageListService imageListService,
            ITreeViewService treeViewService,
            ISelectFragmentControl selectFragmentControl)
        {
            _configurationService = configurationService;
            _imageListService = imageListService;
            _treeViewService = treeViewService;
            this.selectFragmentControl = selectFragmentControl;
        }

        public void Build(RadTreeView treeView)
        {
            treeView.BeginUpdate();
            treeView.ImageList = _imageListService.ImageList;
            treeView.Nodes.Clear();
            TreeFolder treeFolder = _configurationService.FragmentList.FragmentsTreeFolder;
            RadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                Text = treeFolder.Name,
                Name = treeFolder.Name
            };

            treeView.Nodes.Add(rootNode);
            GetFolderChildren(treeFolder, rootNode, true);
            treeView.EndUpdate();
        }

        private void GetFolderChildren(TreeFolder treeFolder, RadTreeNode treeNode, bool root = false)
        {
            foreach (string file in treeFolder.FileNames)
            {
                Fragment fragment = _configurationService.FragmentList.Fragments[file];
                RadTreeNode childTreeNode = new()
                {
                    ImageIndex = ImageIndexes.FILEIMAGEINDEX,
                    Text = file,
                    Name = $"{treeNode.Name}/{file}"
                };
                treeNode.Nodes.Add(childTreeNode);
                childTreeNode.ToolTipText = fragment.Name;
                if (root)
                    _treeViewService.MakeVisible(childTreeNode);
            }

            foreach (TreeFolder childFolder in treeFolder.FolderNames)
            {
                RadTreeNode childFolderTreeNode = new()
                {
                    ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                    Text = childFolder.Name,
                    Name = $"{treeNode.Name}/{childFolder.Name}"
                };
                treeNode.Nodes.Add(childFolderTreeNode);
                if (root)
                    _treeViewService.MakeVisible(childFolderTreeNode);
                GetFolderChildren(childFolder, childFolderTreeNode);
                if (selectFragmentControl.ExpandedNodes.ContainsKey(childFolderTreeNode.Name))
                    childFolderTreeNode.Expand();
            }
        }
    }
}

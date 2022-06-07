using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.IO;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class SelectModulesForDeploymentTreeViewBuilder : ISelectModulesForDeploymentTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEmptyFolderRemover _emptyFolderRemover;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;

        public SelectModulesForDeploymentTreeViewBuilder(IConfigurationService configurationService, IEmptyFolderRemover emptyFolderRemover, IPathHelper pathHelper, ITreeViewService treeViewService)
        {
            _configurationService = configurationService;
            _emptyFolderRemover = emptyFolderRemover;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
        }

        public void Build(RadTreeView treeView, string applicationName)
        {
            treeView.TriStateMode = true;
            treeView.ImageList = _treeViewService.ImageList;
            treeView.Nodes.Clear();

            string rulesPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.RULESFOLDER, applicationName, ProjectPropertiesConstants.BUILDFOLDER);
            if (!Directory.Exists(rulesPath))
                Directory.CreateDirectory(rulesPath);

            treeView.BeginUpdate();
            RadTreeNode rootNode = new()
            {
                ImageIndex = TreeNodeImageIndexes.PROJECTFOLDERIMAGEINDEX,
                Text = _configurationService.ProjectProperties.ProjectName,
                Name = rulesPath
            };

            treeView.Nodes.Add(rootNode);
            AddRulesNodes(rootNode, rulesPath, true);
            _emptyFolderRemover.RemoveEmptyFolders(rootNode);
            treeView.Refresh();
            treeView.EndUpdate();
        }

        private void AddRulesNodes(RadTreeNode treeNode, string directoryPath, bool root = false)
        {
            DirectoryInfo directoryInfo = new(directoryPath);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension.ToLowerInvariant() != FileExtensions.RULESFILEEXTENSION)
                    continue;

                RadTreeNode childNode = new()
                {
                    ImageIndex = TreeNodeImageIndexes.FILEIMAGEINDEX,
                    Name = fileInfo.FullName,
                    Text = _pathHelper.GetModuleName(fileInfo.Name),
                    Tag = new RulesResourcesPair
                    (
                        fileInfo.FullName,
                        _pathHelper.CombinePaths
                        (
                            _pathHelper.GetFilePath(fileInfo.FullName), 
                            $"{_pathHelper.GetFileNameNoExtention(fileInfo.FullName)}{FileExtensions.RESOURCEFILEEXTENSION}"
                        )
                    )
                };

                treeNode.Nodes.Add(childNode);
                if (root)
                    _treeViewService.MakeVisible(childNode);
            }

            //No subfolders in the build folder
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class ExcludedModulesTreeViewBuilder : IExcludedModulesTreeViewBuilder
    {
        private readonly ICheckSelectedTreeNodes _checkSelectedTreeNodes;
        private readonly IConfigurationService _configurationService;
        private readonly IEmptyFolderRemover _emptyFolderRemover;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IImageListService _imageListService;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IUiNotificationService _uiNotificationService;

        public ExcludedModulesTreeViewBuilder(
            ICheckSelectedTreeNodes checkSelectedTreeNodes,
            IConfigurationService configurationService,
            IEmptyFolderRemover emptyFolderRemover,
            IFileIOHelper fileIOHelper,
            IImageListService imageListService,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService)
        {
            _checkSelectedTreeNodes = checkSelectedTreeNodes;
            _configurationService = configurationService;
            _emptyFolderRemover = emptyFolderRemover;
            _fileIOHelper = fileIOHelper;
            _imageListService = imageListService;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        private readonly HashSet<string> moduleNames = new();

        public void Build(RadTreeView treeView, IList<string> selectedModules)
        {
            treeView.TriStateMode = true;
            treeView.ImageList = _imageListService.ImageList;
            treeView.Nodes.Clear();
            moduleNames.Clear();/*Must be cleared on every call in case build gets called more than once on the same instance.*/

            string documentPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
            try
            {
                if (!Directory.Exists(documentPath))
                    _fileIOHelper.CreateDirectory(documentPath);
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }

            treeView.BeginUpdate();
            RadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
                Text = _configurationService.ProjectProperties.ProjectName,
                Name = documentPath
            };
            treeView.Nodes.Add(rootNode);
            AddModuleNodes(rootNode, documentPath, true);
            _emptyFolderRemover.RemoveEmptyFolders(rootNode);
            _checkSelectedTreeNodes.CheckListedNodes(rootNode, selectedModules.ToHashSet());

            treeView.Refresh();
            treeView.EndUpdate();
        }

        private void AddModuleNodes(RadTreeNode treeNode, string directoryPath, bool root = false)
        {
            DirectoryInfo directoryInfo = new(directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (!FileExtensions.DocumentExtensions.Contains(fileInfo.Extension.ToLowerInvariant()))
                    continue;

                string moduleName = _pathHelper.GetModuleName(fileInfo.FullName).Trim();
                string moduleFullName = _pathHelper.CombinePaths(_pathHelper.GetFilePath(fileInfo.FullName), moduleName);
                if (moduleNames.Contains(moduleName))
                    continue;

                RadTreeNode childNode = new()
                {
                    ImageIndex = ImageIndexes.FILEIMAGEINDEX,
                    Name = moduleFullName,
                    Text = moduleName
                };

                treeNode.Nodes.Add(childNode);

                moduleNames.Add(moduleName);
                if (root)
                    _treeViewService.MakeVisible(childNode);
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                RadTreeNode childNode = new()
                {
                    ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                    Name = subDirectoryInfo.FullName,
                    Text = subDirectoryInfo.Name
                };
                treeNode.Nodes.Add(childNode);

                if (root)
                    _treeViewService.MakeVisible(childNode);

                AddModuleNodes(childNode, _pathHelper.CombinePaths(directoryPath, subDirectoryInfo.Name));
            }
        }
    }
}

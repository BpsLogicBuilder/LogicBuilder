using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class RulesExplorerTreeViewBuilder : IRulesExplorerTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IImageListService _imageListService;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IUiNotificationService _uiNotificationService;

        private readonly IDictionary<string, string> expandedNodes;

        public RulesExplorerTreeViewBuilder(
            IConfigurationService configurationService,
            IFileIOHelper fileIOHelper,
            IImageListService imageListService,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService,
            IDictionary<string, string> expandedNodes)
        {
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _imageListService = imageListService;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            this.expandedNodes = expandedNodes;
        }

        public void Build(RadTreeView treeView)
        {
            Point point = new(treeView.HScrollBar.Value, treeView.VScrollBar.Value);
            string? selectedNodeName = treeView.SelectedNode?.Name;

            treeView.BeginUpdate();
            Build();
            treeView.EndUpdate();

            /*ScrollToPreviousPosition does not work if executed before treeView.EndUpdate();.*/
            _treeViewService.SelectTreeNode(treeView, selectedNodeName);
            _treeViewService.ScrollToPreviousPosition(treeView, point);

            void Build()
            {
                treeView.ShowRootLines = false;
                treeView.ImageList = _imageListService.ImageList;
                treeView.Nodes.Clear();

                string documentPath = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.RULESFOLDER);
                try
                {
                    foreach (Application application in _configurationService.ProjectProperties.ApplicationList.Values)
                    {
                        VerifyFolderExists(application.Name, ProjectPropertiesConstants.BUILDFOLDER);
                        VerifyFolderExists(application.Name, ProjectPropertiesConstants.DIAGRAMFOLDER);
                        VerifyFolderExists(application.Name, ProjectPropertiesConstants.TABLEFOLDER);
                    }

                    void VerifyFolderExists(string applicationName, string subDirectoryname)
                    {
                        string fullPath = _pathHelper.CombinePaths(documentPath, applicationName, subDirectoryname);
                        if (!Directory.Exists(fullPath))
                            _fileIOHelper.CreateDirectory(fullPath);
                    }
                }
                catch (LogicBuilderException ex)
                {
                    _uiNotificationService.NotifyLogicBuilderException(ex);
                    return;
                }

                StateImageRadTreeNode rootNode = new()
                {
                    ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
                    Text = _configurationService.ProjectProperties.ProjectName,
                    Name = documentPath
                };
                treeView.Nodes.Add(rootNode);

                foreach (Application application in _configurationService.ProjectProperties.ApplicationList.Values)
                {
                    AddApplicationFolderNode(rootNode, documentPath, application);
                }
            }
        }

        private void AddApplicationFolderNode(StateImageRadTreeNode treeNode, string documentPath, Application application)
        {
            DirectoryInfo subDirectoryInfo = new(_pathHelper.CombinePaths(documentPath, application.Name));
            StateImageRadTreeNode childNode = new()
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                Name = subDirectoryInfo.FullName,
                Text = subDirectoryInfo.Name
            };

            treeNode.Nodes.Add(childNode);
            _treeViewService.MakeVisible(childNode);
            if (expandedNodes.ContainsKey(childNode.Name))
                childNode.Expand();

            AddApplicationChildFolderNode(childNode, _pathHelper.CombinePaths(subDirectoryInfo.FullName, ProjectPropertiesConstants.BUILDFOLDER));
            AddApplicationChildFolderNode(childNode, _pathHelper.CombinePaths(subDirectoryInfo.FullName, ProjectPropertiesConstants.DIAGRAMFOLDER));
            AddApplicationChildFolderNode(childNode, _pathHelper.CombinePaths(subDirectoryInfo.FullName, ProjectPropertiesConstants.TABLEFOLDER));
        }

        private void AddApplicationChildFolderNode(StateImageRadTreeNode treeNode, string directoryPath)
        {
            DirectoryInfo subDirectoryInfo = new(directoryPath);
            StateImageRadTreeNode childNode = new()
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                Name = subDirectoryInfo.FullName,
                Text = subDirectoryInfo.Name
            };

            treeNode.Nodes.Add(childNode);
            if (expandedNodes.ContainsKey(childNode.Name))
                childNode.Expand();

            AddFileNodes(childNode, subDirectoryInfo.FullName);
        }

        private static void AddFileNodes(StateImageRadTreeNode treeNode, string directoryPath)
        {
            DirectoryInfo directoryInfo = new(directoryPath);
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                string fileExtension = fileInfo.Extension.ToLowerInvariant();
                if (!FileExtensions.RulesFolderFileExtensions.Contains(fileExtension))
                    continue;

                StateImageRadTreeNode childNode = new()
                {
                    ImageIndex = ImageIndexes.FILEIMAGEINDEX,
                    Name = fileInfo.FullName,
                    Text = fileInfo.Name
                };

                treeNode.Nodes.Add(childNode);
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.IO;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders
{
    internal class RulesExplorerTreeViewBuilderUtility
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        private readonly IDictionary<string, string> expandedNodes;

        public RulesExplorerTreeViewBuilderUtility(
            IConfigurationService configurationService,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService,
            IDictionary<string, string> expandedNodes)
        {
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            this.expandedNodes = expandedNodes;
        }

        public void Build(RadTreeView treeView)
        {
            treeView.BeginUpdate();

            treeView.ShowRootLines = false;
            treeView.ImageList = _treeViewService.ImageList;
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
                ImageIndex = TreeNodeImageIndexes.PROJECTFOLDERIMAGEINDEX,
                Text = _configurationService.ProjectProperties.ProjectName,
                Name = documentPath
            };
            treeView.Nodes.Add(rootNode);

            foreach (Application application in _configurationService.ProjectProperties.ApplicationList.Values)
            {
                AddApplicationFolderNode(rootNode, documentPath, application);
            }

            treeView.EndUpdate();
        }

        private void AddApplicationFolderNode(StateImageRadTreeNode treeNode, string documentPath, Application application)
        {
            DirectoryInfo subDirectoryInfo = new(_pathHelper.CombinePaths(documentPath, application.Name));
            StateImageRadTreeNode childNode = new()
            {
                ImageIndex = TreeNodeImageIndexes.CLOSEDFOLDERIMAGEINDEX,
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
                ImageIndex = TreeNodeImageIndexes.CLOSEDFOLDERIMAGEINDEX,
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
                    ImageIndex = TreeNodeImageIndexes.FILEIMAGEINDEX,
                    Name = fileInfo.FullName,
                    Text = fileInfo.Name
                };

                treeNode.Nodes.Add(childNode);
            }
        }
    }
}

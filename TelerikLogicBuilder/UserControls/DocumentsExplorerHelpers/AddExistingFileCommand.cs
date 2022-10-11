using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class AddExistingFileCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IUiNotificationService _uiNotificationService;

        public AddExistingFileCommand(
            IConfigurationService configurationService,
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                using RadOpenFileDialog openFileDialog = new();
                openFileDialog.Filter = "Visio|*.vsdx|Visio|*.vsd|Table|*.tbl";
                openFileDialog.MultiSelect = true;
                openFileDialog.InitialDirectory = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
                openFileDialog.ShowDialog();

                IEnumerable<string> files = openFileDialog.FileNames;

                if (files?.Any() == true)
                {
                    IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(_mainWindow.DocumentsExplorer.TreeView);
                    if (selectedNodes.Count != 1)
                        return;

                    RadTreeNode folderNode = GetFolderNode(selectedNodes[0]);

                    foreach (string fileFullName in files)
                    {
                        string fileName = _pathHelper.GetFileName(fileFullName).ToLowerInvariant();
                        if (_mainWindow.DocumentsExplorer.DocumentNames.TryGetValue(fileName, out string? existingFileFullPath))
                        {
                            throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fileExistsExceptionMessage, existingFileFullPath));
                        }
                        
                        _fileIOHelper.CopyFile
                        (
                            fileFullName,
                            _pathHelper.CombinePaths
                            (
                                folderNode.Name,
                                fileName
                            )
                        );

                        if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(folderNode.Name))
                            _mainWindow.DocumentsExplorer.ExpandedNodes.Add(folderNode.Name, folderNode.Text);
                    }

                    _mainWindow.DocumentsExplorer.RefreshTreeView();

                    RadTreeNode GetFolderNode(RadTreeNode treeNode) 
                        => _treeViewService.IsFileNode(treeNode) 
                            ? treeNode.Parent 
                            : treeNode;
                }
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}

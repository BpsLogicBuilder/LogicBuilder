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
        private readonly IDocumentsExplorer _documentsExplorer;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public AddExistingFileCommand(
            IConfigurationService configurationService,
            IFileIOHelper fileIOHelper,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService,
            IDocumentsExplorer documentsExplorer)
        {
            _configurationService = configurationService;
            _documentsExplorer = documentsExplorer;
            _fileIOHelper = fileIOHelper;
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
                    foreach (string fileFullName in files)
                    {
                        string fileName = _pathHelper.GetFileName(fileFullName).ToLowerInvariant();
                        if (_documentsExplorer.DocumentNames.TryGetValue(fileName, out string? existingFileFullPath))
                        {
                            throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fileExistsExceptionMessage, existingFileFullPath));
                        }

                        RadTreeNode folderNode = GetFolderNode(_documentsExplorer.TreeView.SelectedNode);
                        
                        _fileIOHelper.CopyFile
                        (
                            fileFullName,
                            _pathHelper.CombinePaths
                            (
                                folderNode.Name,
                                fileName
                            )
                        );

                        if (!_documentsExplorer.ExpandedNodes.ContainsKey(folderNode.Name))
                            _documentsExplorer.ExpandedNodes.Add(folderNode.Name, folderNode.Text);
                    }

                    _documentsExplorer.RefreshTreeView();

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

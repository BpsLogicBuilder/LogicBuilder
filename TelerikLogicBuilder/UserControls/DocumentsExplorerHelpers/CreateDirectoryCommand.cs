using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class CreateDirectoryCommand : ClickCommandBase
    {
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly IDocumentsExplorer _documentsExplorer;

        public CreateDirectoryCommand(
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService,
            IDocumentsExplorer documentsExplorer)
        {
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            _documentsExplorer = documentsExplorer;
        }

        public override void Execute()
        {
            try
            {
                CreateDirectory(_documentsExplorer.TreeView.SelectedNode);
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void CreateDirectory(RadTreeNode selectedNode)
        {
            if (selectedNode == null)
                return;

            RadTreeNode destinationFolderNode = GetFolderNode(selectedNode);

            using IScopedDisposableManager<InputBoxForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<InputBoxForm>>();
            InputBoxForm inputBox = disposableManager.ScopedService;
            inputBox.SetTitles(RegularExpressions.FILENAME, Strings.inputFileNewFolderNameCaption, Strings.inputFileNewFolderNamePrompt);
            inputBox.ShowDialog(_mainWindow.Instance);

            if (inputBox.DialogResult != DialogResult.OK)
                return;

            string newFolderFullName = _pathHelper.CombinePaths(destinationFolderNode.Name, inputBox.Input.Trim());

            if (!_documentsExplorer.ExpandedNodes.ContainsKey(destinationFolderNode.Name))
                _documentsExplorer.ExpandedNodes.Add(destinationFolderNode.Name, destinationFolderNode.Text);

            _fileIOHelper.CreateDirectory(newFolderFullName);
        }

        RadTreeNode GetFolderNode(RadTreeNode treeNode) 
           => _treeViewService.IsFileNode(treeNode)
                            ? treeNode.Parent
                            : treeNode;
    }
}

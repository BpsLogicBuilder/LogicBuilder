using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
        private readonly IUiNotificationService _uiNotificationService;

        public CreateDirectoryCommand(
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService)
        {
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
                CreateDirectory();
                _mainWindow.DocumentsExplorer.RefreshTreeView();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void CreateDirectory()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(_mainWindow.DocumentsExplorer.TreeView);
            if (selectedNodes.Count != 1)
                return;

            RadTreeNode selectedNode = selectedNodes[0];

            RadTreeNode destinationFolderNode = GetFolderNode(selectedNode);

            using IScopedDisposableManager<IInputBoxForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IInputBoxForm>>();
            IInputBoxForm inputBox = disposableManager.ScopedService;
            inputBox.SetTitles(RegularExpressions.FILENAME, Strings.inputFileNewFolderNameCaption, Strings.inputFileNewFolderNamePrompt);
            inputBox.ShowDialog(_mainWindow.Instance);

            if (inputBox.DialogResult != DialogResult.OK)
                return;

            string newFolderFullName = _pathHelper.CombinePaths(destinationFolderNode.Name, inputBox.Input.Trim());

            _fileIOHelper.CreateDirectory(newFolderFullName);

            if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(destinationFolderNode.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(destinationFolderNode.Name, destinationFolderNode.Text);
        }

        RadTreeNode GetFolderNode(RadTreeNode treeNode) 
           => _treeViewService.IsFileNode(treeNode)
                            ? treeNode.Parent
                            : treeNode;
    }
}

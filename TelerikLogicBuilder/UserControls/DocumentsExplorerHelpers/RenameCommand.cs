using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class RenameCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IMoveFileOperations _moveFileOperations;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IUiNotificationService _uiNotificationService;

        public RenameCommand(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            IMoveFileOperations moveFileOperations,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
            _moveFileOperations = moveFileOperations;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(_mainWindow.DocumentsExplorer.TreeView);
                if (selectedNodes.Count != 1)
                    return;

                RadTreeNode selectedNode = selectedNodes[0];

                if (_treeViewService.IsFileNode(selectedNode))
                {
                    RenameFile(selectedNode);
                    _mainWindow.DocumentsExplorer.RefreshTreeView();
                }
                else if (_treeViewService.IsFolderNode(selectedNode))
                {
                    RenameFolder(selectedNode);
                    _mainWindow.DocumentsExplorer.RefreshTreeView();
                }
                else if (_treeViewService.IsRootNode(selectedNode))
                {
                    RenameProject();
                }
                else
                {
                    throw _exceptionHelper.CriticalException("{0CC214A6-1749-4434-A92B-2102D965D9D2}");
                }
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void RenameFile(RadTreeNode selectedNode)
        {
            using IScopedDisposableManager<InputBoxForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<InputBoxForm>>();
            InputBoxForm inputBox = disposableManager.ScopedService;
            inputBox.Input = _pathHelper.GetFileName(selectedNode.Name);
            inputBox.SetTitles(RegularExpressions.FILENAME, Strings.inputFileNewFileNameCaption, Strings.inputFileNewFileNamePrompt);
            inputBox.ShowDialog(_mainWindow.Instance);

            if (inputBox.DialogResult != DialogResult.OK)
                return;

            string path = _pathHelper.GetFilePath(selectedNode.Name);
            string fileExtension = _pathHelper.GetExtension(selectedNode.Name).ToLowerInvariant();
            string newFileName = inputBox.Input.ToLowerInvariant().EndsWith(fileExtension) ? inputBox.Input : $"{inputBox.Input}{fileExtension}";
            string newFileFullName = _pathHelper.CombinePaths(path, newFileName);

            _moveFileOperations.MoveFile
            (
                selectedNode,
                newFileFullName
            );

            if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(selectedNode.Parent.Name))
                _mainWindow.DocumentsExplorer.ExpandedNodes.Add(selectedNode.Parent.Name, selectedNode.Parent.Text);
        }

        private void RenameFolder(RadTreeNode selectedNode)
        {
            using IScopedDisposableManager<InputBoxForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<InputBoxForm>>();
            InputBoxForm inputBox = disposableManager.ScopedService;
            inputBox.Input = _pathHelper.GetFolderName(selectedNode.Name);
            inputBox.SetTitles(RegularExpressions.FILENAME, Strings.inputFileNewFolderNameCaption, Strings.inputFileNewFolderNamePrompt);
            inputBox.ShowDialog(_mainWindow.Instance);

            if (inputBox.DialogResult != DialogResult.OK)
                return;

            DirectoryInfo directoryInfo = _fileIOHelper.GetNewDirectoryInfo(selectedNode.Name);
            string newFolderFullName = _pathHelper.CombinePaths(directoryInfo.Parent!.FullName, inputBox.Input);

            _moveFileOperations.MoveFolder
            (
                selectedNode,
                newFolderFullName
            );

            if (_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(selectedNode.Name))
            {
                if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(newFolderFullName))
                    _mainWindow.DocumentsExplorer.ExpandedNodes.Add(newFolderFullName, inputBox.Input);

                _mainWindow.DocumentsExplorer.ExpandedNodes.Remove(selectedNode.Name);
            }
        }

        private void RenameProject()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            if (mdiParent.EditControl != null)
            {
                throw new LogicBuilderException
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture, 
                        Strings.closeFileWarningFormat, 
                        _pathHelper.GetFileName(mdiParent.EditControl.SourceFile)
                    )
                );
            }

            using IScopedDisposableManager<InputBoxForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<InputBoxForm>>();
            InputBoxForm inputBox = disposableManager.ScopedService;
            inputBox.Input = _configurationService.ProjectProperties.ProjectName;
            inputBox.SetTitles(RegularExpressions.FILENAME, Strings.inputNewProjectNameCaption, Strings.inputNewProjectNamePrompt);
            inputBox.ShowDialog(_mainWindow.Instance);

            if (inputBox.DialogResult != DialogResult.OK)
                return;

            DialogResult result = DisplayMessage.ShowQuestion
            (
                _mainWindow.Instance,
                Strings.theProjectWillCloseAndReopen,
                _mainWindow.RightToLeft
            );

            if (result != DialogResult.OK)
                return;

            string newProjectName = inputBox.Input.Trim();
            string oldFileFullName = _configurationService.ProjectProperties.ProjectFileFullName;
            string oldFolderFullName = _pathHelper.RemoveTrailingPathSeparator(_configurationService.ProjectProperties.ProjectPath);
            DirectoryInfo directoryInfo = _fileIOHelper.GetNewDirectoryInfo(oldFolderFullName);
            string newFolderFullName = _pathHelper.CombinePaths
            (
                /*The ProjectProperties constructor validates that project path is not the root folder*/
                directoryInfo.Parent!.FullName, 
                newProjectName
            );

            /*This is the new file full name in the new folder*/
            string newFileFullName = _pathHelper.CombinePaths(newFolderFullName, $"{newProjectName}{FileExtensions.PROJECTFILEEXTENSION}");


            mdiParent.CloseProject();//Not closing the project will cause issues with all file system watchers.
            
            try
            {
                //first rename the project file inside the old folder
                _fileIOHelper.MoveFile
                (
                    oldFileFullName,
                    _pathHelper.CombinePaths(oldFolderFullName, $"{newProjectName}{FileExtensions.PROJECTFILEEXTENSION}")
                );
            }
            catch (LogicBuilderException)
            {
                //if rename fails reopen the project using the old project name
                mdiParent.OpenProject(oldFileFullName);
                throw;
            }

            try
            {
                if (string.Compare(oldFolderFullName, newFolderFullName, true, CultureInfo.InvariantCulture) != 0)
                    _fileIOHelper.MoveFolder(oldFolderFullName, newFolderFullName);

                mdiParent.OpenProject(newFileFullName);
            }
            catch (LogicBuilderException)
            {
                if (Directory.Exists(oldFolderFullName))
                {//Compensate if Directory could not be moved.
                    _fileIOHelper.MoveFile
                    (
                        /*new file from old folder*/
                        _pathHelper.CombinePaths(oldFolderFullName, $"{newProjectName}{FileExtensions.PROJECTFILEEXTENSION}"), 
                        oldFileFullName
                    );

                    mdiParent.OpenProject(oldFileFullName);
                    throw;
                }
                else
                {//Directory was moved but there is an exception. Too risky to leave the app open
                    throw;
                }
            }
        }
    }
}

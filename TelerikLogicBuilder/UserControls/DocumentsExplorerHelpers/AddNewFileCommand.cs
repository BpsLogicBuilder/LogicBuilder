using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class AddNewFileCommand : ClickCommandBase
    {
        private readonly IAddNewFileOperations _addNewFileOperations;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IUiNotificationService _uiNotificationService;

        public AddNewFileCommand(
            IAddNewFileOperations addNewFileOperations,
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService)
        {
            _addNewFileOperations = addNewFileOperations;
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                AddNewFile();
                _mainWindow.DocumentsExplorer.RefreshTreeView();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void AddNewFile()
        {
            IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(_mainWindow.DocumentsExplorer.TreeView);
            if (selectedNodes.Count != 1)
                return;

            RadTreeNode destinationFolderNode = GetFolderNode(selectedNodes[0]);

            using IScopedDisposableManager<IAddNewFileForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IAddNewFileForm>>();
            IAddNewFileForm addNewFile = disposableManager.ScopedService;
            addNewFile.ShowDialog(_mainWindow.Instance);

            if (addNewFile.DialogResult == DialogResult.OK)
            {
                if (_mainWindow.DocumentsExplorer.DocumentNames.TryGetValue(addNewFile.FileName.ToLowerInvariant(), out string? existingFileFullPath))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fileExistsExceptionMessage, existingFileFullPath));

                IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
                mdiParent.ChangeCursor(Cursors.WaitCursor);
                CreateFile();
                mdiParent.ChangeCursor(Cursors.Default);

                if (!_mainWindow.DocumentsExplorer.ExpandedNodes.ContainsKey(destinationFolderNode.Name))
                    _mainWindow.DocumentsExplorer.ExpandedNodes.Add(destinationFolderNode.Name, destinationFolderNode.Text);
            }

            void CreateFile()
            {
                switch (_pathHelper.GetExtension(addNewFile.FileName))
                {
                    case FileExtensions.VISIOFILEEXTENSION:
                    case FileExtensions.VSDXFILEEXTENSION:
                        _addNewFileOperations.AddNewVisioFile
                        (
                            _pathHelper.CombinePaths
                            (
                                destinationFolderNode.Name,
                                addNewFile.FileName
                            )
                        );
                        break;
                    case FileExtensions.TABLEFILEEXTENSION:
                        _addNewFileOperations.AddNewTableFile
                        (
                            _pathHelper.CombinePaths
                            (
                                destinationFolderNode.Name,
                                addNewFile.FileName
                            )
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{3D794386-72DA-4923-BA66-DFC641E78E8B}");
                }
            }

            RadTreeNode GetFolderNode(RadTreeNode treeNode)
               => _treeViewService.IsFileNode(treeNode)
                            ? treeNode.Parent
                            : treeNode;
        }
    }
}

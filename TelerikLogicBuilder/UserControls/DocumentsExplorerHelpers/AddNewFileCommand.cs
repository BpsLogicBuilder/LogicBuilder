using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class AddNewFileCommand : ClickCommandBase
    {
        private readonly IAddNewFileOperations _addNewFileOperations;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMessageBoxOptionsHelper _messageBoxOptionsHelper;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly IDocumentsExplorer _documentsExplorer;

        public AddNewFileCommand(
            IAddNewFileOperations addNewFileOperations,
            IExceptionHelper exceptionHelper,
            IMessageBoxOptionsHelper messageBoxOptionsHelper,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService,
            IDocumentsExplorer documentsExplorer)
        {
            _addNewFileOperations = addNewFileOperations;
            _exceptionHelper = exceptionHelper;
            _messageBoxOptionsHelper = messageBoxOptionsHelper;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            _documentsExplorer = documentsExplorer;
        }

        public override void Execute()
        {
            try
            {
                AddNewFile(_documentsExplorer.TreeView.SelectedNode);
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void AddNewFile(RadTreeNode selectedNode)
        {
            if (selectedNode == null)
                return;

            RadTreeNode destinationFolderNode = GetFolderNode(selectedNode);

            using IScopedDisposableManager<AddNewFileForm> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<AddNewFileForm>>();
            AddNewFileForm addNewFile = disposableManager.ScopedService;
            addNewFile.ShowDialog(_messageBoxOptionsHelper.Instance);

            if (addNewFile.DialogResult == DialogResult.OK)
            {
                if (_documentsExplorer.DocumentNames.TryGetValue(addNewFile.FileName.ToLowerInvariant(), out string? existingFileFullPath))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fileExistsExceptionMessage, existingFileFullPath));

                if (!_documentsExplorer.ExpandedNodes.ContainsKey(destinationFolderNode.Name))
                    _documentsExplorer.ExpandedNodes.Add(destinationFolderNode.Name, destinationFolderNode.Text);

                IMDIParent mdiParent = (IMDIParent)_messageBoxOptionsHelper.Instance;
                mdiParent.ChangeCursor(Cursors.WaitCursor);
                CreateFile();
                mdiParent.ChangeCursor(Cursors.Default);
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

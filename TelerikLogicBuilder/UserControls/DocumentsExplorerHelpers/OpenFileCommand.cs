using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class OpenFileCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IOpenFileOperations _openFileOperations;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;
        private readonly IDocumentsExplorer _documentsExplorer;

        public OpenFileCommand(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            IOpenFileOperations openFileOperations,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService,
            IDocumentsExplorer documentsExplorer)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _openFileOperations = openFileOperations;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
            _documentsExplorer = documentsExplorer;
        }

        public override void Execute()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            try
            {
                
                mdiParent.ChangeCursor(Cursors.WaitCursor);
                RequestDocumentOpened(_documentsExplorer.TreeView.SelectedNode);
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
            finally
            {
                mdiParent.ChangeCursor(Cursors.Default);
            }
            
            void RequestDocumentOpened(RadTreeNode selectedNode)
            {
                if (selectedNode == null
                    || _treeViewService.IsRootNode(selectedNode)
                    || _treeViewService.IsFolderNode(selectedNode))
                    return;

                switch (_pathHelper.GetExtension(selectedNode.Name))
                {
                    case FileExtensions.VISIOFILEEXTENSION:
                    case FileExtensions.VSDXFILEEXTENSION:
                        _openFileOperations.OpenVisioFile(selectedNode.Name);
                        break;
                    case FileExtensions.TABLEFILEEXTENSION:
                        _openFileOperations.OpenTableFile(selectedNode.Name);
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{AC058572-7D51-4CA7-8E84-0931AF2F9DB3}");
                }
            }
        }
    }
}

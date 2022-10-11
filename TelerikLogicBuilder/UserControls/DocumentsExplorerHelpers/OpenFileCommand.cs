using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
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
        private readonly IUiNotificationService _uiNotificationService;

        public OpenFileCommand(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            IOpenFileOperations openFileOperations,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            IUiNotificationService uiNotificationService)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _openFileOperations = openFileOperations;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            try
            {
                
                mdiParent.ChangeCursor(Cursors.WaitCursor);
                RequestDocumentOpened();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
            finally
            {
                mdiParent.ChangeCursor(Cursors.Default);
            }
            
            void RequestDocumentOpened()
            {
                IList<RadTreeNode> selectedNodes = _treeViewService.GetSelectedNodes(_mainWindow.DocumentsExplorer.TreeView);
                if (selectedNodes.Count != 1)
                    return;

                RadTreeNode selectedNode = selectedNodes[0];

                if (!_treeViewService.IsFileNode(selectedNode))
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

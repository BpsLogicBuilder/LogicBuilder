using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class OpenFileCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IOpenFileOperations _openFileOperations;
        private readonly IPathHelper _pathHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IDocumentsExplorer _documentsExplorer;

        public OpenFileCommand(
            IExceptionHelper exceptionHelper,
            IOpenFileOperations openFileOperations,
            IPathHelper pathHelper,
            ITreeViewService treeViewService,
            IDocumentsExplorer documentsExplorer)
        {
            _exceptionHelper = exceptionHelper;
            _openFileOperations = openFileOperations;
            _pathHelper = pathHelper;
            _treeViewService = treeViewService;
            _documentsExplorer = documentsExplorer;
        }

        public override void Execute()
        {
            RequestDocumentOpened(_documentsExplorer.TreeView.SelectedNode);
            
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

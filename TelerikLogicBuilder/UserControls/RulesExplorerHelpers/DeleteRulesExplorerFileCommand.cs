using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers
{
    internal class DeleteRulesExplorerFileCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewService _treeViewService;
        private readonly UiNotificationService _uiNotificationService;

        public DeleteRulesExplorerFileCommand(
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            ITreeViewService treeViewService,
            UiNotificationService uiNotificationService)
        {
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
            _treeViewService = treeViewService;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                RadTreeNode? selectedNode = _mainWindow.RulesExplorer.TreeView.SelectedNode;
                if (selectedNode == null)
                    return;

                if (!_treeViewService.IsFileNode(selectedNode))
                    throw _exceptionHelper.CriticalException("{9C072D29-B70A-4618-BE56-4841B5F74C15}");
                
                DeleteFile(selectedNode);
                _mainWindow.RulesExplorer.RefreshTreeView();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void DeleteFile(RadTreeNode treeNode)
        {
            DialogResult dialogResult = DisplayMessage.ShowQuestion
            (
                _mainWindow.Instance,
                string.Format(CultureInfo.CurrentCulture, Strings.deleteFileQuestionFormat, treeNode.Text),
                string.Empty,
                _mainWindow.RightToLeft,
                Telerik.WinControls.RadMessageIcon.Error
            );

            if (dialogResult != DialogResult.OK)
                return;

            _fileIOHelper.DeleteFile(treeNode.Name);
        }
    }
}

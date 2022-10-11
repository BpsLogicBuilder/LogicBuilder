using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers
{
    internal class DeleteAllRulesCommand : ClickCommandBase
    {
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IUiNotificationService _uiNotificationService;

        public DeleteAllRulesCommand(
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            IUiNotificationService uiNotificationService)
        {
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                DeleteAllRules();
                _mainWindow.RulesExplorer.RefreshTreeView();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void DeleteAllRules()
        {
            DialogResult dialogResult = DisplayMessage.ShowQuestion
            (
                _mainWindow.Instance,
                Strings.deleteAllRulesQuestion,
                string.Empty,
                _mainWindow.RightToLeft,
                Telerik.WinControls.RadMessageIcon.Error
            );

            if (dialogResult != DialogResult.OK)
                return;

            _fileIOHelper.DeleteFolder(_mainWindow.RulesExplorer.TreeView.Nodes[0].Name, true);
        }
    }
}

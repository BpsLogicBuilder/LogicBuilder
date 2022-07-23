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
        private readonly UiNotificationService _uiNotificationService;
        private readonly IRulesExplorer _rulesExplorer;

        public DeleteAllRulesCommand(
            IFileIOHelper fileIOHelper,
            IMainWindow mainWindow,
            UiNotificationService uiNotificationService,
            IRulesExplorer rulesExplorer)
        {
            _fileIOHelper = fileIOHelper;
            _mainWindow = mainWindow;
            _uiNotificationService = uiNotificationService;
            _rulesExplorer = rulesExplorer;
        }

        public override void Execute()
        {
            try
            {
                DeleteAllRules();
                _rulesExplorer.RefreshTreeView();
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

            _fileIOHelper.DeleteFolder(_rulesExplorer.TreeView.Nodes[0].Name, true);
        }
    }
}

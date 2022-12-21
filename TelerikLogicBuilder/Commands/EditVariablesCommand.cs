using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class EditVariablesCommand : ClickCommandBase
    {
        private readonly IEditVariables _editVariables;
        private readonly IUiNotificationService _uiNotificationService;

        public EditVariablesCommand(
            IEditVariables editVariables,
            IUiNotificationService uiNotificationService)
        {
            _editVariables = editVariables;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                _editVariables.Edit();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}

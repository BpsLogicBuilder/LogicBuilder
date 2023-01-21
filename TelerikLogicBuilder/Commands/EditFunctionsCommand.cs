using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class EditFunctionsCommand : ClickCommandBase
    {
        private readonly IEditFunctions _editFunctions;
        private readonly IUiNotificationService _uiNotificationService;

        public EditFunctionsCommand(
            IEditFunctions editFunctions,
            IUiNotificationService uiNotificationService)
        {
            _editFunctions = editFunctions;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                _editFunctions.Edit();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}

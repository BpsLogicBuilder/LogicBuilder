using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class EditConstructorsCommand : ClickCommandBase
    {
        private readonly IEditConstructors _editConstructors;
        private readonly IUiNotificationService _uiNotificationService;

        public EditConstructorsCommand(
            IEditConstructors editConstructors,
            IUiNotificationService uiNotificationService)
        {
            _editConstructors = editConstructors;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                _editConstructors.Edit();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}

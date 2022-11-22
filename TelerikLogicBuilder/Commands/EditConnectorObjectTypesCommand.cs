using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class EditConnectorObjectTypesCommand : ClickCommandBase
    {
        private readonly IEditConnectorObjectTypes _editConnectorObjectTypes;
        private readonly IUiNotificationService _uiNotificationService;

        public EditConnectorObjectTypesCommand(
            IEditConnectorObjectTypes editConnectorObjectTypes,
            IUiNotificationService uiNotificationService)
        {
            _editConnectorObjectTypes = editConnectorObjectTypes;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                _editConnectorObjectTypes.Edit();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}

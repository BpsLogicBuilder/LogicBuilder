using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class EditProjectPropertiesCommand : ClickCommandBase
    {
        private readonly IEditProjectProperties _editProjectProperties;
        private readonly IUiNotificationService _uiNotificationService;

        public EditProjectPropertiesCommand(
            IEditProjectProperties editProjectProperties,
            IUiNotificationService uiNotificationService)
        {
            _editProjectProperties = editProjectProperties;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                _editProjectProperties.Edit();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}

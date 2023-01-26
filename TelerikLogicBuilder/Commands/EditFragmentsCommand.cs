using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class EditFragmentsCommand : ClickCommandBase
    {
        private readonly IEditFragments _editFragments;
        private readonly IUiNotificationService _uiNotificationService;

        public EditFragmentsCommand(
            IEditFragments editFragments,
            IUiNotificationService uiNotificationService)
        {
            _editFragments = editFragments;
            _uiNotificationService = uiNotificationService;
        }

        public override void Execute()
        {
            try
            {
                _editFragments.Edit();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }
    }
}

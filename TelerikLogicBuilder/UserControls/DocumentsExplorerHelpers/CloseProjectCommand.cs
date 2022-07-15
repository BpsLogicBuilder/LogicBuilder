using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class CloseProjectCommand : ClickCommandBase
    {
        private readonly IMessageBoxOptionsHelper _messageBoxOptionsHelper;

        public CloseProjectCommand(IMessageBoxOptionsHelper messageBoxOptionsHelper)
        {
            _messageBoxOptionsHelper = messageBoxOptionsHelper;
        }

        public override void Execute()
        {
            ((IMDIParent)_messageBoxOptionsHelper.MainWindow).CloseProject();
        }
    }
}

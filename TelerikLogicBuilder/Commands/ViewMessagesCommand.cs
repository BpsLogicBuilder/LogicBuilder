using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ViewMessagesCommand : ClickCommandBase
    {
        private readonly IMessages _messages;

        public ViewMessagesCommand(IMessages messages)
        {
            _messages = messages;
        }

        public override void Execute()
        {
            _messages.Visible = true;
        }
    }
}

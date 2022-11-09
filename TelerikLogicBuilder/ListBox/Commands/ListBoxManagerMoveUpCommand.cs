using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.ListBox.Commands
{
    internal class ListBoxManagerMoveUpCommand : ClickCommandBase
    {
        private readonly IRadListBoxManager _radListBoxManager;

        public ListBoxManagerMoveUpCommand(IRadListBoxManager radListBoxManager)
        {
            _radListBoxManager = radListBoxManager;
        }

        public override void Execute()
        {
            _radListBoxManager.MoveUp();
        }
    }
}

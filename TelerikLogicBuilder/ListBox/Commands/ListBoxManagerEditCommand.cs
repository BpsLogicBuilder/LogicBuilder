using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.ListBox.Commands
{
    internal class ListBoxManagerEditCommand : ClickCommandBase
    {
        private readonly IRadListBoxManager _radListBoxManager;

        public ListBoxManagerEditCommand(IRadListBoxManager radListBoxManager)
        {
            _radListBoxManager = radListBoxManager;
        }

        public override void Execute()
        {
            _radListBoxManager.Edit();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.ListBox.Commands
{
    internal class ListBoxManagerRemoveCommand : ClickCommandBase
    {
        private readonly IRadListBoxManager _radListBoxManager;

        public ListBoxManagerRemoveCommand(IRadListBoxManager radListBoxManager)
        {
            _radListBoxManager = radListBoxManager;
        }

        public override void Execute()
        {
            _radListBoxManager.Remove();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands
{
    internal class UpdateVariableLiteralListBoxItemCommand : ClickCommandBase
    {
        private readonly ILiteralListBoxItemFactory _literalListBoxItemFactory;
        private readonly IEditVariableLiteralListControl editVariableLiteralListControl;

        public UpdateVariableLiteralListBoxItemCommand(
            ILiteralListBoxItemFactory literalListBoxItemFactory,
            IEditVariableLiteralListControl editVariableLiteralListControl)
        {
            _literalListBoxItemFactory = literalListBoxItemFactory;
            this.editVariableLiteralListControl = editVariableLiteralListControl;
        }

        private IRadListBoxManager<ILiteralListBoxItem> RadListBoxManager => editVariableLiteralListControl.RadListBoxManager;

        public override void Execute()
        {
            RadListBoxManager.Update
            (
                _literalListBoxItemFactory.GetVariableLiteralListBoxItem
                (
                    editVariableLiteralListControl.ValueControl.VisibleText,
                    editVariableLiteralListControl.ValueControl.MixedXml,
                    editVariableLiteralListControl.LiteralType,
                    editVariableLiteralListControl.ApplicationControl,
                    editVariableLiteralListControl.ListControl
                )
            );
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands
{
    internal class AddLiteralListBoxItemCommand : ClickCommandBase
    {
        private readonly ILiteralListBoxItemFactory _literalListBoxItemFactory;
        private readonly IEditParameterLiteralListControl editLiteralListControl;

        public AddLiteralListBoxItemCommand(
            ILiteralListBoxItemFactory literalListBoxItemFactory,
            IEditParameterLiteralListControl editLiteralListControl)
        {
            _literalListBoxItemFactory = literalListBoxItemFactory;
            this.editLiteralListControl = editLiteralListControl;
        }

        private IRadListBoxManager<ILiteralListBoxItem> RadListBoxManager => editLiteralListControl.RadListBoxManager;

        public override void Execute()
        {
            RadListBoxManager.Add
            (
                _literalListBoxItemFactory.GetParameterLiteralListBoxItem
                (
                    editLiteralListControl.ValueControl.VisibleText,
                    editLiteralListControl.ValueControl.MixedXml,
                    editLiteralListControl.LiteralType,
                    editLiteralListControl.ApplicationControl,
                    editLiteralListControl.ListControl
                )
            );
        }
    }
}

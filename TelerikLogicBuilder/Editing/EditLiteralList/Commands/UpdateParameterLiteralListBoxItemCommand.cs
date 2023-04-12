using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands
{
    internal class UpdateParameterLiteralListBoxItemCommand : ClickCommandBase
    {
        private readonly ILiteralListBoxItemFactory _literalListBoxItemFactory;
        private readonly IEditParameterLiteralListControl editParameterLiteralListControl;

        public UpdateParameterLiteralListBoxItemCommand(
            ILiteralListBoxItemFactory literalListBoxItemFactory,
            IEditParameterLiteralListControl editParameterLiteralListControl)
        {
            _literalListBoxItemFactory = literalListBoxItemFactory;
            this.editParameterLiteralListControl = editParameterLiteralListControl;
        }

        private IRadListBoxManager<ILiteralListBoxItem> RadListBoxManager => editParameterLiteralListControl.RadListBoxManager;

        public override void Execute()
        {
            RadListBoxManager.Update
            (
                _literalListBoxItemFactory.GetParameterLiteralListBoxItem
                (
                    editParameterLiteralListControl.ValueControl.VisibleText,
                    editParameterLiteralListControl.ValueControl.MixedXml,
                    editParameterLiteralListControl.LiteralType,
                    editParameterLiteralListControl.ApplicationControl,
                    editParameterLiteralListControl.ListControl
                )
            );
        }
    }
}

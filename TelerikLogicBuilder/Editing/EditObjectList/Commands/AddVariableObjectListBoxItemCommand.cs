using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands
{
    internal class AddVariableObjectListBoxItemCommand : ClickCommandBase
    {
        private readonly IObjectListBoxItemFactory _objectListBoxItemFactory;
        private readonly IEditVariableObjectListControl editVariableObjectListControl;

        public AddVariableObjectListBoxItemCommand(
            IObjectListBoxItemFactory objectListBoxItemFactory,
            IEditVariableObjectListControl editVariableObjectListControl)
        {
            _objectListBoxItemFactory = objectListBoxItemFactory;
            this.editVariableObjectListControl = editVariableObjectListControl;
        }

        private IRadListBoxManager<IObjectListBoxItem> RadListBoxManager => editVariableObjectListControl.RadListBoxManager;

        public override void Execute()
        {
            if (this.editVariableObjectListControl.ValueControl.IsEmpty)
                return;

            RadListBoxManager.Add
            (
                _objectListBoxItemFactory.GetVariableObjectListBoxItem
                (
                    editVariableObjectListControl.ValueControl.VisibleText,
                    editVariableObjectListControl.ValueControl.MixedXml,
                    editVariableObjectListControl.ObjectType,
                    editVariableObjectListControl.ApplicationControl,
                    editVariableObjectListControl.ListControl
                )
            );
        }
    }
}

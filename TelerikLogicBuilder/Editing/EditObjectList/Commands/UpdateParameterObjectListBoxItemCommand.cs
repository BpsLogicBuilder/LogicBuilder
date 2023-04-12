using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands
{
    internal class UpdateParameterObjectListBoxItemCommand : ClickCommandBase
    {
        private readonly IObjectListBoxItemFactory _objectListBoxItemFactory;
        private readonly IEditParameterObjectListControl editParameterObjectListControl;

        public UpdateParameterObjectListBoxItemCommand(
            IObjectListBoxItemFactory objectListBoxItemFactory,
            IEditParameterObjectListControl editParameterObjectListControl)
        {
            _objectListBoxItemFactory = objectListBoxItemFactory;
            this.editParameterObjectListControl = editParameterObjectListControl;
        }

        private IRadListBoxManager<IObjectListBoxItem> RadListBoxManager => editParameterObjectListControl.RadListBoxManager;

        public override void Execute()
        {
            if (this.editParameterObjectListControl.ValueControl.IsEmpty)
                return;

            RadListBoxManager.Update
            (
                _objectListBoxItemFactory.GetParameterObjectListBoxItem
                (
                    editParameterObjectListControl.ValueControl.VisibleText,
                    editParameterObjectListControl.ValueControl.MixedXml,
                    editParameterObjectListControl.ObjectType,
                    editParameterObjectListControl.ApplicationControl,
                    editParameterObjectListControl.ListControl
                )
            );
        }
    }
}

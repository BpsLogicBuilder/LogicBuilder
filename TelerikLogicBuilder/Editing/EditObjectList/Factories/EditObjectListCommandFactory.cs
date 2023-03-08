using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class EditObjectListCommandFactory : IEditObjectListCommandFactory
    {
        private readonly Func<IEditObjectListControl, AddObjectListBoxItemCommand> _getAddObjectListBoxItemCommand;
        private readonly Func<IEditObjectListControl, UpdateObjectListBoxItemCommand> _getUpdateObjectListBoxItemCommand;

        public EditObjectListCommandFactory(
            Func<IEditObjectListControl, AddObjectListBoxItemCommand> getAddObjectListBoxItemCommand,
            Func<IEditObjectListControl, UpdateObjectListBoxItemCommand> getUpdateObjectListBoxItemCommand)
        {
            _getAddObjectListBoxItemCommand = getAddObjectListBoxItemCommand;
            _getUpdateObjectListBoxItemCommand = getUpdateObjectListBoxItemCommand;
        }

        public AddObjectListBoxItemCommand GetAddObjectListBoxItemCommand(IEditObjectListControl editObjectListControl)
            => _getAddObjectListBoxItemCommand(editObjectListControl);

        public UpdateObjectListBoxItemCommand GetUpdateObjectListBoxItemCommand(IEditObjectListControl editObjectListControl)
            => _getUpdateObjectListBoxItemCommand(editObjectListControl);
    }
}

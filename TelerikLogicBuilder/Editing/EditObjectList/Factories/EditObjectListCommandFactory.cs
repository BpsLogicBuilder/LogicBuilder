using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class EditObjectListCommandFactory : IEditObjectListCommandFactory
    {
        private readonly Func<IEditObjectListControl, AddObjectListBoxItemCommand> _getAddObjectListBoxItemCommand;
        private readonly Func<IEditObjectListForm, EditObjectListFormXmlCommand> _getEditObjectListFormXmlCommand;
        private readonly Func<IEditObjectListControl, UpdateObjectListBoxItemCommand> _getUpdateObjectListBoxItemCommand;

        public EditObjectListCommandFactory(
            Func<IEditObjectListControl, AddObjectListBoxItemCommand> getAddObjectListBoxItemCommand,
            Func<IEditObjectListForm, EditObjectListFormXmlCommand> getEditObjectListFormXmlCommand,
            Func<IEditObjectListControl, UpdateObjectListBoxItemCommand> getUpdateObjectListBoxItemCommand)
        {
            _getAddObjectListBoxItemCommand = getAddObjectListBoxItemCommand;
            _getEditObjectListFormXmlCommand = getEditObjectListFormXmlCommand;
            _getUpdateObjectListBoxItemCommand = getUpdateObjectListBoxItemCommand;
        }

        public AddObjectListBoxItemCommand GetAddObjectListBoxItemCommand(IEditObjectListControl editObjectListControl)
            => _getAddObjectListBoxItemCommand(editObjectListControl);

        public EditObjectListFormXmlCommand GetEditObjectListFormXmlCommand(IEditObjectListForm editObjectListForm)
            => _getEditObjectListFormXmlCommand(editObjectListForm);

        public UpdateObjectListBoxItemCommand GetUpdateObjectListBoxItemCommand(IEditObjectListControl editObjectListControl)
            => _getUpdateObjectListBoxItemCommand(editObjectListControl);
    }
}

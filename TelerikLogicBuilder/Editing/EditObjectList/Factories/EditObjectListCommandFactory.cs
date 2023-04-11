using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class EditObjectListCommandFactory : IEditObjectListCommandFactory
    {
        private readonly Func<IEditParameterObjectListControl, AddObjectListBoxItemCommand> _getAddObjectListBoxItemCommand;
        private readonly Func<IEditParameterObjectListForm, EditObjectListFormXmlCommand> _getEditObjectListFormXmlCommand;
        private readonly Func<IEditParameterObjectListControl, UpdateObjectListBoxItemCommand> _getUpdateObjectListBoxItemCommand;

        public EditObjectListCommandFactory(
            Func<IEditParameterObjectListControl, AddObjectListBoxItemCommand> getAddObjectListBoxItemCommand,
            Func<IEditParameterObjectListForm, EditObjectListFormXmlCommand> getEditObjectListFormXmlCommand,
            Func<IEditParameterObjectListControl, UpdateObjectListBoxItemCommand> getUpdateObjectListBoxItemCommand)
        {
            _getAddObjectListBoxItemCommand = getAddObjectListBoxItemCommand;
            _getEditObjectListFormXmlCommand = getEditObjectListFormXmlCommand;
            _getUpdateObjectListBoxItemCommand = getUpdateObjectListBoxItemCommand;
        }

        public AddObjectListBoxItemCommand GetAddObjectListBoxItemCommand(IEditParameterObjectListControl editObjectListControl)
            => _getAddObjectListBoxItemCommand(editObjectListControl);

        public EditObjectListFormXmlCommand GetEditObjectListFormXmlCommand(IEditParameterObjectListForm editObjectListForm)
            => _getEditObjectListFormXmlCommand(editObjectListForm);

        public UpdateObjectListBoxItemCommand GetUpdateObjectListBoxItemCommand(IEditParameterObjectListControl editObjectListControl)
            => _getUpdateObjectListBoxItemCommand(editObjectListControl);
    }
}

using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class EditObjectListCommandFactory : IEditObjectListCommandFactory
    {
        private readonly Func<IEditParameterObjectListControl, AddParameterObjectListBoxItemCommand> _getAddObjectListBoxItemCommand;
        private readonly Func<IEditParameterObjectListForm, EditParameterObjectListFormXmlCommand> _getEditObjectListFormXmlCommand;
        private readonly Func<IEditParameterObjectListControl, UpdateParameterObjectListBoxItemCommand> _getUpdateObjectListBoxItemCommand;

        public EditObjectListCommandFactory(
            Func<IEditParameterObjectListControl, AddParameterObjectListBoxItemCommand> getAddObjectListBoxItemCommand,
            Func<IEditParameterObjectListForm, EditParameterObjectListFormXmlCommand> getEditObjectListFormXmlCommand,
            Func<IEditParameterObjectListControl, UpdateParameterObjectListBoxItemCommand> getUpdateObjectListBoxItemCommand)
        {
            _getAddObjectListBoxItemCommand = getAddObjectListBoxItemCommand;
            _getEditObjectListFormXmlCommand = getEditObjectListFormXmlCommand;
            _getUpdateObjectListBoxItemCommand = getUpdateObjectListBoxItemCommand;
        }

        public AddParameterObjectListBoxItemCommand GetAddParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl)
            => _getAddObjectListBoxItemCommand(editParameterObjectListControl);

        public EditParameterObjectListFormXmlCommand GetEditParameterObjectListFormXmlCommand(IEditParameterObjectListForm editParameterObjectListForm)
            => _getEditObjectListFormXmlCommand(editParameterObjectListForm);

        public UpdateParameterObjectListBoxItemCommand GetUpdateParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl)
            => _getUpdateObjectListBoxItemCommand(editParameterObjectListControl);
    }
}

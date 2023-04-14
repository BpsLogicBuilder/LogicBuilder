using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal class EditObjectListCommandFactory : IEditObjectListCommandFactory
    {
        private readonly Func<IEditParameterObjectListControl, AddParameterObjectListBoxItemCommand> _getAddParameterObjectListBoxItemCommand;
        private readonly Func<IEditVariableObjectListControl, AddVariableObjectListBoxItemCommand> _getAddVariableObjectListBoxItemCommand;
        private readonly Func<IEditParameterObjectListForm, EditParameterObjectListFormXmlCommand> _getEditParameterObjectListFormXmlCommand;
        private readonly Func<IEditVariableObjectListForm, EditVariableObjectListFormXmlCommand> _getEditVariableObjectListFormXmlCommand;
        private readonly Func<IEditParameterObjectListControl, UpdateParameterObjectListBoxItemCommand> _getUpdateParameterObjectListBoxItemCommand;
        private readonly Func<IEditVariableObjectListControl, UpdateVariableObjectListBoxItemCommand> _getUpdateVariableObjectListBoxItemCommand;

        public EditObjectListCommandFactory(
            Func<IEditParameterObjectListControl, AddParameterObjectListBoxItemCommand> getAddParameterObjectListBoxItemCommand,
            Func<IEditVariableObjectListControl, AddVariableObjectListBoxItemCommand> getAddVariableObjectListBoxItemCommand,
            Func<IEditParameterObjectListForm, EditParameterObjectListFormXmlCommand> getEditParameterObjectListFormXmlCommand,
            Func<IEditVariableObjectListForm, EditVariableObjectListFormXmlCommand> getEditVariableObjectListFormXmlCommand,
            Func<IEditParameterObjectListControl, UpdateParameterObjectListBoxItemCommand> getUpdateParameterObjectListBoxItemCommand,
            Func<IEditVariableObjectListControl, UpdateVariableObjectListBoxItemCommand> getUpdateVariableObjectListBoxItemCommand)
        {
            _getAddParameterObjectListBoxItemCommand = getAddParameterObjectListBoxItemCommand;
            _getAddVariableObjectListBoxItemCommand = getAddVariableObjectListBoxItemCommand;
            _getEditParameterObjectListFormXmlCommand = getEditParameterObjectListFormXmlCommand;
            _getEditVariableObjectListFormXmlCommand = getEditVariableObjectListFormXmlCommand;
            _getUpdateParameterObjectListBoxItemCommand = getUpdateParameterObjectListBoxItemCommand;
            _getUpdateVariableObjectListBoxItemCommand = getUpdateVariableObjectListBoxItemCommand;
        }

        public AddParameterObjectListBoxItemCommand GetAddParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl)
            => _getAddParameterObjectListBoxItemCommand(editParameterObjectListControl);

        public AddVariableObjectListBoxItemCommand GetAddVariableObjectListBoxItemCommand(IEditVariableObjectListControl editVariableObjectListControl)
            => _getAddVariableObjectListBoxItemCommand(editVariableObjectListControl);

        public EditParameterObjectListFormXmlCommand GetEditParameterObjectListFormXmlCommand(IEditParameterObjectListForm editParameterObjectListForm)
            => _getEditParameterObjectListFormXmlCommand(editParameterObjectListForm);

        public EditVariableObjectListFormXmlCommand GetEditVariableObjectListFormXmlCommand(IEditVariableObjectListForm editVariableObjectListForm)
            => _getEditVariableObjectListFormXmlCommand(editVariableObjectListForm);

        public UpdateParameterObjectListBoxItemCommand GetUpdateParameterObjectListBoxItemCommand(IEditParameterObjectListControl editParameterObjectListControl)
            => _getUpdateParameterObjectListBoxItemCommand(editParameterObjectListControl);

        public UpdateVariableObjectListBoxItemCommand GetUpdateVariableObjectListBoxItemCommand(IEditVariableObjectListControl editVariableObjectListControl)
            => _getUpdateVariableObjectListBoxItemCommand(editVariableObjectListControl);
    }
}

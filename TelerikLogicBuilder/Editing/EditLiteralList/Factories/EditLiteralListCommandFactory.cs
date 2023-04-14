using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class EditLiteralListCommandFactory : IEditLiteralListCommandFactory
    {
        private readonly Func<IEditParameterLiteralListControl, AddParameterLiteralListBoxItemCommand> _getAddParameterLiteralListBoxItemCommand;
        private readonly Func<IEditVariableLiteralListControl, AddVariableLiteralListBoxItemCommand> _getAddVariableLiteralListBoxItemCommand;
        private readonly Func<IEditParameterLiteralListForm, EditParameterLiteralListFormXmlCommand> _getEditParameterLiteralListFormXmlCommand;
        private readonly Func<IEditVariableLiteralListForm, EditVariableLiteralListFormXmlCommand> _getEditVariableLiteralListFormXmlCommand;
        private readonly Func<IEditVariableLiteralListControl, UpdateVariableLiteralListBoxItemCommand> _getUpdateVariableLiteralListBoxItemCommand;
        private readonly Func<IEditParameterLiteralListControl, UpdateParameterLiteralListBoxItemCommand> _getUpdateParameterLiteralListBoxItemCommand;

        public EditLiteralListCommandFactory(
            Func<IEditParameterLiteralListControl, AddParameterLiteralListBoxItemCommand> getAddParameterLiteralListBoxItemCommand,
            Func<IEditVariableLiteralListControl, AddVariableLiteralListBoxItemCommand> getAddVariableLiteralListBoxItemCommand,
            Func<IEditParameterLiteralListForm, EditParameterLiteralListFormXmlCommand> getEditParameterLiteralListFormXmlCommand,
            Func<IEditVariableLiteralListForm, EditVariableLiteralListFormXmlCommand> getEditVariableLiteralListFormXmlCommand,
            Func<IEditParameterLiteralListControl, UpdateParameterLiteralListBoxItemCommand> getUpdateParameterLiteralListBoxItemCommand,
            Func<IEditVariableLiteralListControl, UpdateVariableLiteralListBoxItemCommand> getUpdateVariableLiteralListBoxItemCommand)
        {
            _getAddParameterLiteralListBoxItemCommand = getAddParameterLiteralListBoxItemCommand;
            _getAddVariableLiteralListBoxItemCommand = getAddVariableLiteralListBoxItemCommand;
            _getEditParameterLiteralListFormXmlCommand = getEditParameterLiteralListFormXmlCommand;
            _getEditVariableLiteralListFormXmlCommand = getEditVariableLiteralListFormXmlCommand;
            _getUpdateParameterLiteralListBoxItemCommand = getUpdateParameterLiteralListBoxItemCommand;
            _getUpdateVariableLiteralListBoxItemCommand = getUpdateVariableLiteralListBoxItemCommand;
        }

        public AddParameterLiteralListBoxItemCommand GetAddParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl)
            => _getAddParameterLiteralListBoxItemCommand(editParameterLiteralListControl);

        public AddVariableLiteralListBoxItemCommand GetAddVariableLiteralListBoxItemCommand(IEditVariableLiteralListControl editVariableLiteralListControl)
            => _getAddVariableLiteralListBoxItemCommand(editVariableLiteralListControl);

        public EditParameterLiteralListFormXmlCommand GetEditParameterLiteralListFormXmlCommand(IEditParameterLiteralListForm editParameterLiteralListForm)
            => _getEditParameterLiteralListFormXmlCommand(editParameterLiteralListForm);

        public EditVariableLiteralListFormXmlCommand GetEditVariableLiteralListFormXmlCommand(IEditVariableLiteralListForm editVariableLiteralListForm)
            => _getEditVariableLiteralListFormXmlCommand(editVariableLiteralListForm);

        public UpdateParameterLiteralListBoxItemCommand GetUpdateParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl)
            => _getUpdateParameterLiteralListBoxItemCommand(editParameterLiteralListControl);

        public UpdateVariableLiteralListBoxItemCommand GetUpdateVariableLiteralListBoxItemCommand(IEditVariableLiteralListControl editVariableLiteralListControl)
            => _getUpdateVariableLiteralListBoxItemCommand(editVariableLiteralListControl);
    }
}

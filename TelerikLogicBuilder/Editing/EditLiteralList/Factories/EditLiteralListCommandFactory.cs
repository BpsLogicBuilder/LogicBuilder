using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class EditLiteralListCommandFactory : IEditLiteralListCommandFactory
    {
        private readonly Func<IEditParameterLiteralListControl, AddParameterLiteralListBoxItemCommand> _getAddLiteralListBoxItemCommand;
        private readonly Func<IEditParameterLiteralListForm, EditParameterLiteralListFormXmlCommand> _getEditLiteralListFormXmlCommand;
        private readonly Func<IEditParameterLiteralListControl, UpdateParameterLiteralListBoxItemCommand> _getUpdateLiteralListBoxItemCommand;

        public EditLiteralListCommandFactory(
            Func<IEditParameterLiteralListControl, AddParameterLiteralListBoxItemCommand> getAddLiteralListBoxItemCommand,
            Func<IEditParameterLiteralListForm, EditParameterLiteralListFormXmlCommand> getEditLiteralListFormXmlCommand,
            Func<IEditParameterLiteralListControl, UpdateParameterLiteralListBoxItemCommand> getUpdateLiteralListBoxItemCommand)
        {
            _getAddLiteralListBoxItemCommand = getAddLiteralListBoxItemCommand;
            _getEditLiteralListFormXmlCommand = getEditLiteralListFormXmlCommand;
            _getUpdateLiteralListBoxItemCommand = getUpdateLiteralListBoxItemCommand;
        }

        public AddParameterLiteralListBoxItemCommand GetAddParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl)
            => _getAddLiteralListBoxItemCommand(editParameterLiteralListControl);

        public EditParameterLiteralListFormXmlCommand GetEditParameterLiteralListFormXmlCommand(IEditParameterLiteralListForm editParameterLiteralListForm)
            => _getEditLiteralListFormXmlCommand(editParameterLiteralListForm);

        public UpdateParameterLiteralListBoxItemCommand GetUpdateParameterLiteralListBoxItemCommand(IEditParameterLiteralListControl editParameterLiteralListControl)
            => _getUpdateLiteralListBoxItemCommand(editParameterLiteralListControl);
    }
}

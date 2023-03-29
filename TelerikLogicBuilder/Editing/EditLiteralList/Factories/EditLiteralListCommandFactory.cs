using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class EditLiteralListCommandFactory : IEditLiteralListCommandFactory
    {
        private readonly Func<IEditLiteralListControl, AddLiteralListBoxItemCommand> _getAddLiteralListBoxItemCommand;
        private readonly Func<IEditLiteralListForm, EditLiteralListFormXmlCommand> _getEditLiteralListFormXmlCommand;
        private readonly Func<IEditLiteralListControl, UpdateLiteralListBoxItemCommand> _getUpdateLiteralListBoxItemCommand;

        public EditLiteralListCommandFactory(
            Func<IEditLiteralListControl, AddLiteralListBoxItemCommand> getAddLiteralListBoxItemCommand,
            Func<IEditLiteralListForm, EditLiteralListFormXmlCommand> getEditLiteralListFormXmlCommand,
            Func<IEditLiteralListControl, UpdateLiteralListBoxItemCommand> getUpdateLiteralListBoxItemCommand)
        {
            _getAddLiteralListBoxItemCommand = getAddLiteralListBoxItemCommand;
            _getEditLiteralListFormXmlCommand = getEditLiteralListFormXmlCommand;
            _getUpdateLiteralListBoxItemCommand = getUpdateLiteralListBoxItemCommand;
        }

        public AddLiteralListBoxItemCommand GetAddLiteralListBoxItemCommand(IEditLiteralListControl editLiteralListControl)
            => _getAddLiteralListBoxItemCommand(editLiteralListControl);

        public EditLiteralListFormXmlCommand GetEditLiteralListFormXmlCommand(IEditLiteralListForm editLiteralListForm)
            => _getEditLiteralListFormXmlCommand(editLiteralListForm);

        public UpdateLiteralListBoxItemCommand GetUpdateLiteralListBoxItemCommand(IEditLiteralListControl editLiteralListControl)
            => _getUpdateLiteralListBoxItemCommand(editLiteralListControl);
    }
}

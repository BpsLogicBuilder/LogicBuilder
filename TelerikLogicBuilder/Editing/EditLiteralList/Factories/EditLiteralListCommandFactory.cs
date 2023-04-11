using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class EditLiteralListCommandFactory : IEditLiteralListCommandFactory
    {
        private readonly Func<IEditParameterLiteralListControl, AddLiteralListBoxItemCommand> _getAddLiteralListBoxItemCommand;
        private readonly Func<IEditParameterLiteralListForm, EditLiteralListFormXmlCommand> _getEditLiteralListFormXmlCommand;
        private readonly Func<IEditParameterLiteralListControl, UpdateLiteralListBoxItemCommand> _getUpdateLiteralListBoxItemCommand;

        public EditLiteralListCommandFactory(
            Func<IEditParameterLiteralListControl, AddLiteralListBoxItemCommand> getAddLiteralListBoxItemCommand,
            Func<IEditParameterLiteralListForm, EditLiteralListFormXmlCommand> getEditLiteralListFormXmlCommand,
            Func<IEditParameterLiteralListControl, UpdateLiteralListBoxItemCommand> getUpdateLiteralListBoxItemCommand)
        {
            _getAddLiteralListBoxItemCommand = getAddLiteralListBoxItemCommand;
            _getEditLiteralListFormXmlCommand = getEditLiteralListFormXmlCommand;
            _getUpdateLiteralListBoxItemCommand = getUpdateLiteralListBoxItemCommand;
        }

        public AddLiteralListBoxItemCommand GetAddLiteralListBoxItemCommand(IEditParameterLiteralListControl editLiteralListControl)
            => _getAddLiteralListBoxItemCommand(editLiteralListControl);

        public EditLiteralListFormXmlCommand GetEditLiteralListFormXmlCommand(IEditParameterLiteralListForm editLiteralListForm)
            => _getEditLiteralListFormXmlCommand(editLiteralListForm);

        public UpdateLiteralListBoxItemCommand GetUpdateLiteralListBoxItemCommand(IEditParameterLiteralListControl editLiteralListControl)
            => _getUpdateLiteralListBoxItemCommand(editLiteralListControl);
    }
}

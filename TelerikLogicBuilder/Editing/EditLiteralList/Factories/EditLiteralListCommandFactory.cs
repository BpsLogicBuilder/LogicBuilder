using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal class EditLiteralListCommandFactory : IEditLiteralListCommandFactory
    {
        private readonly Func<IEditLiteralListControl, AddLiteralListBoxItemCommand> _getAddLiteralListBoxItemCommand;
        private readonly Func<IEditLiteralListControl, UpdateLiteralListBoxItemCommand> _getUpdateLiteralListBoxItemCommand;

        public EditLiteralListCommandFactory(
            Func<IEditLiteralListControl, AddLiteralListBoxItemCommand> getAddLiteralListBoxItemCommand,
            Func<IEditLiteralListControl, UpdateLiteralListBoxItemCommand> getUpdateLiteralListBoxItemCommand)
        {
            _getAddLiteralListBoxItemCommand = getAddLiteralListBoxItemCommand;
            _getUpdateLiteralListBoxItemCommand = getUpdateLiteralListBoxItemCommand;
        }

        public AddLiteralListBoxItemCommand GetAddLiteralListBoxItemCommand(IEditLiteralListControl editLiteralListControl)
            => _getAddLiteralListBoxItemCommand(editLiteralListControl);

        public UpdateLiteralListBoxItemCommand GetUpdateLiteralListBoxItemCommand(IEditLiteralListControl editLiteralListControl)
            => _getUpdateLiteralListBoxItemCommand(editLiteralListControl);
    }
}

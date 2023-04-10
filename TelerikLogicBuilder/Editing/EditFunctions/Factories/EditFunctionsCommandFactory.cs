using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal class EditFunctionsCommandFactory : IEditFunctionsCommandFactory
    {
        private readonly Func<IEditFunctionsForm, AddFunctionListBoxItemCommand> _getAddFunctionListBoxItemCommand;
        private readonly Func<IEditFunctionsForm, EditFunctionsFormCopyXmlCommand> _getEditFFunctionsFormCopyXmlCommand;
        private readonly Func<IEditFunctionsForm, EditFunctionsFormEditXmlCommand> _getEditFunctionsFormXmlCommand;
        private readonly Func<IEditFunctionsForm, UpdateFunctionListBoxItemCommand> _getUpdateFunctionListBoxItemCommand;

        public EditFunctionsCommandFactory(Func<IEditFunctionsForm, AddFunctionListBoxItemCommand> getAddFunctionListBoxItemCommand,
            Func<IEditFunctionsForm, EditFunctionsFormCopyXmlCommand> getEditFFunctionsFormCopyXmlCommand,
            Func<IEditFunctionsForm, EditFunctionsFormEditXmlCommand> getEditFunctionsFormXmlCommand,
            Func<IEditFunctionsForm, UpdateFunctionListBoxItemCommand> getUpdateFunctionListBoxItemCommand)
        {
            _getAddFunctionListBoxItemCommand = getAddFunctionListBoxItemCommand;
            _getEditFFunctionsFormCopyXmlCommand = getEditFFunctionsFormCopyXmlCommand;
            _getEditFunctionsFormXmlCommand = getEditFunctionsFormXmlCommand;
            _getUpdateFunctionListBoxItemCommand = getUpdateFunctionListBoxItemCommand;
        }

        public AddFunctionListBoxItemCommand GetAddFunctionListBoxItemCommand(IEditFunctionsForm editFunctionsForm)
            => _getAddFunctionListBoxItemCommand(editFunctionsForm);

        public EditFunctionsFormCopyXmlCommand GetEditFFunctionsFormCopyXmlCommand(IEditFunctionsForm editFunctionsForm)
            => _getEditFFunctionsFormCopyXmlCommand(editFunctionsForm);

        public EditFunctionsFormEditXmlCommand GetEditFunctionsFormXmlCommand(IEditFunctionsForm editFunctionsForm)
            => _getEditFunctionsFormXmlCommand(editFunctionsForm);

        public UpdateFunctionListBoxItemCommand GetUpdateFunctionListBoxItemCommand(IEditFunctionsForm editFunctionsForm)
            => _getUpdateFunctionListBoxItemCommand(editFunctionsForm);
    }
}

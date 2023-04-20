using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories
{
    internal class EditConditionFunctionsFormCommandFactory : IEditConditionFunctionsFormCommandFactory
    {
        private readonly Func<IEditConditionFunctionsForm, AddConditionFunctionListBoxItemCommand> _getAddConditionFunctionListBoxItemCommand;
        private readonly Func<IEditConditionFunctionsForm, EditConditionFunctionsFormCopyXmlCommand> _getEditConditionFunctionsFormCopyXmlCommand;
        private readonly Func<IEditConditionFunctionsForm, EditConditionFunctionsFormEditXmlCommand> _getEditConditionFunctionsFormEditXmlCommand;
        private readonly Func<IEditConditionFunctionsForm, UpdateConditionFunctionListBoxItemCommand> _getUpdateConditionFunctionListBoxItemCommand;

        public EditConditionFunctionsFormCommandFactory(
            Func<IEditConditionFunctionsForm, AddConditionFunctionListBoxItemCommand> getAddConditionFunctionListBoxItemCommand,
            Func<IEditConditionFunctionsForm, EditConditionFunctionsFormCopyXmlCommand> getEditConditionFunctionsFormCopyXmlCommand,
            Func<IEditConditionFunctionsForm, EditConditionFunctionsFormEditXmlCommand> getEditConditionFunctionsFormEditXmlCommand,
            Func<IEditConditionFunctionsForm, UpdateConditionFunctionListBoxItemCommand> getUpdateConditionFunctionListBoxItemCommand)
        {
            _getAddConditionFunctionListBoxItemCommand = getAddConditionFunctionListBoxItemCommand;
            _getEditConditionFunctionsFormCopyXmlCommand = getEditConditionFunctionsFormCopyXmlCommand;
            _getEditConditionFunctionsFormEditXmlCommand = getEditConditionFunctionsFormEditXmlCommand;
            _getUpdateConditionFunctionListBoxItemCommand = getUpdateConditionFunctionListBoxItemCommand;
        }

        public AddConditionFunctionListBoxItemCommand GetAddConditionFunctionListBoxItemCommand(IEditConditionFunctionsForm editConditionFunctionsForm)
            => _getAddConditionFunctionListBoxItemCommand(editConditionFunctionsForm);

        public EditConditionFunctionsFormCopyXmlCommand GetEditConditionFunctionsFormCopyXmlCommand(IEditConditionFunctionsForm editConditionFunctionsForm)
            => _getEditConditionFunctionsFormCopyXmlCommand(editConditionFunctionsForm);

        public EditConditionFunctionsFormEditXmlCommand GetEditConditionFunctionsFormEditXmlCommand(IEditConditionFunctionsForm editConditionFunctionsForm)
            => _getEditConditionFunctionsFormEditXmlCommand(editConditionFunctionsForm);

        public UpdateConditionFunctionListBoxItemCommand GetUpdateConditionFunctionListBoxItemCommand(IEditConditionFunctionsForm editConditionFunctionsForm)
            => _getUpdateConditionFunctionListBoxItemCommand(editConditionFunctionsForm);
    }
}

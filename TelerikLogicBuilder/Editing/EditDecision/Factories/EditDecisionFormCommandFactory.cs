using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories
{
    internal class EditDecisionFormCommandFactory : IEditDecisionFormCommandFactory
    {
        private readonly Func<IEditDecisionForm, AddDecisionFunctionListBoxItemCommand> _getAddDecisionFunctionListBoxItemCommand;
        private readonly Func<IEditDecisionForm, EditDecisionFormCopyXmlCommand> _getEditDecisionFormCopyXmlCommand;
        private readonly Func<IEditDecisionForm, EditDecisionFormEditXmlCommand> _getEditDecisionFormEditXmlCommand;
        private readonly Func<IEditDecisionForm, UpdateDecisionFunctionListBoxItemCommand> _getUpdateDecisionFunctionListBoxItemCommand;

        public EditDecisionFormCommandFactory(
            Func<IEditDecisionForm, AddDecisionFunctionListBoxItemCommand> getAddDecisionFunctionListBoxItemCommand,
            Func<IEditDecisionForm, EditDecisionFormCopyXmlCommand> getEditDecisionFormCopyXmlCommand,
            Func<IEditDecisionForm, EditDecisionFormEditXmlCommand> getEditDecisionFormEditXmlCommand,
            Func<IEditDecisionForm, UpdateDecisionFunctionListBoxItemCommand> getUpdateDecisionFunctionListBoxItemCommand)
        {
            _getAddDecisionFunctionListBoxItemCommand = getAddDecisionFunctionListBoxItemCommand;
            _getEditDecisionFormCopyXmlCommand = getEditDecisionFormCopyXmlCommand;
            _getEditDecisionFormEditXmlCommand = getEditDecisionFormEditXmlCommand;
            _getUpdateDecisionFunctionListBoxItemCommand = getUpdateDecisionFunctionListBoxItemCommand;
        }

        public AddDecisionFunctionListBoxItemCommand GetAddDecisionFunctionListBoxItemCommand(IEditDecisionForm editDecisionForm)
            => _getAddDecisionFunctionListBoxItemCommand(editDecisionForm);

        public EditDecisionFormCopyXmlCommand GetEditDecisionFormCopyXmlCommand(IEditDecisionForm editDecisionForm)
            => _getEditDecisionFormCopyXmlCommand(editDecisionForm);

        public EditDecisionFormEditXmlCommand GetEditDecisionFormEditXmlCommand(IEditDecisionForm editDecisionForm)
            => _getEditDecisionFormEditXmlCommand(editDecisionForm);

        public UpdateDecisionFunctionListBoxItemCommand GetUpdateDecisionFunctionListBoxItemCommand(IEditDecisionForm editDecisionForm)
            => _getUpdateDecisionFunctionListBoxItemCommand(editDecisionForm);
    }
}

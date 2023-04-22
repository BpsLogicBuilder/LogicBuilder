using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories
{
    internal class EditDecisionsFormCommandFactory : IEditDecisionsFormCommandFactory
    {
        private readonly Func<IEditDecisionsForm, AddDecisionListBoxItemCommand> _getAddDecisionListBoxItemCommand;
        private readonly Func<IEditDecisionsForm, EditDecisionCommand> _getEditDecisionCommand;
        private readonly Func<IEditDecisionsForm, EditDecisionsFormCopyXmlCommand> _getEditDecisionsFormCopyXmlCommand;
        private readonly Func<IEditDecisionsForm, EditDecisionsFormEditXmlCommand> _getEditDecisionsFormEditXmlCommand;
        private readonly Func<IEditDecisionsForm, UpdateDecisionListBoxItemCommand> _getUpdateDecisionListBoxItemCommand;

        public EditDecisionsFormCommandFactory(
            Func<IEditDecisionsForm, AddDecisionListBoxItemCommand> getAddDecisionListBoxItemCommand,
            Func<IEditDecisionsForm, EditDecisionCommand> getEditDecisionCommand,
            Func<IEditDecisionsForm, EditDecisionsFormCopyXmlCommand> getEditDecisionsFormCopyXmlCommand,
            Func<IEditDecisionsForm, EditDecisionsFormEditXmlCommand> getEditDecisionsFormEditXmlCommand,
            Func<IEditDecisionsForm, UpdateDecisionListBoxItemCommand> getUpdateDecisionListBoxItemCommand)
        {
            _getAddDecisionListBoxItemCommand = getAddDecisionListBoxItemCommand;
            _getEditDecisionCommand = getEditDecisionCommand;
            _getEditDecisionsFormCopyXmlCommand = getEditDecisionsFormCopyXmlCommand;
            _getEditDecisionsFormEditXmlCommand = getEditDecisionsFormEditXmlCommand;
            _getUpdateDecisionListBoxItemCommand = getUpdateDecisionListBoxItemCommand;
        }

        public AddDecisionListBoxItemCommand GetAddDecisionListBoxItemCommand(IEditDecisionsForm editDecisionsForm)
            => _getAddDecisionListBoxItemCommand(editDecisionsForm);

        public EditDecisionCommand GetEditDecisionCommand(IEditDecisionsForm editDecisionsForm)
            => _getEditDecisionCommand(editDecisionsForm);

        public EditDecisionsFormCopyXmlCommand GetEditDecisionsFormCopyXmlCommand(IEditDecisionsForm editDecisionsForm)
            => _getEditDecisionsFormCopyXmlCommand(editDecisionsForm);

        public EditDecisionsFormEditXmlCommand GetEditDecisionsFormEditXmlCommand(IEditDecisionsForm editDecisionsForm)
            => _getEditDecisionsFormEditXmlCommand(editDecisionsForm);

        public UpdateDecisionListBoxItemCommand GetUpdateDecisionListBoxItemCommand(IEditDecisionsForm editDecisionsForm)
            => _getUpdateDecisionListBoxItemCommand(editDecisionsForm);
    }
}

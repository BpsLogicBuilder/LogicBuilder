using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories
{
    internal interface IEditDecisionsFormCommandFactory
    {
        AddDecisionListBoxItemCommand GetAddDecisionListBoxItemCommand(IEditDecisionsForm editDecisionsForm);
        EditDecisionCommand GetEditDecisionCommand(IEditDecisionsForm editDecisionsForm);
        EditDecisionsFormCopyXmlCommand GetEditDecisionsFormCopyXmlCommand(IEditDecisionsForm editDecisionsForm);
        EditDecisionsFormEditXmlCommand GetEditDecisionsFormEditXmlCommand(IEditDecisionsForm editDecisionsForm);
        UpdateDecisionListBoxItemCommand GetUpdateDecisionListBoxItemCommand(IEditDecisionsForm editDecisionsForm);
    }
}

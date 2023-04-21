using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories
{
    internal interface IEditDecisionFormCommandFactory
    {
        AddDecisionFunctionListBoxItemCommand GetAddDecisionFunctionListBoxItemCommand(IEditDecisionForm editDecisionForm);
        EditDecisionFormCopyXmlCommand GetEditDecisionFormCopyXmlCommand(IEditDecisionForm editDecisionForm);
        EditDecisionFormEditXmlCommand GetEditDecisionFormEditXmlCommand(IEditDecisionForm editDecisionForm);
        UpdateDecisionFunctionListBoxItemCommand GetUpdateDecisionFunctionListBoxItemCommand(IEditDecisionForm editDecisionForm);
    }
}

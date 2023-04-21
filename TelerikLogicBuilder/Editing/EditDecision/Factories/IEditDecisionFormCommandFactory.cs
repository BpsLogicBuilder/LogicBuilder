using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories
{
    internal interface IEditDecisionFormCommandFactory
    {
        AddDecisionFunctionListBoxItemCommand GetAddDecisionFunctionListBoxItemCommand(IEditDecisionForm editDecisionForm);
        EditDecisionFormCopyXmlCommand GetEditDecisionFormCopyXmlCommand(IEditDecisionForm editDecisionForm);
        EditDecisionFormEditXmlCommand GetEditDecisionFormEditXmlCommand(IEditDecisionForm editDecisionForm);
        UpdateDecisionFunctionListBoxItemCommand GetUpdateDecisionFunctionListBoxItemCommand(IEditDecisionForm editDecisionForm);
    }
}

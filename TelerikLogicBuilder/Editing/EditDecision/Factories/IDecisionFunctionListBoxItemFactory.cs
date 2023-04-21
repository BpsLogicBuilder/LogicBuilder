using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories
{
    internal interface IDecisionFunctionListBoxItemFactory
    {
        IDecisionFunctionListBoxItem GetDecisionFunctionListBoxItem(string visibleText,
            string hiddenText,
            IApplicationControl applicationControl);
    }
}

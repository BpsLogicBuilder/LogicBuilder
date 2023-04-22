using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories
{
    internal interface IDecisionListBoxItemFactory
    {
        IDecisionListBoxItem GetDecisionListBoxItem(string visibleText,
            string hiddenText,
            IApplicationControl applicationControl);
    }
}

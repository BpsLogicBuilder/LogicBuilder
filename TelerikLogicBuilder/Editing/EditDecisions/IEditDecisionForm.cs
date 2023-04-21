using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions
{
    internal interface IEditDecisionForm : IApplicationForm
    {
        IEditingControl? CurrentEditingControl { get; }
        string DecisionXml { get; }
        string DecisionVisibleText { get; }
        IEditConditionFunctionControl EditConditionFunctionControl { get; }
        IRadListBoxManager<IDecisionFunctionListBoxItem> RadListBoxManager { get; }
        void UpdateDecisionFunctionsList(string? xmlString);
    }
}

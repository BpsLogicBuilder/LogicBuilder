using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision
{
    internal interface IDecisionFunctionListBoxItem : IListBoxManageable
    {
        string HiddenText { get; }
        string VisibleText { get; }
    }
}

using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions
{
    internal interface IDecisionFunctionListBoxItem : IListBoxManageable
    {
        string HiddenText { get; }
        string VisibleText { get; }
    }
}

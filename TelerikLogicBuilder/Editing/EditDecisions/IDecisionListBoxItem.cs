using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions
{
    internal interface IDecisionListBoxItem : IListBoxManageable
    {
        string HiddenText { get; }
        string VisibleText { get; }
    }
}

using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions
{
    internal interface IFunctionListBoxItem : IListBoxManageable
    {
        string HiddenText { get; }
        string VisibleText { get; }
    }
}

using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList
{
    internal interface IObjectListBoxItem : IListBoxManageable
    {
        string HiddenText { get; }
        string VisibleText { get; }
    }
}

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories
{
    internal interface IEditVariableViewControlFactory
    {
        IEditVariableDropdownViewControl GetEditVariableDropdownViewControl(IEditVariableControl editVariableControl);
        IEditVariableListViewControl GetEditVariableListViewControl(IEditVariableControl editVariableControl);
        IEditVariableTreeViewControl GetEditVariableTreeViewControl(IEditVariableControl editVariableControl);
    }
}

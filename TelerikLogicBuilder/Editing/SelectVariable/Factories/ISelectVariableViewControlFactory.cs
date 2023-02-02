namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories
{
    internal interface ISelectVariableViewControlFactory
    {
        ISelectVariableDropDownViewControl GetSelectVariableDropdownViewControl(ISelectVariableControl editVariableControl);
        ISelectVariableListViewControl GetSelectVariableListViewControl(ISelectVariableControl editVariableControl);
        ISelectVariableTreeViewControl GetSelectVariableTreeViewControl(ISelectVariableControl editVariableControl);
    }
}

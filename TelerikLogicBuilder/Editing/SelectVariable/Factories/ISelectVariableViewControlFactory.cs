namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable.Factories
{
    internal interface ISelectVariableViewControlFactory
    {
        ISelectVariableDropDownViewControl GetSelectVariableDropdownViewControl(ISelectVariableControl selectVariableControl);
        ISelectVariableListViewControl GetSelectVariableListViewControl(ISelectVariableControl selectVariableControl);
        ISelectVariableTreeViewControl GetSelectVariableTreeViewControl(ISelectVariableControl selectVariableControl);
    }
}

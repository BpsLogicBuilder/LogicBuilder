namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction.Factories
{
    internal interface ISelectFunctionViewControlFactory
    {
        ISelectFunctionDropDownViewControl GetSelectFunctionDropdownViewControl(ISelectFunctionControl selectFunctionControl);
        ISelectFunctionListViewControl GetSelectFunctionListViewControl(ISelectFunctionControl selectFunctionControl);
        ISelectFunctionTreeViewControl GetSelectFunctionTreeViewControl(ISelectFunctionControl selectFunctionControl);
    }
}

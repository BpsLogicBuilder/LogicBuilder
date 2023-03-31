namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment.Factories
{
    internal interface ISelectFragmentViewControlFactory
    {
        ISelectFragmentDropDownViewControl GetSelectFragmentDropdownViewControl(ISelectFragmentControl selectFragmentControl);
        ISelectFragmentListViewControl GetSelectFragmentListViewControl(ISelectFragmentControl selectFragmentControl);
        ISelectFragmentTreeViewControl GetSelectFragmentTreeViewControl(ISelectFragmentControl selectFragmentControl);
    }
}

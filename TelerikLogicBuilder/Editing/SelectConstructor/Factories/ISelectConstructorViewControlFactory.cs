namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor.Factories
{
    internal interface ISelectConstructorViewControlFactory
    {
        ISelectConstructorDropDownViewControl GetSelectConstructorDropdownViewControl(ISelectConstructorControl selectConstructorControl);
        ISelectConstructorListViewControl GetSelectConstructorListViewControl(ISelectConstructorControl selectConstructorControl);
        ISelectConstructorTreeViewControl GetSelectConstructorTreeViewControl(ISelectConstructorControl selectConstructorControl);
    }
}

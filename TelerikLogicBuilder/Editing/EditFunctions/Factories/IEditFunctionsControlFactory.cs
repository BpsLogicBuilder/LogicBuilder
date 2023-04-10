namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal interface IEditFunctionsControlFactory
    {
        IEditVoidFunctionControl GetEditVoidFunctionControl(IEditFunctionsForm editFunctionsForm);
    }
}

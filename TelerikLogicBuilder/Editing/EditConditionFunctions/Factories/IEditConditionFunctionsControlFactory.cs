namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories
{
    internal interface IEditConditionFunctionsControlFactory
    {
        IEditConditionFunctionControl GetEditConditionFunctionControl(IEditConditionFunctionsForm editConditionFunctionsFormForm);
    }
}

using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories
{
    internal interface IEditConditionFunctionCommandFactory
    {
        SelectConditionFunctionCommand GetSelectConditionFunctionCommand(IEditConditionFunctionControl editConditionFunctionControl);
    }
}

using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories
{
    internal interface IEditConditionFunctionCommandFactory
    {
        SelectConditionFunctionCommand GetSelectConditionFunctionCommand(IEditConditionFunctionControl editConditionFunctionControl);
    }
}

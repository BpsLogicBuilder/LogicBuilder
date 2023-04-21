using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories
{
    internal interface IEditConditionFunctionsControlFactory
    {
        IEditConditionFunctionControl GetEditConditionFunctionControl(IApplicationForm parentForm);
    }
}

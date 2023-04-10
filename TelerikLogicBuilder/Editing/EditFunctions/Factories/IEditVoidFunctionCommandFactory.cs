using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal interface IEditVoidFunctionCommandFactory
    {
        SelectVoidFunctionCommand GetSelectVoidFunctionCommand(IEditVoidFunctionControl editVoidFunctionControl);
    }
}

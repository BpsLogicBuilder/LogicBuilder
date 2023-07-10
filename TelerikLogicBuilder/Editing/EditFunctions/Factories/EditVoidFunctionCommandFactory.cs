using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal class EditVoidFunctionCommandFactory : IEditVoidFunctionCommandFactory
    {
        public SelectVoidFunctionCommand GetSelectVoidFunctionCommand(IEditVoidFunctionControl editVoidFunctionControl)
            => new
            (
                editVoidFunctionControl
            );
    }
}

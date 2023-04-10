using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal class EditVoidFunctionCommandFactory : IEditVoidFunctionCommandFactory
    {
        private readonly Func<IEditVoidFunctionControl, SelectVoidFunctionCommand> _getSelectVoidFunctionCommand;

        public EditVoidFunctionCommandFactory(
            Func<IEditVoidFunctionControl, SelectVoidFunctionCommand> getSelectVoidFunctionCommand)
        {
            _getSelectVoidFunctionCommand = getSelectVoidFunctionCommand;
        }

        public SelectVoidFunctionCommand GetSelectVoidFunctionCommand(IEditVoidFunctionControl editVoidFunctionControl)
            => _getSelectVoidFunctionCommand(editVoidFunctionControl);
    }
}

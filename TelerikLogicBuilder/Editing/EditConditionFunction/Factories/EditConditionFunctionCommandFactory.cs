using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories
{
    internal class EditConditionFunctionCommandFactory : IEditConditionFunctionCommandFactory
    {
        private readonly Func<IEditConditionFunctionControl, SelectConditionFunctionCommand> _getSelectConditionFunctionCommand;

        public EditConditionFunctionCommandFactory(
            Func<IEditConditionFunctionControl, SelectConditionFunctionCommand> getSelectConditionFunctionCommand)
        {
            _getSelectConditionFunctionCommand = getSelectConditionFunctionCommand;
        }

        public SelectConditionFunctionCommand GetSelectConditionFunctionCommand(IEditConditionFunctionControl editConditionFunctionControl)
            => _getSelectConditionFunctionCommand(editConditionFunctionControl);
    }
}

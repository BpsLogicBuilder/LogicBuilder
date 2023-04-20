using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories
{
    internal class EditConditionFunctionsControlFactory : IEditConditionFunctionsControlFactory
    {
        private readonly Func<IEditConditionFunctionsForm, IEditConditionFunctionControl> _getEditConditionFunctionControl;

        public EditConditionFunctionsControlFactory(
            Func<IEditConditionFunctionsForm, IEditConditionFunctionControl> getEditConditionFunctionControl)
        {
            _getEditConditionFunctionControl = getEditConditionFunctionControl;
        }

        public IEditConditionFunctionControl GetEditConditionFunctionControl(IEditConditionFunctionsForm editConditionFunctionsFormForm)
            => _getEditConditionFunctionControl(editConditionFunctionsFormForm);
    }
}

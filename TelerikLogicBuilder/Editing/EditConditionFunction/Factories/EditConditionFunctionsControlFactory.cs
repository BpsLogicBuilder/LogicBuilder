using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories
{
    internal class EditConditionFunctionsControlFactory : IEditConditionFunctionsControlFactory
    {
        private readonly Func<IApplicationForm, IEditConditionFunctionControl> _getEditConditionFunctionControl;

        public EditConditionFunctionsControlFactory(
            Func<IApplicationForm, IEditConditionFunctionControl> getEditConditionFunctionControl)
        {
            _getEditConditionFunctionControl = getEditConditionFunctionControl;
        }

        public IEditConditionFunctionControl GetEditConditionFunctionControl(IApplicationForm parentForm)
            => _getEditConditionFunctionControl(parentForm);
    }
}

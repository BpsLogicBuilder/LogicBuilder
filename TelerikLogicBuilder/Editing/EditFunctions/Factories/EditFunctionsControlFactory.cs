using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal class EditFunctionsControlFactory : IEditFunctionsControlFactory
    {
        private readonly Func<IEditFunctionsForm, IEditVoidFunctionControl> _getEditVoidFunctionControl;

        public EditFunctionsControlFactory(
            Func<IEditFunctionsForm, IEditVoidFunctionControl> getEditVoidFunctionControl)
        {
            _getEditVoidFunctionControl = getEditVoidFunctionControl;
        }

        public IEditVoidFunctionControl GetEditVoidFunctionControl(IEditFunctionsForm editFunctionsForm)
            => _getEditVoidFunctionControl(editFunctionsForm);
    }
}

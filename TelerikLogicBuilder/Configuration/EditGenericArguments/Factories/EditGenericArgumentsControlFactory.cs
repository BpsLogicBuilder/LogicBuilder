using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Factories
{
    internal class EditGenericArgumentsControlFactory : IEditGenericArgumentsControlFactory
    {
        private readonly Func<IEditGenericArgumentsForm, IEditGenericArgumentsControl> _getEditGenericArgumentsControl;

        public EditGenericArgumentsControlFactory(
            Func<IEditGenericArgumentsForm, IEditGenericArgumentsControl> getEditGenericArgumentsControl)
        {
            _getEditGenericArgumentsControl = getEditGenericArgumentsControl;
        }

        public IEditGenericArgumentsControl GetEditGenericArgumentsControl(IEditGenericArgumentsForm editGenericArgumentsForm)
            => _getEditGenericArgumentsControl(editGenericArgumentsForm);
    }
}

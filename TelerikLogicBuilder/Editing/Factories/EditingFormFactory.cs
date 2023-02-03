using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormFactory : IEditingFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<Type, ISelectConstructorForm> _getSelectConstructorForm;
        private readonly Func<Type, ISelectVariableForm> _getSelectVariableForm;

        public EditingFormFactory(
            Func<Type, ISelectConstructorForm> getSelectConstructorForm,
            Func<Type, ISelectVariableForm> getSelectVariableForm)
        {
            _getSelectConstructorForm = getSelectConstructorForm;
            _getSelectVariableForm = getSelectVariableForm;
        }

        public ISelectConstructorForm GetSelectConstructorForm(Type assignedTo)
        {
            _scopedService = _getSelectConstructorForm(assignedTo);
            return (ISelectConstructorForm)_scopedService;
        }

        public ISelectVariableForm GetSelectVariableForm(Type assignedTo)
        {
            _scopedService = _getSelectVariableForm(assignedTo);
            return (ISelectVariableForm)_scopedService;
        }

        public void Dispose()
        {
            _scopedService?.Dispose();
        }
    }
}

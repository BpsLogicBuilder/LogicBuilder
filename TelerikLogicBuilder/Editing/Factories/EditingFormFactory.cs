using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormFactory : IEditingFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<Type, ISelectVariableForm> _getSelectVariableForm;

        public EditingFormFactory(
            Func<Type, ISelectVariableForm> getSelectVariableForm)
        {
            _getSelectVariableForm = getSelectVariableForm;
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

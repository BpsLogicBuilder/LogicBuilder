using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class SelectEditingControlFactory : ISelectEditingControlFactory
    {
        private readonly Func<IEditingForm, Type, ISelectVariableControl> _getSelectVariableControl;

        public SelectEditingControlFactory(
            Func<IEditingForm, Type, ISelectVariableControl> getSelectVariableControl)
        {
            _getSelectVariableControl = getSelectVariableControl;
        }

        public ISelectVariableControl GetSelectVariableControl(IEditingForm editingForm, Type assignedTo)
            => _getSelectVariableControl(editingForm, assignedTo);
    }
}

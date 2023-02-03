using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class SelectEditingControlFactory : ISelectEditingControlFactory
    {
        private readonly Func<IEditingForm, Type, ISelectConstructorControl> _getSelectConstructorControl;
        private readonly Func<IEditingForm, Type, ISelectVariableControl> _getSelectVariableControl;

        public SelectEditingControlFactory(
            Func<IEditingForm, Type, ISelectConstructorControl> getSelectConstructorControl,
            Func<IEditingForm, Type, ISelectVariableControl> getSelectVariableControl)
        {
            _getSelectConstructorControl = getSelectConstructorControl;
            _getSelectVariableControl = getSelectVariableControl;
        }

        public ISelectConstructorControl GetSelectConstructorControl(IEditingForm editingForm, Type assignedTo)
            => _getSelectConstructorControl(editingForm, assignedTo);

        public ISelectVariableControl GetSelectVariableControl(IEditingForm editingForm, Type assignedTo)
            => _getSelectVariableControl(editingForm, assignedTo);
    }
}

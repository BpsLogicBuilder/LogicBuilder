using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class SelectEditingControlFactory : ISelectEditingControlFactory
    {
        private readonly Func<IEditingForm, Type, ISelectConstructorControl> _getSelectConstructorControl;
        private readonly Func<ISelectFunctionForm, Type, ISelectFunctionControl> _getSelectFunctionControl;
        private readonly Func<IEditingForm, Type, ISelectVariableControl> _getSelectVariableControl;

        public SelectEditingControlFactory(
            Func<IEditingForm, Type, ISelectConstructorControl> getSelectConstructorControl,
            Func<ISelectFunctionForm, Type, ISelectFunctionControl> getSelectFunctionControl,
            Func<IEditingForm, Type, ISelectVariableControl> getSelectVariableControl)
        {
            _getSelectConstructorControl = getSelectConstructorControl;
            _getSelectFunctionControl = getSelectFunctionControl;
            _getSelectVariableControl = getSelectVariableControl;
        }

        public ISelectConstructorControl GetSelectConstructorControl(IEditingForm editingForm, Type assignedTo)
            => _getSelectConstructorControl(editingForm, assignedTo);

        public ISelectFunctionControl GetSelectFunctionControl(ISelectFunctionForm selectFunctionForm, Type assignedTo)
            => _getSelectFunctionControl(selectFunctionForm, assignedTo);

        public ISelectVariableControl GetSelectVariableControl(IEditingForm editingForm, Type assignedTo)
            => _getSelectVariableControl(editingForm, assignedTo);
    }
}

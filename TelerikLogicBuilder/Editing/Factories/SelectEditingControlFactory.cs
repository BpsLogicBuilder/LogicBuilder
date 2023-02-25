using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class SelectEditingControlFactory : ISelectEditingControlFactory
    {
        private readonly Func<ISelectConstructorForm, Type, ISelectConstructorControl> _getSelectConstructorControl;
        private readonly Func<ISelectFunctionForm, Type, ISelectFunctionControl> _getSelectFunctionControl;
        private readonly Func<IEditingForm, Type, IEditVariableControl> _getEditVariableControl;

        public SelectEditingControlFactory(
            Func<ISelectConstructorForm, Type, ISelectConstructorControl> getSelectConstructorControl,
            Func<ISelectFunctionForm, Type, ISelectFunctionControl> getSelectFunctionControl,
            Func<IEditingForm, Type, IEditVariableControl> getEditVariableControl)
        {
            _getSelectConstructorControl = getSelectConstructorControl;
            _getSelectFunctionControl = getSelectFunctionControl;
            _getEditVariableControl = getEditVariableControl;
        }

        public ISelectConstructorControl GetSelectConstructorControl(ISelectConstructorForm selectConstructorForm, Type assignedTo)
            => _getSelectConstructorControl(selectConstructorForm, assignedTo);

        public ISelectFunctionControl GetSelectFunctionControl(ISelectFunctionForm selectFunctionForm, Type assignedTo)
            => _getSelectFunctionControl(selectFunctionForm, assignedTo);

        public IEditVariableControl GetEditVariableControl(IEditingForm editingForm, Type assignedTo)
            => _getEditVariableControl(editingForm, assignedTo);
    }
}

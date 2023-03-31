using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class ConfiguredItemControlFactory : IConfiguredItemControlFactory
    {
        private readonly Func<ISelectConstructorForm, Type, ISelectConstructorControl> _getSelectConstructorControl;
        private readonly Func<ISelectFragmentForm, ISelectFragmentControl> _getSelectFragmentControl;
        private readonly Func<ISelectFunctionForm, Type, ISelectFunctionControl> _getSelectFunctionControl;

        public ConfiguredItemControlFactory(
            Func<ISelectConstructorForm, Type, ISelectConstructorControl> getSelectConstructorControl,
            Func<ISelectFragmentForm, ISelectFragmentControl> getSelectFragmentControl,
            Func<ISelectFunctionForm, Type, ISelectFunctionControl> getSelectFunctionControl)
        {
            _getSelectConstructorControl = getSelectConstructorControl;
            _getSelectFragmentControl = getSelectFragmentControl;
            _getSelectFunctionControl = getSelectFunctionControl;
        }

        public ISelectConstructorControl GetSelectConstructorControl(ISelectConstructorForm selectConstructorForm, Type assignedTo)
            => _getSelectConstructorControl(selectConstructorForm, assignedTo);

        public ISelectFragmentControl GetSelectFragmentControl(ISelectFragmentForm selectFragmentForm)
            => _getSelectFragmentControl(selectFragmentForm);

        public ISelectFunctionControl GetSelectFunctionControl(ISelectFunctionForm selectFunctionForm, Type assignedTo)
            => _getSelectFunctionControl(selectFunctionForm, assignedTo);
    }
}

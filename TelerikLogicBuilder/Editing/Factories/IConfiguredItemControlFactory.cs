using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IConfiguredItemControlFactory
    {
        ISelectConstructorControl GetSelectConstructorControl(ISelectConstructorForm selectConstructorForm, Type assignedTo);
        ISelectFragmentControl GetSelectFragmentControl(ISelectFragmentForm selectFragmentForm);
        ISelectFunctionControl GetSelectFunctionControl(ISelectFunctionForm selectFunctionForm, Type assignedTo);
    }
}

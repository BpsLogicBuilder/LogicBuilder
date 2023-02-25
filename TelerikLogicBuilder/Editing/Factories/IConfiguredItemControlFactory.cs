using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IConfiguredItemControlFactory
    {
        ISelectConstructorControl GetSelectConstructorControl(ISelectConstructorForm selectConstructorForm, Type assignedTo);
        ISelectFunctionControl GetSelectFunctionControl(ISelectFunctionForm selectFunctionForm, Type assignedTo);
    }
}

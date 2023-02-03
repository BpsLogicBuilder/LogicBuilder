using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface ISelectEditingControlFactory
    {
        ISelectConstructorControl GetSelectConstructorControl(IEditingForm editingForm, Type assignedTo);
        ISelectVariableControl GetSelectVariableControl(IEditingForm editingForm, Type assignedTo);
    }
}

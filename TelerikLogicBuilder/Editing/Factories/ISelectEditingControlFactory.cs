using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface ISelectEditingControlFactory
    {
        ISelectVariableControl GetSelectVariableControl(IEditingForm editingForm, Type assignedTo);
    }
}

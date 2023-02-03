using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingFormFactory : IDisposable
    {
        ISelectConstructorForm GetSelectConstructorForm(Type assignedTo);
        ISelectVariableForm GetSelectVariableForm(Type assignedTo);
    }
}

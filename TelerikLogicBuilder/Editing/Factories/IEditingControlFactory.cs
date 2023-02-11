using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingControlFactory
    {
        IEditConstructorControl GetEditConstructorControl(IEditingForm editingForm, Constructor constructor, Type assignedTo);
    }
}

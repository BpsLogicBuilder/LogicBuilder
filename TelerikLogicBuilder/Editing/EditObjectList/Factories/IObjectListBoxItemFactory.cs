using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Factories
{
    internal interface IObjectListBoxItemFactory
    {
        IObjectListBoxItem GetParameterObjectListBoxItem(
            string visibleText,
            string hiddenText,
            Type assignedTo,
            IApplicationForm applicationForm,
            ListParameterInputStyle listControl);

        IObjectListBoxItem GetVariableObjectListBoxItem(
            string visibleText,
            string hiddenText,
            Type assignedTo,
            IApplicationForm applicationForm,
            ListVariableInputStyle listControl);
    }
}

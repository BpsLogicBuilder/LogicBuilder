using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.Factories
{
    internal interface ILiteralListBoxItemFactory
    {
        ILiteralListBoxItem GetParameterLiteralListBoxItem(
            string visibleText,
            string hiddenText,
            Type assignedTo,
            IApplicationForm applicationForm,
            ListParameterInputStyle listControl);

        ILiteralListBoxItem GetVariableLiteralListBoxItem(
            string visibleText,
            string hiddenText,
            Type assignedTo,
            IApplicationForm applicationForm,
            ListVariableInputStyle listControl);
    }
}

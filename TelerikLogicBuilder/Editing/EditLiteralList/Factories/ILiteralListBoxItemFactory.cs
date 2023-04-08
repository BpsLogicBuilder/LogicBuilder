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
            IApplicationControl applicationControl,
            ListParameterInputStyle listControl);

        ILiteralListBoxItem GetVariableLiteralListBoxItem(
            string visibleText,
            string hiddenText,
            Type assignedTo,
            IApplicationControl applicationControl,
            ListVariableInputStyle listControl);
    }
}

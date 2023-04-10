using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal interface IFunctionListBoxItemFactory
    {
        IFunctionListBoxItem GetFunctionListBoxItem(
            string visibleText,
            string hiddenText,
            Type assignedTo,
            IApplicationControl applicationControl);
    }
}

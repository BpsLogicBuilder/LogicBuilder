using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal class FunctionListBoxItemFactory : IFunctionListBoxItemFactory
    {
        private readonly Func<string, string, Type, IApplicationControl, IFunctionListBoxItem> _getFunctionListBoxItem;

        public FunctionListBoxItemFactory(
            Func<string, string, Type, IApplicationControl, IFunctionListBoxItem> getFunctionListBoxItem)
        {
            _getFunctionListBoxItem = getFunctionListBoxItem;
        }

        public IFunctionListBoxItem GetFunctionListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationControl applicationControl)
            => _getFunctionListBoxItem(visibleText, hiddenText, assignedTo, applicationControl);
    }
}

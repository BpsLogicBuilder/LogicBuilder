using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Factories
{
    internal class ConditionFunctionListBoxItemFactory : IConditionFunctionListBoxItemFactory
    {
        private readonly Func<string, string, IApplicationControl, IConditionFunctionListBoxItem> _getBooleanFunctionListBoxItem;

        public ConditionFunctionListBoxItemFactory(
            Func<string, string, IApplicationControl, IConditionFunctionListBoxItem> getBooleanFunctionListBoxItem)
        {
            _getBooleanFunctionListBoxItem = getBooleanFunctionListBoxItem;
        }

        public IConditionFunctionListBoxItem GetConditionFunctionListBoxItem(string visibleText, string hiddenText, IApplicationControl applicationControl)
            => _getBooleanFunctionListBoxItem(visibleText, hiddenText, applicationControl);
    }
}

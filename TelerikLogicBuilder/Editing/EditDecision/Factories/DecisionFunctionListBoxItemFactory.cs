using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision.Factories
{
    internal class DecisionFunctionListBoxItemFactory : IDecisionFunctionListBoxItemFactory
    {
        private readonly Func<string, string, IApplicationControl, IDecisionFunctionListBoxItem> _getDecisionFunctionListBoxItem;

        public DecisionFunctionListBoxItemFactory(
            Func<string, string, IApplicationControl, IDecisionFunctionListBoxItem> getDecisionFunctionListBoxItem)
        {
            _getDecisionFunctionListBoxItem = getDecisionFunctionListBoxItem;
        }

        public IDecisionFunctionListBoxItem GetDecisionFunctionListBoxItem(string visibleText, string hiddenText, IApplicationControl applicationControl)
            => _getDecisionFunctionListBoxItem(visibleText, hiddenText, applicationControl);
    }
}

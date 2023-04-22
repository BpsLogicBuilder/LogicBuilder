using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories
{
    internal class DecisionListBoxItemFactory : IDecisionListBoxItemFactory
    {
        private readonly Func<string, string, IApplicationControl, IDecisionListBoxItem> _getDecisionListBoxItem;

        public DecisionListBoxItemFactory(
            Func<string, string, IApplicationControl, IDecisionListBoxItem> getDecisionListBoxItem)
        {
            _getDecisionListBoxItem = getDecisionListBoxItem;
        }

        public IDecisionListBoxItem GetDecisionListBoxItem(string visibleText, string hiddenText, IApplicationControl applicationControl)
            => _getDecisionListBoxItem(visibleText, hiddenText, applicationControl);
    }
}

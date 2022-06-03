using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator
{
    internal class BuildRulesResult
    {
        public BuildRulesResult(IList<ResultMessage> resultMessages, IList<RuleBag> rules, IDictionary<string, string> resourceStrings)
        {
            ResultMessages = resultMessages;
            Rules = rules;
            ResourceStrings = resourceStrings;
        }

        internal IList<ResultMessage> ResultMessages { get; }
        internal IList<RuleBag> Rules { get; }
        internal IDictionary<string, string> ResourceStrings { get; }
    }
}

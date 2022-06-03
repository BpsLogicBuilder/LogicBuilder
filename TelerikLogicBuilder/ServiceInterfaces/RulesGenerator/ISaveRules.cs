using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using LogicBuilder.Workflow.Activities.Rules;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface ISaveRules
    {
        void Save(string sourceFile, IList<RuleBag> rules, string documentTypeFolder, RuleChainingBehavior ruleChainingBehavior);
    }
}

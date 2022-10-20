using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface IShapeSetRuleBuilder
    {
        IList<RuleBag> GenerateRules();
    }
}

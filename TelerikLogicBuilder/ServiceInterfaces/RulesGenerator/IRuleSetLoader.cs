using LogicBuilder.Workflow.Activities.Rules;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IRuleSetLoader
    {
        RuleSet LoadRuleSet(string fullPath);
    }
}

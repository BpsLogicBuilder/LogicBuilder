using System.Collections.Generic;
using WorkflowRules = LogicBuilder.Workflow.Activities.Rules;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface ITableRowRuleBuilder
    {
        IList<WorkflowRules.Rule> GenerateRules();
    }
}

using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;
using System.Data;
using WorkflowRules = LogicBuilder.Workflow.Activities.Rules;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface ITableRowRuleBuilder
    {
        IList<WorkflowRules.Rule> GenerateRules(DataRow dataRow, string moduleName, int ruleCount, ApplicationTypeInfo application, IDictionary<string, string> resourceStrings);
    }
}

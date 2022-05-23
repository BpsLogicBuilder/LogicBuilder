using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface IRulesAssembler
    {
		Task AssembleResources(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource, IList<string> sourceFiles);
		Task AssembleRules(IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource, IList<string> sourceFiles);
		string SerializeRules(object rules, RuntimeType platForm);
		RuleSet DeserializeRuleSet(string ruleSetXmlDefinition, string fullPath);
	}
}

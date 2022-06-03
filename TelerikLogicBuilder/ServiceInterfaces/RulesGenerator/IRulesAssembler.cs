using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IRulesAssembler
    {
        Task AssembleResources(IList<string> sourceFiles, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource);
        Task AssembleRules(IList<string> sourceFiles, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource);
        string SerializeRules(object rules, RuntimeType platForm);
        RuleSet DeserializeRuleSet(string ruleSetXmlDefinition, string fullPath);
    }
}

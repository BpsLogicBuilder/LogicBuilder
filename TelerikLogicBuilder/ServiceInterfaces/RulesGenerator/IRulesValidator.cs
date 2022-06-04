using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using LogicBuilder.Workflow.Activities.Rules;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IRulesValidator
    {
        Task<IList<ResultMessage>> Validate(RuleSet ruleSet, ApplicationTypeInfo application, CancellationTokenSource cancellationTokenSource);
    }
}

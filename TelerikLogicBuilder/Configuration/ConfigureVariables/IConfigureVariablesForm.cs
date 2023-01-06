using ABIS.LogicBuilder.FlowBuilder.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables
{
    internal interface IConfigureVariablesForm : IConfigurationForm
    {
        IConfigureVariablesTreeNodeControl CurrentTreeNodeControl { get; }
        HelperStatus? HelperStatus { get; }
        IDictionary<string, VariableBase> VariablesDictionary { get; }
        HashSet<string> VariableNames { get; }
    }
}

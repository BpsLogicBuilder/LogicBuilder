using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables
{
    internal interface IConfigureVariablesForm : IConfigurationForm
    {
        IDictionary<string, VariableBase> VariablesDictionary { get; }
        HashSet<string> VariableNames { get; }
    }
}

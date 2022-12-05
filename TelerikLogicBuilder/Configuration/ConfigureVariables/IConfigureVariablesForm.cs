using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables
{
    internal interface IConfigureVariablesForm : IApplicationForm
    {
        IDictionary<string, string> ExpandedNodes { get; }
    }
}

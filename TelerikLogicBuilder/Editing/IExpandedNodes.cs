using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IExpandedNodes
    {
        IDictionary<string, string> ExpandedNodes { get; }
    }
}

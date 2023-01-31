using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable
{
    internal interface IEditVariableControl
    {
        IDictionary<string, string> ExpandedNodes { get; }
        bool ItemSelected { get; }
        string? VariableName { get; }
    }
}

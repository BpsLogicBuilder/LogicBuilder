using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable
{
    internal interface ISelectVariableControl : IEditingControl
    {
        event EventHandler? Changed;
        IDictionary<string, string> ExpandedNodes { get; }
        bool ItemSelected { get; }
        string? VariableName { get; }
        void SetVariable(string variableName);
    }
}

using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable
{
    internal interface ISelectVariableViewControl
    {
        event EventHandler? Changed;
        string VariableName { get; }
        bool ItemSelected { get; }
        void SelectVariable(string variableName);
    }
}

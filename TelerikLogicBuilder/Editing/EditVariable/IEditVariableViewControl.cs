using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable
{
    internal interface IEditVariableViewControl
    {
        event EventHandler? Changed;
        string VariableName { get; }
        bool ItemSelected { get; }
        void SelectVariable(string variableName);
    }
}

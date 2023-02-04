using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction
{
    internal interface ISelectFunctionViewControl
    {
        event EventHandler? Changed;
        string FunctionName { get; }
        bool ItemSelected { get; }
        void SelectFunction(string functionName);
    }
}

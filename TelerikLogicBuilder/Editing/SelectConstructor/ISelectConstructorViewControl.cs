using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor
{
    internal interface ISelectConstructorViewControl
    {
        event EventHandler? Changed;
        string ConstructorName { get; }
        bool ItemSelected { get; }
        void SelectConstructor(string constructorName);
    }
}

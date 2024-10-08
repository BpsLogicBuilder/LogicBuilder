﻿using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor
{
    internal interface ISelectConstructorForm : IApplicationForm
    {
        string ConstructorName { get; }
        void SetConstructor(string constructorName);
    }
}

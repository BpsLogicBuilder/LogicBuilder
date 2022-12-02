﻿using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue
{
    internal interface IConfigureLiteralListDefaultValueForm : IForm, IListBoxHostForm
    {
        IList<string> DefaultValueItems { get; }
        Type Type { get; }
        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}
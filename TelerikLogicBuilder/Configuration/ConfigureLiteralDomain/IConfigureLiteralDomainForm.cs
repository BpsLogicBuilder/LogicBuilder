using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain
{
    internal interface IConfigureLiteralDomainForm : IForm
    {
        IList<string> DomainItems { get; }
        Type Type { get; }
        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}

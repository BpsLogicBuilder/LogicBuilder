using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Forms
{
    internal interface IConfigureLoadAssemblyPaths : IDisposable
    {
        IList<string> Paths { get; }
        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}

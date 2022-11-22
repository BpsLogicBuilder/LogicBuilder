using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths
{
    internal interface IConfigureLoadAssemblyPathsForm : IDisposable
    {
        IList<string> Paths { get; }
        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}

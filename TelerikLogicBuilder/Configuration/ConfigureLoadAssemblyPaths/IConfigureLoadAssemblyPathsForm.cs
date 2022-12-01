using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths
{
    internal interface IConfigureLoadAssemblyPathsForm : IForm, IListBoxHostForm
    {
        IList<string> Paths { get; }
        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}

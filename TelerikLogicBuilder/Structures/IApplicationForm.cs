using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal interface IApplicationForm
    {
        event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;
        ApplicationTypeInfo Application { get; }
        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}

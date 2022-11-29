using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal interface IApplicationForm : IDisposable
    {
        event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;
        ApplicationTypeInfo Application { get; }
        DialogResult DialogResult { get; }
        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
        DialogResult ShowDialog(IWin32Window owner);
    }
}

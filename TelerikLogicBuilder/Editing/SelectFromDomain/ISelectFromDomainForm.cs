using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain
{
    internal interface ISelectFromDomainForm : IDisposable
    {
        string SelectedItem { get; }
        DialogResult DialogResult { get; }
        DialogResult ShowDialog(IWin32Window owner);
    }
}

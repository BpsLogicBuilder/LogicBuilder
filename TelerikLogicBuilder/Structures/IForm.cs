using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal interface IForm : IDisposable
    {
        DialogResult DialogResult { get; }
        DialogResult ShowDialog(IWin32Window owner);
    }
}

using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal interface IApplicationForm : IDisposable, ISetDialogMessages, IApplicationHostControl
    {
        DialogResult DialogResult { get; }
        DialogResult ShowDialog(IWin32Window owner);
    }
}

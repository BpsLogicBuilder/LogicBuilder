using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditJump
{
    internal interface IEditJumpForm : IShapeEditForm, IDisposable
    {
        DialogResult DialogResult { get; }
        DialogResult ShowDialog(IWin32Window owner);
    }
}

using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditPriority
{
    internal interface IEditPriorityForm : IShapeEditForm, IDisposable
    {
        DialogResult DialogResult { get; }
        DialogResult ShowDialog(IWin32Window owner);
    }
}

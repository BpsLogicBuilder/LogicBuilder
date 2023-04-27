using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditModuleShape
{
    internal interface IEditModuleShapeForm : IShapeEditForm, IDisposable
    {
        DialogResult DialogResult { get; }
        DialogResult ShowDialog(IWin32Window owner);
    }
}

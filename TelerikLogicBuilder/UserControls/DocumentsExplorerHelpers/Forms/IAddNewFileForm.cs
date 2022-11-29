using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
{
    internal interface IAddNewFileForm : IDisposable
    {
        DialogResult DialogResult { get; }
        string FileName { get; }
        DialogResult ShowDialog(IWin32Window owner);
    }
}

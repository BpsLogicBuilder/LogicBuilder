using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
{
    internal interface IInputBoxForm : IDisposable
    {
        DialogResult DialogResult { get; }
        string Input { get; set; }
        void SetTitles(string regularExpression, string caption, string prompt);
        DialogResult ShowDialog(IWin32Window owner);
    }
}

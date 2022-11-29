using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    internal interface ITextViewer : IDisposable
    {
        DialogResult DialogResult { get; }
        FormStartPosition StartPosition { get; set; }
        string Text { get; set; }
        void SetText(string[] lines);
        void SetText(string viewText);
        DialogResult ShowDialog();
        DialogResult ShowDialog(IWin32Window owner);
    }
}

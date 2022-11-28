using Microsoft.Office.Interop.Visio;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface IFindConfiguredItemInShape : IDisposable
    {
        Point Location { get; set; }
        FormStartPosition StartPosition { get; set; }
        void Setup(Document visioDocument);
        DialogResult ShowDialog(IWin32Window owner);
    }
}

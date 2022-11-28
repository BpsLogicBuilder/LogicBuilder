using System;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface IFindConfiguredItemInCell : IDisposable
    {
        Point Location { get; set; }
        FormStartPosition StartPosition { get; set; }
        void Setup(RadGridView dataGridView);
        DialogResult ShowDialog(IWin32Window owner);
    }
}

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface IFindReplaceTextInCell : IDisposable
    {
        Point Location { get; set; }
        FormStartPosition StartPosition { get; set; }
        void Setup(RadGridView dataGridView, DataSet dataSet);
        DialogResult ShowDialog(IWin32Window owner);
    }
}

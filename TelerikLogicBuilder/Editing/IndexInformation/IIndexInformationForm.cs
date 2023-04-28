using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.IndexInformation
{
    internal interface IIndexInformationForm : IDisposable
    {
        void SetIndexes(int pageIndex, int shapeIndex);
        DialogResult ShowDialog(IWin32Window owner);
    }
}

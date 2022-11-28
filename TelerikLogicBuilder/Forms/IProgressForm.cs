using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    internal interface IProgressForm : IDisposable
    {
        bool IsDisposed { get; }
        void Close();
        void Show(IWin32Window owner);
    }
}

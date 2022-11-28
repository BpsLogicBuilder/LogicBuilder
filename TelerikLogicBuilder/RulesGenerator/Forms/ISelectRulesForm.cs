using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ISelectRulesForm : IDisposable
    {
        DialogResult DialogResult { get; }
        bool IsDisposed { get; }
        IList<string> SourceFiles { get; }
        void SetTitle(string title);
        DialogResult ShowDialog(IWin32Window owner);
    }
}

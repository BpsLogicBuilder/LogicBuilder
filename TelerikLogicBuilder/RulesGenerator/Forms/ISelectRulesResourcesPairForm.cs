using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ISelectRulesResourcesPairForm : IDisposable
    {
        DialogResult DialogResult { get; }
        bool IsDisposed { get; }
        IList<RulesResourcesPair> SourceFiles { get; }
        void SetTitle(string title);
        DialogResult ShowDialog(IWin32Window owner);
    }
}

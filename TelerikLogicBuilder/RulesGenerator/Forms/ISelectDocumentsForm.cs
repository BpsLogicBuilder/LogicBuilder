using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ISelectDocumentsForm : IDisposable
    {
        DialogResult DialogResult { get; }
        IList<string> SourceFiles { get; }
        DialogResult ShowDialog(IWin32Window owner);
    }
}

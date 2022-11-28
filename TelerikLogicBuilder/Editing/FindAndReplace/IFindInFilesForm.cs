using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface IFindInFilesForm : IDisposable
    {
        SearchOptions SearchType { get; }

        string SearchPattern { get; }

        string SearchString { get; }

        bool MatchCase { get; }

        bool MatchWholeWord { get; }

        DialogResult ShowDialog(IWin32Window owner);
    }
}

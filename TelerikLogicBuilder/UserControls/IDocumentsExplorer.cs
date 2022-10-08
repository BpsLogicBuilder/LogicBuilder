using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IDocumentsExplorer
    {
        IList<RadTreeNode> CutTreeNodes { get; }
        IDictionary<string, string> DocumentNames { get; }
        IDictionary<string, string> ExpandedNodes { get; }
        RadTreeView TreeView { get; }

        void RefreshTreeView();
    }
}

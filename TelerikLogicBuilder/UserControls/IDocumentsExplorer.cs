using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IDocumentsExplorer
    {
        IList<RadTreeNode> CutTreeNodes { get; }
        DockStyle Dock { set; }
        IDictionary<string, string> DocumentNames { get; }
        IDictionary<string, string> ExpandedNodes { get; }
        RadTreeView TreeView { get; }

        void ClearProfile();
        void CreateProfile();
        void RefreshTreeView();
    }
}

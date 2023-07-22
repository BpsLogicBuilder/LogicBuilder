using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IRulesExplorer
    {
        DockStyle Dock { set; }
        IDictionary<string, string> ExpandedNodes { get; }
        RadTreeView TreeView { get; }

        void ClearProfile();
        void CreateProfile();
        void RefreshTreeView();
    }
}

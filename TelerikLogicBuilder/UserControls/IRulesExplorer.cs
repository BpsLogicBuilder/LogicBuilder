using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal interface IRulesExplorer
    {
        IDictionary<string, string> ExpandedNodes { get; }
        RadTreeView TreeView { get; }

        void RefreshTreeView();
    }
}

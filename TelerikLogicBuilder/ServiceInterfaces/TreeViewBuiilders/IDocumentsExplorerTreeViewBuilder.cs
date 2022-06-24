using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders
{
    internal interface IDocumentsExplorerTreeViewBuilder
    {
        void Build(RadTreeView treeView, DocumentExplorerErrorsList documentProfileErrors, IDictionary<string, string> documentNames, IDictionary<string, string> expandedNodes);
    }
}

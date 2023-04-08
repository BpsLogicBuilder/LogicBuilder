using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal interface IDataGraphEditingManager
    {
        void CreateContextMenus();
        void RequestDocumentUpdate(IEditingControl editingControl);
        void SetContextMenuState(ParametersDataTreeNode treeNode);
        void SetControlValues(ParametersDataTreeNode selectedNode);
        void UpdateXmlDocument(RadTreeNode selectedNode);
    }
}

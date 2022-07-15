using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal interface IDeleteOperations
    {
        void DeleteFile(RadTreeNode treeNode);
        void DeleteFolder(RadTreeNode treeNode);
        void DeleteProject(RadTreeNode treeNode);
    }
}

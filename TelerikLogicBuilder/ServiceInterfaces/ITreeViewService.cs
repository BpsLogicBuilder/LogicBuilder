using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITreeViewService
    {
        bool IsRootNode(RadTreeNode treeNode);
        bool IsFileNode(RadTreeNode treeNode);
        bool IsFolderNode(RadTreeNode treeNode);
        void MakeVisible(RadTreeNode treeNode);
    }
}

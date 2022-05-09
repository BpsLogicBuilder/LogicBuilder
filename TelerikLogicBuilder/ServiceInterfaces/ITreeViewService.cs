using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITreeViewService
    {
        ImageList ImageList { get; }
        bool IsRootNode(RadTreeNode treeNode);
        bool IsFolderNode(RadTreeNode treeNode);
        void MakeVisible(RadTreeNode treeNode);
    }
}

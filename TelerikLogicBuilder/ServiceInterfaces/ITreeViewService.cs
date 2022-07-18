using System.Drawing;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITreeViewService
    {
        bool IsRootNode(RadTreeNode treeNode);
        bool IsFileNode(RadTreeNode treeNode);
        bool IsFolderNode(RadTreeNode treeNode);
        void MakeVisible(RadTreeNode treeNode);
        void ScrollToPreviousPosition(RadTreeView treeView, Point point);
        void SelectTreeNode(RadTreeView treeView, string? nodeName);
    }
}

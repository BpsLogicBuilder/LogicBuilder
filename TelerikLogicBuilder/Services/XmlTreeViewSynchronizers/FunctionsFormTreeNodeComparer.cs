using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class FunctionsFormTreeNodeComparer : IFunctionsFormTreeNodeComparer
    {
        readonly ITreeViewService service;

        public FunctionsFormTreeNodeComparer(ITreeViewService service)
        {
            this.service = service;
        }

        public int Compare(RadTreeNode? treeNodeA, RadTreeNode? treeNodeB)
        {
            if (treeNodeA == null || treeNodeB == null)
                throw new InvalidOperationException("{6622B506-9C1D-46CA-9C8A-9DD8143D80A2}");

            if ((treeNodeA.ImageIndex == treeNodeB.ImageIndex) || (service.IsFolderNode(treeNodeA) && service.IsFolderNode(treeNodeB)))
                return string.Compare(treeNodeA.Text, treeNodeB.Text);
            else
                return service.IsMethodNode(treeNodeA) ? -1 : 1;//Only Functions and Folders exist at the same level for sorting purposes.
                                                                //Parameters may also have different indexes - however parameters will not be sorted.
        }
    }
}

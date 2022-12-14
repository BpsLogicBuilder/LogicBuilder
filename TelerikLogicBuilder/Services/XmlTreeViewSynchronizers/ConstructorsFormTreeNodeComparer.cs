using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ConstructorsFormTreeNodeComparer : IConstructorsFormTreeNodeComparer
    {
        readonly ITreeViewService service;

        public ConstructorsFormTreeNodeComparer(ITreeViewService service)
        {
            this.service = service;
        }

        public int Compare(RadTreeNode? treeNodeA, RadTreeNode? treeNodeB)
        {
            if (treeNodeA == null || treeNodeB == null)
                throw new InvalidOperationException("{CF0F2F42-1C34-4EB4-9D06-EB902F9DEB3D}");

            if ((treeNodeA.ImageIndex == treeNodeB.ImageIndex) || (service.IsFolderNode(treeNodeA) && service.IsFolderNode(treeNodeB)))
                return string.Compare(treeNodeA.Text, treeNodeB.Text);
            else
                return service.IsConstructorNode(treeNodeA) ? -1 : 1;//Only Constructors and Folders exist at the same level for sorting purposes.
                                                                     //Parameters may also have different indexes - however parameters will not be sorted.
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class VariablesFormTreeNodeComparer : IVariablesFormTreeNodeComparer
    {
        readonly ITreeViewService service;

        public VariablesFormTreeNodeComparer(ITreeViewService service)
        {
            this.service = service;
        }

        public int Compare(RadTreeNode? treeNodeA, RadTreeNode? treeNodeB)
        {
            if (treeNodeA == null || treeNodeB == null)
                throw new InvalidOperationException("{165AA5C2-5F5E-4F59-9286-9EBF73921786}");

            if ((service.IsVariableTypeNode(treeNodeA) && service.IsVariableTypeNode(treeNodeB)) || (service.IsFolderNode(treeNodeA) && service.IsFolderNode(treeNodeB)))
                return string.Compare(treeNodeA.Text, treeNodeB.Text);
            else
                return service.IsVariableTypeNode(treeNodeA) ? -1 : 1;
        }
    }
}

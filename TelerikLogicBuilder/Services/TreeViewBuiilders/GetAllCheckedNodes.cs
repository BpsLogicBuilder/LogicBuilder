using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class GetAllCheckedNodes : IGetAllCheckedNodes
    {
        private readonly ITreeViewService _treeViewService;

        public GetAllCheckedNodes(ITreeViewService treeViewService)
        {
            _treeViewService = treeViewService;
        }

        public IList<string> GetNames(RadTreeNode rootTreeNode)
        {
            List<RadTreeNode> checkedNodes = new();
            GetNames(rootTreeNode, checkedNodes);
            return checkedNodes.Select(n => n.Name).ToArray();
        }

        public IList<RadTreeNode> GetNodes(RadTreeNode rootTreeNode)
        {
            List<RadTreeNode> checkedNodes = new();
            GetNames(rootTreeNode, checkedNodes);
            return checkedNodes;
        }

        public void GetNames(RadTreeNode treeNode, IList<RadTreeNode> checkedNodes)
        {
            foreach (RadTreeNode node in treeNode.Nodes)
            {
                if (node.CheckState == ToggleState.On && !_treeViewService.IsFolderNode(node))
                {
                    checkedNodes.Add(node);
                }

                if (_treeViewService.IsFolderNode(node))
                    GetNames(node, checkedNodes);
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Collections.Generic;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class GetAllCheckedNodeNames : IGetAllCheckedNodeNames
    {
        private readonly ITreeViewService _treeViewService;

        public GetAllCheckedNodeNames(ITreeViewService treeViewService)
        {
            _treeViewService = treeViewService;
        }

        public IList<string> GetNames(RadTreeNode rootTreeNode)
        {
            List<string> checkedNames = new();
            GetNames(rootTreeNode, checkedNames);
            return checkedNames;
        }

        public void GetNames(RadTreeNode treeNode, IList<string> checkedNames)
        {
            foreach (RadTreeNode node in treeNode.Nodes)
            {
                if (node.CheckState == ToggleState.On && !_treeViewService.IsFolderNode(node))
                {
                    checkedNames.Add(node.Name);
                }

                if (_treeViewService.IsFolderNode(node))
                    GetNames(node, checkedNames);
            }
        }
    }
}

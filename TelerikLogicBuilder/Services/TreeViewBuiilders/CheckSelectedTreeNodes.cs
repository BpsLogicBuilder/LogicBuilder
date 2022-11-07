using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Collections.Generic;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class CheckSelectedTreeNodes : ICheckSelectedTreeNodes
    {
        private readonly ITreeViewService _treeViewService;

        public CheckSelectedTreeNodes(ITreeViewService treeViewService)
        {
            _treeViewService = treeViewService;
        }

        public void CheckListedNodes(RadTreeNode parentNode, HashSet<string> nodeNames)
        {
            foreach (RadTreeNode treeNode in parentNode.Nodes)
            {
                if (_treeViewService.IsFileNode(treeNode))
                {
                    treeNode.CheckState = nodeNames.Contains(treeNode.Text) 
                        ? ToggleState.On 
                        : ToggleState.Off;
                }

                if (treeNode.Nodes.Count > 0)
                    CheckListedNodes(treeNode, nodeNames);
            }
        }
    }
}

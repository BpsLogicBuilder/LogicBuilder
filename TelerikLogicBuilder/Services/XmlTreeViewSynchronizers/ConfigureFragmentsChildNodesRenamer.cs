using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ConfigureFragmentsChildNodesRenamer : IConfigureFragmentsChildNodesRenamer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;

        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsChildNodesRenamer(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public void RenameChildNodes(RadTreeNode treeNode)
        {
            foreach (RadTreeNode node in treeNode.Nodes)
            {
                if (_treeViewService.IsFolderNode(node))
                {
                    if (node.Expanded && configureFragmentsForm.ExpandedNodes.ContainsKey(node.Name))
                        configureFragmentsForm.ExpandedNodes.Remove(node.Name);
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.FOLDERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                    if (node.Expanded)
                        configureFragmentsForm.ExpandedNodes.Add(node.Name, node.Text);

                    RenameChildNodes(node);
                }
                else if (_treeViewService.IsFileNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.FRAGMENTELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else
                {
                    throw _exceptionHelper.CriticalException("{45FD4883-3868-4417-862A-A84413B71236}");
                }
            }
        }
    }
}

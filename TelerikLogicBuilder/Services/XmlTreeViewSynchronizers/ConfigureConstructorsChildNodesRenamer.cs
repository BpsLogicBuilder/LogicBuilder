using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ConfigureConstructorsChildNodesRenamer : IConfigureConstructorsChildNodesRenamer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsChildNodesRenamer(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public void RenameChildNodes(RadTreeNode treeNode)
        {
            foreach (RadTreeNode node in treeNode.Nodes)
            {
                if (_treeViewService.IsFolderNode(node))
                {
                    if (node.Expanded && this.configureConstructorsForm.ExpandedNodes.ContainsKey(node.Name))
                        this.configureConstructorsForm.ExpandedNodes.Remove(node.Name);
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.FOLDERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                    if (node.Expanded)
                        this.configureConstructorsForm.ExpandedNodes.Add(node.Name, node.Text);

                    RenameChildNodes(node);
                }
                else if (_treeViewService.IsConstructorNode(node))
                {
                    if (node.Expanded && this.configureConstructorsForm.ExpandedNodes.ContainsKey(node.Name))
                        this.configureConstructorsForm.ExpandedNodes.Remove(node.Name);
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.CONSTRUCTORELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                    if (node.Expanded)
                        this.configureConstructorsForm.ExpandedNodes.Add(node.Name, node.Text);

                    RenameChildNodes(node);
                }
                else if (_treeViewService.IsLiteralTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.LITERALPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else if (_treeViewService.IsObjectTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.OBJECTPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else if (_treeViewService.IsGenericTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.GENERICPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else if (_treeViewService.IsListOfLiteralsTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.LITERALLISTPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else if (_treeViewService.IsListOfObjectsTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.OBJECTLISTPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else if (_treeViewService.IsListOfGenericsTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{XmlDataConstants.GENERICLISTPARAMETERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else
                {
                    throw _exceptionHelper.CriticalException("{DB1A25C2-6377-4958-B2D6-B77C62573CB7}");
                }
            }
        }
    }
}

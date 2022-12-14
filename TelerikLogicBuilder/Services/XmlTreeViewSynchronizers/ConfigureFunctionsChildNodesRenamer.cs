using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ConfigureFunctionsChildNodesRenamer : IConfigureFunctionsChildNodesRenamer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsChildNodesRenamer(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public void RenameChildNodes(RadTreeNode treeNode)
        {
            foreach (RadTreeNode node in treeNode.Nodes)
            {
                if (_treeViewService.IsFolderNode(node))
                {
                    if (node.Expanded && this.configureFunctionsForm.ExpandedNodes.ContainsKey(node.Name))
                        this.configureFunctionsForm.ExpandedNodes.Remove(node.Name);
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.FOLDERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                    if (node.Expanded)
                        this.configureFunctionsForm.ExpandedNodes.Add(node.Name, node.Text);

                    RenameChildNodes(node);
                }
                else if (_treeViewService.IsMethodNode(node))
                {
                    if (node.Expanded && configureFunctionsForm.ExpandedNodes.ContainsKey(node.Name))
                        configureFunctionsForm.ExpandedNodes.Remove(node.Name);
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.FUNCTIONELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                    if (node.Expanded)
                        configureFunctionsForm.ExpandedNodes.Add(node.Name, node.Text);

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
                    throw _exceptionHelper.CriticalException("{FE259C54-07FD-4F70-8DC5-B56146A1E439}");
                }
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ConfigureVariablesChildNodesRenamer : IConfigureVariablesChildNodesRenamer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesChildNodesRenamer(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IConfigureVariablesForm configureVariablesForm)
        {
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            this.configureVariablesForm = configureVariablesForm;
        }

        public void RenameChildNodes(RadTreeNode treeNode)
        {
            foreach (RadTreeNode node in treeNode.Nodes)
            {
                if (_treeViewService.IsFolderNode(node))
                {
                    if (node.Expanded && configureVariablesForm.ExpandedNodes.ContainsKey(node.Name))
                        configureVariablesForm.ExpandedNodes.Remove(node.Name);
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.FOLDERELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                    if (node.Expanded)
                        configureVariablesForm.ExpandedNodes.Add(node.Name, node.Text);

                    RenameChildNodes(node);
                }
                else if (_treeViewService.IsLiteralTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.LITERALVARIABLEELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else if (_treeViewService.IsObjectTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.OBJECTVARIABLEELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else if (_treeViewService.IsListOfLiteralsTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.LITERALLISTVARIABLEELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else if (_treeViewService.IsListOfObjectsTypeNode(node))
                {
                    node.Name = $"{treeNode.Name}/{XmlDataConstants.OBJECTLISTVARIABLEELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{node.Text}\"]";
                }
                else
                {
                    throw _exceptionHelper.CriticalException("{0B539BE8-C661-407B-87BC-1DB6188C5CC4}");
                }
            }
        }
    }
}

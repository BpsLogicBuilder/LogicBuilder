using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System.Collections.Generic;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ConfigureGenericArgumentsXmlTreeViewSynchronizer : IConfigureGenericArgumentsXmlTreeViewSynchronizer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureGenericArgumentsForm configureGenericArgumentsForm;

        public ConfigureGenericArgumentsXmlTreeViewSynchronizer(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureGenericArgumentsForm configureGenericArgumentsForm)
        {
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureGenericArgumentsForm = configureGenericArgumentsForm;
        }

        public void ReplaceArgumentNode(RadTreeNode existingTreeNode, XmlElement newXmlGenArgParameterNode)
        {
            if (!_treeViewService.IsGenericArgumentParameterNode(existingTreeNode))
                throw _exceptionHelper.CriticalException("{1DE84339-D8C2-4C9C-B217-FD3F82A89F81}");

            if (!(new HashSet<string>
                    {
                        XmlDataConstants.LITERALPARAMETERELEMENT,
                        XmlDataConstants.OBJECTPARAMETERELEMENT,
                        XmlDataConstants.LITERALLISTPARAMETERELEMENT,
                        XmlDataConstants.OBJECTLISTPARAMETERELEMENT
                    }.Contains(newXmlGenArgParameterNode.Name)))
                throw _exceptionHelper.CriticalException("{73DA0C93-C8DF-4F60-8E03-8C329B9876E9}");

            XmlElement existingXmlNode = _xmlDocumentHelpers.SelectSingleElement(configureGenericArgumentsForm.XmlDocument, existingTreeNode.Name);
            if (existingXmlNode.ParentNode == null)
                throw _exceptionHelper.CriticalException("{5C2CE7BC-F9C0-418F-AD14-8393AFC741F3}");

            existingXmlNode.ParentNode.InsertBefore(newXmlGenArgParameterNode, existingXmlNode);
            existingXmlNode.ParentNode.RemoveChild(existingXmlNode);

            configureGenericArgumentsForm.ValidateXmlDocument();

            RadTreeNode parentNode = existingTreeNode.Parent;
            RadTreeNode newNode = _treeViewService.CreateChildTreeNode
            (
                parentNode,
                newXmlGenArgParameterNode.Name,
                XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE,
                newXmlGenArgParameterNode.Attributes[XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE]!.Value,
                _xmlDocumentHelpers.GetImageIndex(newXmlGenArgParameterNode),
                _xmlDocumentHelpers.GetGenericArgumentTreeNodeDescription(newXmlGenArgParameterNode)
            );

            parentNode.Nodes.Insert(existingTreeNode.Index, newNode);
            configureGenericArgumentsForm.TreeView.SelectedNode = null;/*ensures no action SelectedNodeChanging*/
            existingTreeNode.Remove();
            configureGenericArgumentsForm.TreeView.SelectedNode = newNode;
        }
    }
}

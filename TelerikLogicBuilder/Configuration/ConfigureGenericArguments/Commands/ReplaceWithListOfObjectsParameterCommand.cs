using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Commands
{
    internal class ReplaceWithListOfObjectsParameterCommand : ClickCommandBase
    {
        private readonly IConfigureGenericArgumentsXmlTreeViewSynchronizer _configureGenericArgumentsXmlTreeViewSynchronizer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericConfigManager _genericConfigManager;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureGenericArgumentsForm configureGenericArgumentsForm;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;

        public ReplaceWithListOfObjectsParameterCommand(
            IExceptionHelper exceptionHelper,
            IGenericConfigManager genericConfigManager,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureGenericArgumentsForm configureGenericArgumentsForm)
        {
            _configureGenericArgumentsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureGenericArgumentsXmlTreeViewSynchronizer(configureGenericArgumentsForm);
            _exceptionHelper = exceptionHelper;
            _genericConfigManager = genericConfigManager;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            treeView = configureGenericArgumentsForm.TreeView;
            xmlDocument = configureGenericArgumentsForm.XmlDocument;
            this.configureGenericArgumentsForm = configureGenericArgumentsForm;
        }

        public override void Execute()
        {
            if (treeView.SelectedNode == null)
                return;

            RadTreeNode existingTreeNode = treeView.SelectedNode;

            if (!_treeViewService.IsGenericArgumentParameterNode(existingTreeNode))
                return;

            XmlElement xmlElement = _xmlDocumentHelpers.SelectSingleElement
            (
                xmlDocument,
                existingTreeNode.Name
            );

            try
            {
                _configureGenericArgumentsXmlTreeViewSynchronizer.ReplaceArgumentNode
                (
                    existingTreeNode,
                    _xmlDocumentHelpers.AddElementToDoc
                    (
                        xmlDocument,
                        _xmlDocumentHelpers.ToXmlElement
                        (
                            _genericConfigManager.GetDefaultObjectListGenericConfig
                            (
                                xmlElement.Attributes[XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE]!.Value
                            ).ToXml
                        )
                    )
                );
            }
            catch (LogicBuilderException ex)
            {
                configureGenericArgumentsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

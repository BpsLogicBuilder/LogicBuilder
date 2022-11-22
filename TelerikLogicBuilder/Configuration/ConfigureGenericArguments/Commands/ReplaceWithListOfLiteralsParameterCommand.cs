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
    internal class ReplaceWithListOfLiteralsParameterCommand : ClickCommandBase
    {
        private readonly IConfigureGenericArgumentsXmlTreeViewSynchronizer _configureGenericArgumentsXmlTreeViewSynchronizer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericConfigManager _genericConfigManager;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureGenericArgumentsForm configureGenericArgumentsForm;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;

        public ReplaceWithListOfLiteralsParameterCommand(
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
                throw _exceptionHelper.CriticalException("{37B9B54D-D5DC-4184-BF5D-78AFCFE9212C}");

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
                            _genericConfigManager.GetDefaultLiteralListGenericConfig
                            (
                                xmlElement.GetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE)
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

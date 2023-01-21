using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsAddGenericParameterCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionsXmlTreeViewSynchronizer _configureFunctionsXmlTreeViewSynchronizer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IParameterFactory _parameterFactory;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsAddGenericParameterCommand(
            IExceptionHelper exceptionHelper,
            IParameterFactory parameterFactory,
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _configureFunctionsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureFunctionsXmlTreeViewSynchronizer
            (
                configureFunctionsForm
            );
            _exceptionHelper = exceptionHelper;
            _parameterFactory = parameterFactory;
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        private XmlDocument XmlDocument => configureFunctionsForm.XmlDocument;

        public override void Execute()
        {
            try
            {
                RadTreeNode? selectedNode = configureFunctionsForm.TreeView.SelectedNode;
                if (selectedNode == null)
                    return;

                if (_treeViewService.IsFolderNode(selectedNode))
                    return;

                if (_treeViewService.IsParameterNode(selectedNode))
                    selectedNode = selectedNode.Parent;

                string parameterName = _stringHelper.EnsureUniqueName
                (
                    Strings.defaultNewParameterName,
                    _xmlDocumentHelpers.GetParameterElements
                    (
                        _xmlDocumentHelpers.SelectSingleElement(XmlDocument, selectedNode.Name)
                    )
                    .Select(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
                    .ToHashSet()
                );

                string[] genericArgumentNames = _xmlDocumentHelpers.GetGenericArguments
                (
                    XmlDocument,
                    $"{selectedNode.Name}/{XmlDataConstants.GENERICARGUMENTSELEMENT}"
                );

                if (genericArgumentNames.Length == 0)
                    throw _exceptionHelper.CriticalException("{CA8AA98A-2703-4CB7-A214-7098C4F9C0CC}");

                XmlElement newXmlElement = _xmlDocumentHelpers.AddElementToDoc
                (
                    XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _parameterFactory.GetGenericParameter
                        (
                            parameterName,
                            true,
                            string.Empty,
                            genericArgumentNames[0]
                        ).ToXml
                    )
                );

                _configureFunctionsXmlTreeViewSynchronizer.AddParameterNode((StateImageRadTreeNode)selectedNode, newXmlElement);
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

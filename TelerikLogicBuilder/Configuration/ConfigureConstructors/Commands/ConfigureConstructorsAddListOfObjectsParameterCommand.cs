using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsAddListOfObjectsParameterCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly IParameterFactory _parameterFactory;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        private XmlDocument XmlDocument => configureConstructorsForm.XmlDocument;

        public ConfigureConstructorsAddListOfObjectsParameterCommand(
            IParameterFactory parameterFactory,
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _configureConstructorsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureConstructorsXmlTreeViewSynchronizer
            (
                configureConstructorsForm
            );
            _parameterFactory = parameterFactory;
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public override void Execute()
        {
            try
            {
                RadTreeNode? selectedNode = configureConstructorsForm.TreeView.SelectedNode;
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
                    .Select(e => e.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
                    .ToHashSet()
                );

                XmlElement newXmlElement = _xmlDocumentHelpers.AddElementToDoc
                (
                    XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _parameterFactory.GetListOfObjectsParameter
                        (
                            parameterName,
                            true, 
                            string.Empty,
                            MiscellaneousConstants.DEFAULT_OBJECT_TYPE, 
                            ListType.GenericList,
                            ListParameterInputStyle.HashSetForm
                        ).ToXml
                    )
                );

                _configureConstructorsXmlTreeViewSynchronizer.AddParameterNode((StateImageRadTreeNode)selectedNode, newXmlElement);
            }
            catch (LogicBuilderException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

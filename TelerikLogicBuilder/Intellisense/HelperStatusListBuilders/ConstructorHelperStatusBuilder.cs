using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal class ConstructorHelperStatusBuilder : IConstructorHelperStatusBuilder
    {
        private readonly IConstructorNodeBuilder _constructorNodeBuilder;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConstructorHelperStatusBuilder(
            IConstructorNodeBuilder constructorNodeBuilder,
            IParametersXmlParser parametersXmlParser,
            ITreeViewService treeViewService,
            ITypeLoadHelper typeLoadHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _constructorNodeBuilder = constructorNodeBuilder;
            _parametersXmlParser = parametersXmlParser;
            _treeViewService = treeViewService;
            _typeLoadHelper = typeLoadHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public ConstructorHelperStatus? Build()
        {
            RadTreeNode? selectedNode = configureConstructorsForm.TreeView.SelectedNode;

            if (!configureConstructorsForm.Application.AssemblyAvailable
                || selectedNode == null
                || !_treeViewService.IsConstructorNode(selectedNode))
            {
                return null;
            }

            XmlElement constructorElement = _xmlDocumentHelpers.SelectSingleElement(configureConstructorsForm.XmlDocument, selectedNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(constructorElement).ToDictionary(e => e.Name);
            string classFullName = elements[XmlDataConstants.TYPENAMEELEMENT].InnerText;

            if (!_typeLoadHelper.TryGetSystemType(classFullName, configureConstructorsForm.Application, out Type? declaringType))
                return null;

            ConstructorTreeNode? constructorTreeNode = _constructorNodeBuilder.Build
            (
                declaringType,
                _xmlDocumentHelpers.GetChildElements
                (
                    elements[XmlDataConstants.PARAMETERSELEMENT]
                )
                .Select(_parametersXmlParser.Parse)
                .ToArray()
            );

            return constructorTreeNode == null
                ? null
                : new ConstructorHelperStatus
                (
                    configureConstructorsForm.Application.Application,
                    constructorTreeNode,
                    classFullName
                );
        }
    }
}

﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsAddLiteralParameterCommand : ClickCommandBase
    {
        private readonly IConfigureFunctionsXmlTreeViewSynchronizer _configureFunctionsXmlTreeViewSynchronizer;
        private readonly IParameterFactory _parameterFactory;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        private XmlDocument XmlDocument => configureFunctionsForm.XmlDocument;

        public ConfigureFunctionsAddLiteralParameterCommand(
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
            _parameterFactory = parameterFactory;
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFunctionsForm = configureFunctionsForm;
        }

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

                XmlElement newXmlElement = _xmlDocumentHelpers.AddElementToDoc
                (
                    XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _parameterFactory.GetLiteralParameter
                        (
                            parameterName,
                            true,
                            string.Empty,
                            LiteralParameterType.String,
                            LiteralParameterInputStyle.SingleLineTextBox,
                            true,
                            false,
                            true,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            new List<string>()
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

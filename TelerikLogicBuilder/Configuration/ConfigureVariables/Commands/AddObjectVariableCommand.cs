﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Text.RegularExpressions;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class AddObjectVariableCommand : ClickCommandBase
    {
        private readonly IConfigureVariablesXmlTreeViewSynchronizer _configureVariablesXmlTreeViewSynchronizer;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IVariableFactory _variableFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public AddObjectVariableCommand(
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IVariableFactory variableFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureVariablesForm configureVariablesForm)
        {
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _variableFactory = variableFactory;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _configureVariablesXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureVariablesXmlTreeViewSynchronizer
            (
                configureVariablesForm
            );
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            try
            {
                if (configureVariablesForm.TreeView.SelectedNode == null)
                    return;

                RadTreeNode selectedNode = configureVariablesForm.TreeView.SelectedNode;
                if (_treeViewService.IsVariableTypeNode(selectedNode))
                    selectedNode = selectedNode.Parent;

                string variableName = _stringHelper.EnsureUniqueName(Strings.defaultNewVariableName, configureVariablesForm.VariableNames);

                XmlElement newXmlElement = _xmlDocumentHelpers.AddElementToDoc
                (
                    configureVariablesForm.XmlDocument,
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _variableFactory.GetObjectVariable
                        (
                            variableName,
                            $"{Regex.Replace(variableName[..1], RegularExpressions.REMOVENUMERICANDNONWORD, string.Empty)}{Regex.Replace(variableName[1..], RegularExpressions.REMOVENONWORD, string.Empty)}",
                            VariableCategory.Property,
                            string.Empty,
                            string.Empty,
                            Strings.referenceNamePlaceHolder,
                            Enum.GetName(typeof(ValidIndirectReference), ValidIndirectReference.Property)!,
                            string.Empty,
                            ReferenceCategories.InstanceReference,
                            string.Empty,
                            MiscellaneousConstants.DEFAULT_OBJECT_TYPE
                        ).ToXml
                    )
                );

                _configureVariablesXmlTreeViewSynchronizer.AddVariableNode((StateImageRadTreeNode)selectedNode, newXmlElement);
                configureVariablesForm.CheckEnableImportButton();
            }
            catch (LogicBuilderException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Commands
{
    internal class ConfigureFunctionReturnTypeCommand : ClickCommandBase
    {
        private readonly IReturnTypeXmlParser _returnTypeXmlParser;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionControl configureFunctionControl;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;

        public ConfigureFunctionReturnTypeCommand(
            IReturnTypeXmlParser returnTypeXmlParser,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureFunctionControl configureFunctionControl)
        {
            _returnTypeXmlParser = returnTypeXmlParser;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            treeView = configureFunctionControl.TreeView;
            xmlDocument = configureFunctionControl.XmlDocument;
            this.configureFunctionControl = configureFunctionControl;
        }

        public override void Execute()
        {
            try
            {
                configureFunctionControl.ClearMessage();
                EditReturnType();
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionControl.SetErrorMessage(ex.Message);
            }
        }

        private void EditReturnType()
        {
            RadTreeNode? selectedNode = treeView.SelectedNode;
            if (selectedNode == null || !_treeViewService.IsMethodNode(selectedNode))
                return;

            XmlElement genericArgumentsElement = _xmlDocumentHelpers.GetSingleChildElement
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    xmlDocument,
                    selectedNode.Name
                ),
                e => e.Name == XmlDataConstants.GENERICARGUMENTSELEMENT
            );
            XmlElement returnTypeElement = _xmlDocumentHelpers.GetSingleChildElement
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    xmlDocument,
                    selectedNode.Name
                ),
                e => e.Name == XmlDataConstants.RETURNTYPEELEMENT
            );

            using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            IConfigureReturnTypeForm configureReturnTypeForm = disposableManager.GetConfigureReturnTypeForm
            (
                _xmlDocumentHelpers.GetChildElements
                (
                    genericArgumentsElement
                )
                .Select(e => e.InnerText)
                .ToArray(),
                _returnTypeXmlParser.Parse
                (
                    _xmlDocumentHelpers.GetSingleChildElement(returnTypeElement)
                )
            );
            configureReturnTypeForm.ShowDialog((Control)configureFunctionControl);
            if (configureReturnTypeForm.DialogResult != DialogResult.OK)
                return;

            returnTypeElement.InnerXml = configureReturnTypeForm.ReturnType.ToXml;
            configureFunctionControl.ValidateXmlDocument();
        }
    }
}

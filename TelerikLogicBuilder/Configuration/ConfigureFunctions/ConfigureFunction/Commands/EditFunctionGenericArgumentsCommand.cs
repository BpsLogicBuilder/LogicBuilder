using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction.Commands
{
    internal class EditFunctionGenericArgumentsCommand : ClickCommandBase
    {
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionControl configureFunctionControl;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;

        public EditFunctionGenericArgumentsCommand(
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureFunctionControl configureFunctionControl)
        {
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
                EditGenericArguments();
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionControl.SetErrorMessage(ex.Message);
            }
        }

        private void EditGenericArguments()
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

            using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            IEditGenericArgumentsForm editGenericArgumentsForm = disposableManager.GetEditGenericArgumentsForm
            (
                _xmlDocumentHelpers.GetChildElements
                (
                    genericArgumentsElement
                )
                .Select(e => e.InnerText)
                .ToArray()
            );
            editGenericArgumentsForm.ShowDialog((Control)configureFunctionControl);
            if (editGenericArgumentsForm.DialogResult != DialogResult.OK)
                return;

            genericArgumentsElement.InnerXml = BuildGenericArgumentsXml();
            configureFunctionControl.ValidateXmlDocument();

            string BuildGenericArgumentsXml()
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                {
                    foreach (string item in editGenericArgumentsForm.Arguments)
                        xmlTextWriter.WriteElementString(XmlDataConstants.ITEMELEMENT, item);
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();
                }
                return stringBuilder.ToString();
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter.Commands
{
    internal class UpdateLiteralListParameterDefaultValueCommand : ClickCommandBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly RadDropDownList cmbListLpLiteralType;

        private readonly IConfigureLiteralListParameterControl configureLiteralListParameterControl;

        public UpdateLiteralListParameterDefaultValueCommand(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureLiteralListParameterControl configureLiteralListParameterControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            treeView = configureLiteralListParameterControl.TreeView;
            xmlDocument = configureLiteralListParameterControl.XmlDocument;
            cmbListLpLiteralType = configureLiteralListParameterControl.CmbListLpLiteralType;
            this.configureLiteralListParameterControl = configureLiteralListParameterControl;
        }

        public override void Execute()
        {
            RadTreeNode selecteNode = treeView.SelectedNode;
            if (selecteNode == null)
                return;

            if (!_treeViewService.IsListOfLiteralsTypeNode(selecteNode))
                throw _exceptionHelper.CriticalException("{05EC0877-F21D-40B8-B318-BCAA24C66D1D}");

            XmlElement defaultValueElement = _xmlDocumentHelpers.GetSingleChildElement
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    xmlDocument,
                    selecteNode.Name
                ),
                e => e.Name == XmlDataConstants.DEFAULTVALUEELEMENT
            );

            IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            using IConfigureLiteralListDefaultValueForm configureLiteralListDefaultValueForm = disposableManager.GetConfigureLiteralListDefaultValueForm
            (
                _xmlDocumentHelpers.GetChildElements
                (
                    defaultValueElement
                )
                .Select(e => e.InnerText)
                .ToArray(),
                _enumHelper.GetSystemType((LiteralParameterType)cmbListLpLiteralType.SelectedValue)
            );

            configureLiteralListDefaultValueForm.ShowDialog((Control)configureLiteralListParameterControl);
            if (configureLiteralListDefaultValueForm.DialogResult != DialogResult.OK)
                return;

            try
            {
                defaultValueElement.InnerXml = BuildDefaultValueItemsXml();
                configureLiteralListParameterControl.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                configureLiteralListParameterControl.SetErrorMessage(ex.Message);
            }

            string BuildDefaultValueItemsXml()
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                {
                    foreach (string item in configureLiteralListDefaultValueForm.DefaultValueItems)
                        xmlTextWriter.WriteElementString(XmlDataConstants.ITEMELEMENT, item);
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();
                }
                return stringBuilder.ToString();
            }
        }
    }
}

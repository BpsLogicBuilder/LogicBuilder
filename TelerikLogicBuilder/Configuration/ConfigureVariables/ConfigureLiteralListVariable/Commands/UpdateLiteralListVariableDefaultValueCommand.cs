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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Commands
{
    internal class UpdateLiteralListVariableDefaultValueCommand : ClickCommandBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly RadDropDownList cmbLiteralType;
        private readonly IConfigureLiteralListVariableControl configureLiteralListVariableControl;

        public UpdateLiteralListVariableDefaultValueCommand(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureLiteralListVariableControl configureLiteralListVariableControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            treeView = configureLiteralListVariableControl.TreeView;
            xmlDocument = configureLiteralListVariableControl.XmlDocument;
            cmbLiteralType = configureLiteralListVariableControl.CmbLiteralType;
            this.configureLiteralListVariableControl = configureLiteralListVariableControl;
        }

        public override void Execute()
        {
            RadTreeNode selecteNode = treeView.SelectedNode;
            if (selecteNode == null)
                return;

            if (!_treeViewService.IsListOfLiteralsTypeNode(selecteNode))
                throw _exceptionHelper.CriticalException("{FD5DCA31-248A-4867-8089-3C94E171EB82}");

            XmlElement defaultValueElement = _xmlDocumentHelpers.GetSingleChildElement
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    xmlDocument,
                    selecteNode.Name
                ),
                e => e.Name == XmlDataConstants.DEFAULTVALUEELEMENT
            );

            using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            IConfigureLiteralListDefaultValueForm configureLiteralListDefaultValueForm = disposableManager.GetConfigureLiteralListDefaultValueForm
            (
                _xmlDocumentHelpers.GetChildElements
                (
                    defaultValueElement
                )
                .Select(e => e.InnerText)
                .ToArray(),
                _enumHelper.GetSystemType((LiteralVariableType)cmbLiteralType.SelectedValue)
            );

            configureLiteralListDefaultValueForm.ShowDialog((Control)configureLiteralListVariableControl);
            if (configureLiteralListDefaultValueForm.DialogResult != DialogResult.OK)
                return;

            try
            {
                defaultValueElement.InnerXml = BuildDefaultValueItemsXml();
                configureLiteralListVariableControl.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                configureLiteralListVariableControl.SetErrorMessage(ex.Message);
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

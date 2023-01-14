using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain;
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
    internal class UpdateLiteralListParameterDomainCommand : ClickCommandBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly RadDropDownList cmbListLpLiteralType;

        private readonly IConfigureLiteralListParameterControl configureLiteralListParameterControl;

        public UpdateLiteralListParameterDomainCommand(
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
                throw _exceptionHelper.CriticalException("{52CB2453-BBA6-4449-87D3-F4CC770CF18E}");

            XmlElement domainElement = _xmlDocumentHelpers.GetSingleChildElement
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    xmlDocument,
                    selecteNode.Name
                ),
                e => e.Name == XmlDataConstants.DOMAINELEMENT
            );

            using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            IConfigureLiteralDomainForm configureLiteralDomainForm = disposableManager.GetConfigureLiteralDomainForm
            (
                _xmlDocumentHelpers.GetChildElements
                (
                    domainElement
                )
                .Select(e => e.InnerText)
                .ToArray(),
                _enumHelper.GetSystemType((LiteralParameterType)cmbListLpLiteralType.SelectedValue)
            );

            configureLiteralDomainForm.ShowDialog((Control)configureLiteralListParameterControl);
            if (configureLiteralDomainForm.DialogResult != DialogResult.OK)
                return;

            try
            {
                domainElement.InnerXml = BuildDomainItemsXml();
                configureLiteralListParameterControl.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                configureLiteralListParameterControl.SetErrorMessage(ex.Message);
            }

            string BuildDomainItemsXml()
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                {
                    foreach (string domainItem in configureLiteralDomainForm.DomainItems)
                        xmlTextWriter.WriteElementString(XmlDataConstants.ITEMELEMENT, domainItem);
                    xmlTextWriter.Flush();
                    xmlTextWriter.Close();
                }
                return stringBuilder.ToString();
            }
        }
    }
}

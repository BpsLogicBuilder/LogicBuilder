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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter.Command
{
    internal class UpdateLiteralParameterDomainCommand : ClickCommandBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly RadDropDownList cmbLpLiteralType;

        private readonly IConfigureLiteralParameterControl configureLiteralParameterControl;

        public UpdateLiteralParameterDomainCommand(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureLiteralParameterControl configureLiteralParameterControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            treeView = configureLiteralParameterControl.TreeView;
            xmlDocument = configureLiteralParameterControl.XmlDocument;
            cmbLpLiteralType = configureLiteralParameterControl.CmbLpLiteralType;
            this.configureLiteralParameterControl = configureLiteralParameterControl;
        }

        public override void Execute()
        {
            RadTreeNode selecteNode = treeView.SelectedNode;
            if (selecteNode == null)
                return;

            if (!_treeViewService.IsLiteralTypeNode(selecteNode))
                throw _exceptionHelper.CriticalException("{1549C867-FE1F-4D07-A469-E66F0F08E972}");

            XmlElement domainElement = _xmlDocumentHelpers.GetSingleChildElement
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    xmlDocument,
                    selecteNode.Name
                ),
                e => e.Name == XmlDataConstants.DOMAINELEMENT
            );

            IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            using IConfigureLiteralDomainForm configureLiteralDomainForm = disposableManager.GetConfigureLiteralDomainForm
            (
                _xmlDocumentHelpers.GetChildElements
                (
                    domainElement
                )
                .Select(e => e.InnerText)
                .ToArray(),
                _enumHelper.GetSystemType((LiteralParameterType)cmbLpLiteralType.SelectedValue)
            );

            configureLiteralDomainForm.ShowDialog((Control)configureLiteralParameterControl);
            if (configureLiteralDomainForm.DialogResult != DialogResult.OK)
                return;

            try
            {
                domainElement.InnerXml = BuildDomainItemsXml();
                configureLiteralParameterControl.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                configureLiteralParameterControl.SetErrorMessage(ex.Message);
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

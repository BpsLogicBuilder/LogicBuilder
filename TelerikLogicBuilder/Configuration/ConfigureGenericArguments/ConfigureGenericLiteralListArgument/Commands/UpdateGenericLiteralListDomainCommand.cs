using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericLiteralListArgument.Commands
{
    internal class UpdateGenericLiteralListDomainCommand : ClickCommandBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadDropDownList cmbListLpLiteralType;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly IConfigureGenericLiteralListArgumentControl configureGenericLiteralListArgumentControl;

        public UpdateGenericLiteralListDomainCommand(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureGenericLiteralListArgumentControl configureGenericLiteralListArgumentControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _treeViewService = treeViewService;
            cmbListLpLiteralType = configureGenericLiteralListArgumentControl.CmbListLpLiteralType;
            treeView = configureGenericLiteralListArgumentControl.TreeView;
            xmlDocument = configureGenericLiteralListArgumentControl.XmlDocument;
            this.configureGenericLiteralListArgumentControl = configureGenericLiteralListArgumentControl;
        }

        public override void Execute()
        {
            RadTreeNode selecteNode = treeView.SelectedNode;
            if (selecteNode == null)
                return;

            if (!_treeViewService.IsListOfLiteralsTypeNode(selecteNode))
                throw _exceptionHelper.CriticalException("{0D3F4C68-1184-427F-B178-5F93C05A0ABF}");

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

            configureLiteralDomainForm.ShowDialog((Control)configureGenericLiteralListArgumentControl);
            if (configureLiteralDomainForm.DialogResult != DialogResult.OK)
                return;

            domainElement.InnerXml = BuildDomainItemsXml();

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

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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable.Commands
{
    internal class UpdateLiteralListVariableDomainCommand : ClickCommandBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly RadDropDownList cmbLiteralType;
        private readonly IConfigureLiteralListVariableControl configureLiteralListVariableControl;

        public UpdateLiteralListVariableDomainCommand(
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
                throw _exceptionHelper.CriticalException("{C904D9C2-034D-49EC-A639-CE5B63B5A691}");

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
                _enumHelper.GetSystemType((LiteralVariableType)cmbLiteralType.SelectedValue)
            );

            configureLiteralDomainForm.ShowDialog((Control)configureLiteralListVariableControl);
            if (configureLiteralDomainForm.DialogResult != DialogResult.OK)
                return;

            try
            {
                domainElement.InnerXml = BuildDomainItemsXml();
                configureLiteralListVariableControl.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                configureLiteralListVariableControl.SetErrorMessage(ex.Message);
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

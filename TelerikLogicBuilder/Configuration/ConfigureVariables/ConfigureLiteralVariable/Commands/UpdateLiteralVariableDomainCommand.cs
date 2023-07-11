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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable.Commands
{
    internal class UpdateLiteralVariableDomainCommand : ClickCommandBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly RadDropDownList cmbLiteralType;
        private readonly IConfigureLiteralVariableControl configureLiteralVariableControl;

        public UpdateLiteralVariableDomainCommand(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureLiteralVariableControl configureLiteralVariableControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            treeView = configureLiteralVariableControl.TreeView;
            xmlDocument = configureLiteralVariableControl.XmlDocument;
            cmbLiteralType = configureLiteralVariableControl.CmbLiteralType;
            this.configureLiteralVariableControl = configureLiteralVariableControl;
        }

        public override void Execute()
        {
            RadTreeNode selectedNode = treeView.SelectedNode;
            if (selectedNode == null)
                return;

            if (!_treeViewService.IsLiteralTypeNode(selectedNode))
                throw _exceptionHelper.CriticalException("{86B3670C-7FEF-47ED-8CFC-A45BB7014073}");

            XmlElement domainElement = _xmlDocumentHelpers.GetSingleChildElement
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    xmlDocument,
                    selectedNode.Name
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
                _enumHelper.GetSystemType((LiteralVariableType)cmbLiteralType.SelectedValue)
            );

            configureLiteralDomainForm.ShowDialog((Control)configureLiteralVariableControl);
            if (configureLiteralDomainForm.DialogResult != DialogResult.OK)
                return;

            try
            {
                domainElement.InnerXml = BuildDomainItemsXml();
                configureLiteralVariableControl.ValidateXmlDocument();
            }
            catch (LogicBuilderException ex)
            {
                configureLiteralVariableControl.SetErrorMessage(ex.Message);
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

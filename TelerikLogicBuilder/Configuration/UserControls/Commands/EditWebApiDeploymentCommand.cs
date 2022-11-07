using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands
{
    internal class EditWebApiDeploymentCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly ITreeViewService _treeViewService;
        private readonly IWebApiDeploymentXmlParser _webApiDeploymentXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;

        public EditWebApiDeploymentCommand(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            ITreeViewService treeViewService,
            IWebApiDeploymentXmlParser webApiDeploymentXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IApplicationControl applicationControl)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _treeViewService = treeViewService;
            _mainWindow = mainWindow;
            _webApiDeploymentXmlParser = webApiDeploymentXmlParser;
            this.treeView = applicationControl.TreeView;
            this.xmlDocument = applicationControl.XmlDocument;
        }

        public override void Execute()
        {
            RadTreeNode selecteNode = this.treeView.SelectedNode;
            if (selecteNode == null)
                return;

            if (!_treeViewService.IsApplicationNode(selecteNode))
                throw _exceptionHelper.CriticalException("{830E589E-4C8F-4840-ABC6-5BF7B96AB93B}");

            using IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            ConfigureWebApiDeployment configureWebApiDeployment = disposableManager.GetConfigureWebApiDeployment
            (
                _webApiDeploymentXmlParser.Parse
                (
                    _xmlDocumentHelpers.SelectSingleElement
                    (
                        this.xmlDocument,
                        $"{selecteNode.Name}/{XmlDataConstants.WEBAPIDEPLOYMENTELEMENT}"
                    )
                )
            );
            configureWebApiDeployment.ShowDialog(_mainWindow.Instance);

            if (configureWebApiDeployment.DialogResult != DialogResult.OK)
                return;

            XmlElement elementToUpdate = _xmlDocumentHelpers.ToXmlElement
            (
                configureWebApiDeployment.WebApiDeployment.ToXml
            );

            _xmlDocumentHelpers
                .SelectSingleElement(this.xmlDocument, $"{selecteNode.Name}/{XmlDataConstants.WEBAPIDEPLOYMENTELEMENT}")
                .InnerXml = elementToUpdate.InnerXml;
        }
    }
}

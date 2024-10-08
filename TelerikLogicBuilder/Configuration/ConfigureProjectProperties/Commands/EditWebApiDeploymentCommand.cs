﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Commands
{
    internal class EditWebApiDeploymentCommand : ClickCommandBase
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IWebApiDeploymentXmlParser _webApiDeploymentXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;
        private readonly IApplicationControl applicationControl;

        public EditWebApiDeploymentCommand(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IWebApiDeploymentXmlParser webApiDeploymentXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IApplicationControl applicationControl)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _treeViewService = treeViewService;
            _webApiDeploymentXmlParser = webApiDeploymentXmlParser;
            treeView = applicationControl.TreeView;
            xmlDocument = applicationControl.XmlDocument;
            this.applicationControl = applicationControl;
        }

        public override void Execute()
        {
            RadTreeNode selecteNode = treeView.SelectedNode;
            if (selecteNode == null)
                return;

            if (!_treeViewService.IsApplicationNode(selecteNode))
                throw _exceptionHelper.CriticalException("{830E589E-4C8F-4840-ABC6-5BF7B96AB93B}");

            IConfigurationFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IConfigurationFormFactory>();
            using IConfigureWebApiDeploymentForm configureWebApiDeployment = disposableManager.GetConfigureWebApiDeploymentForm
            (
                _webApiDeploymentXmlParser.Parse
                (
                    _xmlDocumentHelpers.SelectSingleElement
                    (
                        xmlDocument,
                        $"{selecteNode.Name}/{XmlDataConstants.WEBAPIDEPLOYMENTELEMENT}"
                    )
                )
            );
            configureWebApiDeployment.ShowDialog((Control)applicationControl);/*need the current parent not main windows*/

            if (configureWebApiDeployment.DialogResult != DialogResult.OK)
                return;

            XmlElement elementToUpdate = _xmlDocumentHelpers.ToXmlElement
            (
                configureWebApiDeployment.WebApiDeployment.ToXml
            );

            _xmlDocumentHelpers
                .SelectSingleElement(xmlDocument, $"{selecteNode.Name}/{XmlDataConstants.WEBAPIDEPLOYMENTELEMENT}")
                .InnerXml = elementToUpdate.InnerXml;
        }
    }
}

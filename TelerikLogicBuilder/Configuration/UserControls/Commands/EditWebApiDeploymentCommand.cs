﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands
{
    internal class EditWebApiDeploymentCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly RadTreeView treeView;
        private readonly XmlDocument xmlDocument;

        public EditWebApiDeploymentCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IApplicationControl applicationControl)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.treeView = applicationControl.TreeView;
            this.xmlDocument = applicationControl.XmlDocument;
        }

        public override void Execute()
        {
            DisplayMessage.Show("EditWebApiDeploymentCommand");
        }
    }
}
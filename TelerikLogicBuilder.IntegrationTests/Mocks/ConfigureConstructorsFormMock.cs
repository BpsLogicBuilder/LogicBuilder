﻿using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace TelerikLogicBuilder.IntegrationTests.Mocks
{
    internal class ConfigureConstructorsFormMock : IConfigureConstructorsForm
    {
        public ConfigureConstructorsFormMock(ApplicationTypeInfo application, ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper,
            RadTreeView treeView,
            XmlDocument xmlDocument,
            IConfigurationFormChildNodesRenamerFactory configurationFormChildNodesRenamerFactory)
        {
            Application = application;
            _configureConstructorsChildNodesRenamer = configurationFormChildNodesRenamerFactory.GetConfigureConstructorsChildNodesRenamer(this);
            _treeViewXmlDocumentHelper = treeViewXmlDocumentHelper;
            TreeView = treeView;
            XmlDocument = xmlDocument;
        }

        private readonly IConfigureConstructorsChildNodesRenamer _configureConstructorsChildNodesRenamer;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;

        public RadTreeView TreeView { get; }

        public XmlDocument XmlDocument { get; }

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public ApplicationTypeInfo Application { get; }

        public DialogResult DialogResult => throw new NotImplementedException();

        public bool CanExecuteImport => throw new NotImplementedException();

        public IList<RadTreeNode> CutTreeNodes => throw new NotImplementedException();

        public IConfigureConstructorsTreeNodeControl CurrentTreeNodeControl => throw new NotImplementedException();

        public ConstructorHelperStatus? HelperStatus => throw new NotImplementedException();

        public IDictionary<string, Constructor> ConstructorsDictionary => throw new NotImplementedException();

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public void RebuildTreeView()
        {
        }

        public void ReloadXmlDocument(string xmlString)
        {
            _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);
        }

        public void RenameChildNodes(RadTreeNode treeNode)
        {
            _configureConstructorsChildNodesRenamer.RenameChildNodes(treeNode);
        }

        public void SetErrorMessage(string message)
        {
            ApplicationChanged?.Invoke(this, new ApplicationChangedEventArgs(null!));
            throw new NotImplementedException();
        }

        public void SetMessage(string message, string title = "")
        {
            throw new NotImplementedException();
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            throw new NotImplementedException();
        }

        public void ValidateXmlDocument()
        {
            _treeViewXmlDocumentHelper.ValidateXmlDocument();
        }

        public void SelectTreeNode(RadTreeNode treeNode)
        {
            TreeView.SelectedNode = treeNode;
        }

        public void CheckEnableImportButton()
        {
            throw new NotImplementedException();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
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

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class ConfigureFragmentsFormMock : IConfigureFragmentsForm
    {
        private readonly IConfigureFragmentsChildNodesRenamer _configureFragmentsChildNodesRenamer;
        private readonly ITreeViewXmlDocumentHelper _treeViewXmlDocumentHelper;
        public ConfigureFragmentsFormMock()
        {
            _configureFragmentsChildNodesRenamer = null!;
            _treeViewXmlDocumentHelper = null!;
            this.TreeView = null!;
            this.XmlDocument = null!;
        }
        public ConfigureFragmentsFormMock(
            ITreeViewXmlDocumentHelper treeViewXmlDocumentHelper,
            RadTreeView treeView,
            XmlDocument xmlDocument,
            IConfigurationFormChildNodesRenamerFactory configurationFormChildNodesRenamerFactory)
        {
            _configureFragmentsChildNodesRenamer = configurationFormChildNodesRenamerFactory.GetConfigureFragmentsChildNodesRenamer(this);
            _treeViewXmlDocumentHelper = treeViewXmlDocumentHelper;
            this.TreeView = treeView;
            this.XmlDocument = xmlDocument;
        }

        public RadTreeView TreeView { get; }
        public XmlDocument XmlDocument { get; }

        public bool CanExecuteImport => throw new NotImplementedException();

        public IList<RadTreeNode> CutTreeNodes => throw new NotImplementedException();

        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public DialogResult DialogResult => throw new NotImplementedException();

        public HashSet<string> FragmentNames => throw new NotImplementedException();

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void CheckEnableImportButton()
        {
            ApplicationChanged?.Invoke(this, new ApplicationChangedEventArgs(null!));
            throw new NotImplementedException();
        }

        public void ClearMessage()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void RebuildTreeView()
        {
            throw new NotImplementedException();
        }

        public void ReloadXmlDocument(string xmlString)
        {
            _treeViewXmlDocumentHelper.LoadXmlDocument(xmlString);
        }

        public void RenameChildNodes(RadTreeNode treeNode)
        {
            _configureFragmentsChildNodesRenamer.RenameChildNodes(treeNode);
        }

        public void SelectTreeNode(RadTreeNode treeNode)
        {
            TreeView.SelectedNode = treeNode;
        }

        public void SetErrorMessage(string message)
        {
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
    }
}

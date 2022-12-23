using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class ConfigureConstructorsFormMock : IConfigureConstructorsForm
    {
        public IDictionary<string, string> ExpandedNodes { get; } = new Dictionary<string, string>();

        public RadTreeView TreeView => throw new NotImplementedException();

        public XmlDocument XmlDocument => throw new NotImplementedException();

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public DialogResult DialogResult => throw new NotImplementedException();

        public bool CanExecuteImport => throw new NotImplementedException();

        public IList<RadTreeNode> CutTreeNodes => throw new NotImplementedException();

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void CheckEnableImportButton()
        {
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
        }

        public void ReloadXmlDocument(string xmlString)
        {
        }

        public void RenameChildNodes(RadTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        public void SelectTreeNode(RadTreeNode treeNode)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}

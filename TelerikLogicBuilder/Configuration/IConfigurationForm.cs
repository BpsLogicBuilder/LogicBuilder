using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal interface IConfigurationForm : IApplicationForm
    {
        bool CanExecuteImport { get; }
        IList<RadTreeNode> CutTreeNodes { get; }
        IDictionary<string, string> ExpandedNodes { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }

        void CheckEnableImportButton();
        void RebuildTreeView();
        void RenameChildNodes(RadTreeNode treeNode);
        void ReloadXmlDocument(string xmlString);
        void SelectTreeNode(RadTreeNode treeNode);
        void ValidateXmlDocument();
    }
}

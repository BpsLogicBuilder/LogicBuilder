using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITreeViewXmlDocumentHelper
    {
        XmlDocument BackupXmlTreeDocument { get; }
        RadTreeView TreeView { get; }
        SchemaName Schema { get; }
        XmlDocument XmlTreeDocument { get; }

        void LoadXmlDocument(string xmlFileFullName);
        void ValidateXmlDocument();
    }
}

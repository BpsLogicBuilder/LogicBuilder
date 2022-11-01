using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface ITreeViewXmlDocumentHelper
    {
        XmlDocument BackupXmlTreeDocument { get; }
        XmlDocument XmlTreeDocument { get; }

        void LoadXmlDocument(string xmlFileFullName);
        void ValidateXmlDocument();
    }
}

using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ConnectorData
    {
        public ConnectorData(string index, XmlElement textXmlNode, XmlElement? metaObjectDataXmlNode, ConnectorCategory connectorCategory, XmlElement connectorElement)
        {
            Index = index;
            TextXmlNode = textXmlNode;
            MetaObjectDataXmlNode = metaObjectDataXmlNode;
            ConnectorCategory = connectorCategory;
            ConnectorElement = connectorElement;
        }
        #region Properties
        internal string Index { get; }
        internal XmlElement TextXmlNode { get; }
        internal XmlElement? MetaObjectDataXmlNode { get; }
        internal ConnectorCategory ConnectorCategory { get;}
        internal XmlElement ConnectorElement { get; }
        #endregion Properties
    }
}

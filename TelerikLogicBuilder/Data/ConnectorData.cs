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

        /// <summary>
        /// name attribute of the <connector></connector> element
        /// </summary>
        internal string Index { get; }

        /// <summary>
        /// The <text></text> element
        /// </summary>
        internal XmlElement TextXmlNode { get; }

        /// <summary>
        /// The <metaObject></metaObject> element (should be null for decision connectors)
        /// </summary>
        internal XmlElement? MetaObjectDataXmlNode { get; }

        /// <summary>
        /// connectorCategory attribute of the <connector></connector> element
        /// </summary>
        internal ConnectorCategory ConnectorCategory { get;}

        /// <summary>
        /// <connector></connector> element
        /// </summary>
        internal XmlElement ConnectorElement { get; }
        #endregion Properties
    }
}

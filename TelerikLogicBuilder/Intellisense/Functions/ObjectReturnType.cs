using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ObjectReturnType : ReturnTypeBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ObjectReturnType(IXmlDocumentHelpers xmlDocumentHelpers, string objectType)
        {
            this.ObjectType = objectType;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        internal string ObjectType { get; private set; }
        internal override ReturnTypeCategory ReturnTypeCategory => ReturnTypeCategory.Object;
        internal override string ToXml => this.BuildXml();
        internal override string Description => ObjectType;
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTELEMENT);
                    xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, this.ObjectType);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class CreateLiteralParameterXmlElement : ICreateLiteralParameterXmlElement
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public CreateLiteralParameterXmlElement(IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public XmlElement Create(LiteralParameter literalParameter, string innerXml)
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALPARAMETERELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, literalParameter.Name);
                    xmlTextWriter.WriteRaw(innerXml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return _xmlDocumentHelpers.ToXmlElement(stringBuilder.ToString());
        }
    }
}

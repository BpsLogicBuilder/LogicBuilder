using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class Fragment
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        public Fragment(IXmlDocumentHelpers xmlDocumentHelpers, string name, string xml)
        {
            Name = name;
            Xml = xml;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public string Name { get; set; }
        public string Xml { get; set; }
        public string ToXml => this.BuildXml();

        private string BuildXml()
        {
           StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFormattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.FRAGMENTELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                    xmlTextWriter.WriteRaw(this.Xml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
    }
}

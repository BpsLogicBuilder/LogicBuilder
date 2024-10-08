﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class Fragment
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        public Fragment(IXmlDocumentHelpers xmlDocumentHelpers, string name, string xml, string description)
        {
            Name = name;
            Xml = xml;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Xml { get; set; }
        public string ToXml => this.BuildXml();

        private string BuildXml()
        {
           StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFormattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.FRAGMENTELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.DESCRIPTIONATTRIBUTE, this.Description);
                    xmlTextWriter.WriteRaw(this.Xml);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
    }
}

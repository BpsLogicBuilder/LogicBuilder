using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class GenericReturnType : ReturnTypeBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal GenericReturnType(IXmlDocumentHelpers xmlDocumentHelpers, string genericArgumentName)
        {
            this.GenericArgumentName = genericArgumentName;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        internal string GenericArgumentName { get; private set; }
        internal override ReturnTypeCategory ReturnTypeCategory => ReturnTypeCategory.Generic;
        internal override string ToXml => this.BuildXml();
        internal override string Description => GenericArgumentName;
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.GENERICELEMENT);
                    xmlTextWriter.WriteElementString(XmlDataConstants.GENERICARGUMENTNAMEELEMENT, this.GenericArgumentName);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

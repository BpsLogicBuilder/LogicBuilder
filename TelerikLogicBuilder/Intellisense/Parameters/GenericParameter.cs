using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class GenericParameter : ParameterBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal GenericParameter(
            IXmlDocumentHelpers xmlDocumentHelpers,
            string name,
            bool isOptional,
            string comments,
            string genericArgumentName)
            : base(name, isOptional, comments)
        {
            this.GenericArgumentName = genericArgumentName;
            this._xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        internal string GenericArgumentName { get; private set; }

        internal override ParameterCategory ParameterCategory => ParameterCategory.Generic;

        internal override string ToXml => this.BuildXml();

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, Strings.parameterTypeNameFormat2, this.GenericArgumentName, this.Name);

        internal override string Description => GenericArgumentName;
        #endregion Properties

        #region Methods
        string BuildXml()
        {
            StringBuilder stringBuilder = new();
            XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder);
            xmlTextWriter.WriteStartElement(XmlDataConstants.GENERICPARAMETERELEMENT);
                xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                xmlTextWriter.WriteElementString(XmlDataConstants.GENERICARGUMENTNAMEELEMENT, this.GenericArgumentName);
                xmlTextWriter.WriteElementString(XmlDataConstants.OPTIONALELEMENT, this.IsOptional.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                xmlTextWriter.WriteElementString(XmlDataConstants.COMMENTSELEMENT, this.Comments);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

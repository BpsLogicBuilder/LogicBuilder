using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments
{
    internal class LiteralGenericConfig : GenericConfigBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal LiteralGenericConfig(
            IXmlDocumentHelpers xmlDocumentHelpers,
            string genericArgumentName,
            LiteralParameterType literalType,
            LiteralParameterInputStyle control,
            bool useForEquality,
            bool useForHashCode,
            bool useForToString,
            string propertySource,
            string propertySourceParameter,
            string defaultValue,
            List<string> domain)
            : base(genericArgumentName)
        {
            this.LiteralType = literalType;
            this.Control = control;
            this.UseForEquality = useForEquality;
            this.UseForHashCode = useForHashCode;
            this.UseForToString = useForToString;
            this.PropertySource = propertySource;
            this.PropertySourceParameter = propertySourceParameter;
            this.DefaultValue = defaultValue;
            this.Domain = domain;
            this._xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        internal LiteralParameterType LiteralType { get; private set; }
        internal LiteralParameterInputStyle Control { get; private set; }
        internal bool UseForEquality { get; private set; }
        internal bool UseForHashCode { get; private set; }
        internal bool UseForToString { get; private set; }
        internal string PropertySource { get; private set; }
        internal string PropertySourceParameter { get; private set; }
        internal string DefaultValue { get; private set; }
        internal List<string> Domain { get; private set; }

        internal override GenericConfigCategory GenericConfigCategory => GenericConfigCategory.Literal;
        internal override string ToXml => this.BuildXml();
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALPARAMETERELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE, this.GenericArgumentName);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LITERALTYPEELEMENT, Enum.GetName(typeof(LiteralParameterType), this.LiteralType));
                    xmlTextWriter.WriteElementString(XmlDataConstants.CONTROLELEMENT, Enum.GetName(typeof(LiteralParameterInputStyle), this.Control));
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEFOREQUALITYELEMENT, this.UseForEquality.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEFORHASHCODEELEMENT, this.UseForHashCode.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEFORTOSTRINGELEMENT, this.UseForToString.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    xmlTextWriter.WriteElementString(XmlDataConstants.PROPERTYSOURCEELEMENT, this.PropertySource);
                    xmlTextWriter.WriteElementString(XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT, this.PropertySourceParameter);
                    xmlTextWriter.WriteElementString(XmlDataConstants.DEFAULTVALUEELEMENT, this.DefaultValue);
                    xmlTextWriter.WriteStartElement(XmlDataConstants.DOMAINELEMENT);
                    this.Domain.ForEach(item =>
                    {
                        xmlTextWriter.WriteStartElement(XmlDataConstants.ITEMELEMENT);
                        xmlTextWriter.WriteString(item);
                        xmlTextWriter.WriteEndElement();
                    });
                    xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush(); 
            }

            return stringBuilder.ToString();
        }

        public override int GetHashCode() => GenericArgumentName.GetHashCode();
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;

            return IsEqualTo(obj as LiteralGenericConfig);
            bool IsEqualTo(LiteralGenericConfig? config)
            {
                if (config == null) return false;
                return this.GenericArgumentName == config.GenericArgumentName
                    && this.LiteralType == config.LiteralType;
            }
        }
        #endregion Methods
    }
}

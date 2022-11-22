using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments
{
    internal class LiteralListGenericConfig : GenericConfigBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal LiteralListGenericConfig(
            IXmlDocumentHelpers xmlDocumentHelpers,
            string genericArgumentName,
            LiteralParameterType literalType,
            ListType listType,
            ListParameterInputStyle control,
            LiteralParameterInputStyle elementControl,
            string propertySource,
            string propertySourceParameter,
            List<string> defaultValues,
            List<string> domain)
            : base(genericArgumentName)
        {
            this.LiteralType = literalType;
            this.ListType = listType;
            this.Control = control;
            this.ElementControl = elementControl;
            this.PropertySource = propertySource;
            this.PropertySourceParameter = propertySourceParameter;
            this.DefaultValues = defaultValues;
            this.Domain = domain;
            this._xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        internal LiteralParameterType LiteralType { get; private set; }
        internal ListType ListType { get; private set; }
        internal ListParameterInputStyle Control { get; private set; }
        internal LiteralParameterInputStyle ElementControl { get; private set; }
        internal string PropertySource { get; private set; }
        internal string PropertySourceParameter { get; private set; }
        internal List<string> DefaultValues { get; private set; }
        internal List<string> Domain { get; private set; }

        internal override GenericConfigCategory GenericConfigCategory => GenericConfigCategory.LiteralList;
        internal override string ToXml => this.BuildXml();
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALLISTPARAMETERELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE, this.GenericArgumentName);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LITERALTYPEELEMENT, Enum.GetName(typeof(LiteralParameterType), this.LiteralType));
                    xmlTextWriter.WriteElementString(XmlDataConstants.LISTTYPEELEMENT, Enum.GetName(typeof(ListType), this.ListType));
                    xmlTextWriter.WriteElementString(XmlDataConstants.CONTROLELEMENT, Enum.GetName(typeof(ListParameterInputStyle), this.Control));
                    xmlTextWriter.WriteElementString(XmlDataConstants.ELEMENTCONTROLELEMENT, Enum.GetName(typeof(LiteralParameterInputStyle), this.ElementControl));
                    xmlTextWriter.WriteElementString(XmlDataConstants.PROPERTYSOURCEELEMENT, this.PropertySource);
                    xmlTextWriter.WriteElementString(XmlDataConstants.PROPERTYSOURCEPARAMETERELEMENT, this.PropertySourceParameter);
                    xmlTextWriter.WriteStartElement(XmlDataConstants.DEFAULTVALUEELEMENT);
                    this.DefaultValues.ForEach(item =>
                    {
                        xmlTextWriter.WriteStartElement(XmlDataConstants.ITEMELEMENT);
                        xmlTextWriter.WriteString(item);
                        xmlTextWriter.WriteEndElement();
                    });
                    xmlTextWriter.WriteEndElement();
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

            return IsEqualTo(obj as LiteralListGenericConfig);
            bool IsEqualTo(LiteralListGenericConfig? config)
            {
                if (config == null) return false;
                return this.GenericArgumentName == config.GenericArgumentName
                    && this.LiteralType == config.LiteralType
                    && this.ListType == config.ListType;
            }
        }
        #endregion Methods
    }
}

using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ListOfLiteralsParameter : ParameterBase, ILiteralParameter
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ListOfLiteralsParameter(string name,
            bool isOptional,
            string comments,
            LiteralParameterType literalType,
            ListType listType,
            ListParameterInputStyle control,
            LiteralParameterInputStyle elementControl,
            string fieldSource,
            string fieldSourceProperty,
            List<string> defaultValues,
            char[] defaultValuesDelimiters,
            List<string> domain,
            IContextProvider contextProvider)
            : base(name, isOptional, comments)
        {
            this.LiteralType = literalType;
            this.ListType = listType;
            this.Control = control;
            this.ElementControl = elementControl;
            this.PropertySource = fieldSource;
            this.PropertySourceParameter = fieldSourceProperty;
            this.DefaultValues = defaultValues;
            this.DefaultValuesDelimiters = defaultValuesDelimiters;
            this.Domain = domain;
            this._enumHelper = contextProvider.EnumHelper;
            this._xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        internal LiteralParameterType LiteralType { get; private set; }
        internal ListType ListType { get; private set; }
        internal ListParameterInputStyle Control { get; private set; }
        internal LiteralParameterInputStyle ElementControl { get; private set; }
        internal string PropertySource { get; private set; }
        internal string PropertySourceParameter { get; private set; }
        internal List<string> DefaultValues { get; private set; }
        internal char[] DefaultValuesDelimiters { get; private set; }
        internal List<string> Domain { get; private set; }

        internal override ParameterCategory ParameterCategory => ParameterCategory.LiteralList;

        internal override string ToXml => this.BuildXml();

        public override string ToString() => string.Format
        (
            Strings.listParameterTypeNameFormat,
            Enum.GetName(typeof(ListType), this.ListType), 
            Enum.GetName(typeof(LiteralParameterType), this.LiteralType), 
            this.Name
        );

        internal override string Description => this._enumHelper.GetTypeDescription
        (
            ListType,
            this._enumHelper.GetVisibleEnumText(LiteralType)
        );
        #endregion Properties

        #region ILiteralParameter
        LiteralParameterType ILiteralParameter.LiteralType => this.LiteralType;

        LiteralParameterInputStyle ILiteralParameter.Control => this.ElementControl;

        string ILiteralParameter.PropertySource => this.PropertySource;

        string ILiteralParameter.PropertySourceParameter => this.PropertySourceParameter;

        List<string> ILiteralParameter.Domain => this.Domain;

        string ILiteralParameter.Name => this.Name;

        bool ILiteralParameter.IsOptional => this.IsOptional;

        string ILiteralParameter.Comments => this.Comments;
        #endregion ILiteralParameter

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder);
            xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALLISTPARAMETERELEMENT);
                xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                xmlTextWriter.WriteElementString(XmlDataConstants.LITERALTYPEELEMENT, Enum.GetName(typeof(LiteralParameterType), this.LiteralType));
                xmlTextWriter.WriteElementString(XmlDataConstants.LISTTYPEELEMENT, Enum.GetName(typeof(ListType), this.ListType));
                xmlTextWriter.WriteElementString(XmlDataConstants.CONTROLELEMENT, Enum.GetName(typeof(ListParameterInputStyle), this.Control));
                xmlTextWriter.WriteElementString(XmlDataConstants.ELEMENTCONTROLELEMENT, Enum.GetName(typeof(LiteralParameterInputStyle), this.ElementControl));
                xmlTextWriter.WriteElementString(XmlDataConstants.OPTIONALELEMENT, this.IsOptional.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
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
                xmlTextWriter.WriteElementString(XmlDataConstants.COMMENTSELEMENT, this.Comments);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

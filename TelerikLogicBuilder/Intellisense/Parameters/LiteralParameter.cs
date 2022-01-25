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
    internal class LiteralParameter : ParameterBase, ILiteralParameter, IComparableParameter
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal LiteralParameter(string name,
            bool isOptional,
            string comments,
            ParameterType literalType,
            LiteralParameterInputStyle control,
            bool useForEquality,
            bool useForHashCode,
            bool useForToString,
            string propertySource,
            string propertySourceParameter,
            string defaultValue,
            List<string> domain,
            IContextProvider contextProvider)
            : base(name, isOptional, comments)
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
            this._enumHelper = contextProvider.EnumHelper;
            this._xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        internal ParameterType LiteralType { get; private set; }
        internal LiteralParameterInputStyle Control { get; private set; }
        internal bool UseForEquality { get; private set; }
        internal bool UseForHashCode { get; private set; }
        internal bool UseForToString { get; private set; }
        internal string PropertySource { get; private set; }
        internal string PropertySourceParameter { get; private set; }
        internal string DefaultValue { get; private set; }
        internal List<string> Domain { get; private set; }

        internal override ParameterCategory ParameterCategory => ParameterCategory.Literal;

        internal override string ToXml => this.BuildXml();

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, Strings.parameterTypeNameFormat2, Enum.GetName(typeof(ParameterType), this.LiteralType), this.Name);

        internal override string Description => this._enumHelper.GetVisibleEnumText(LiteralType);
        #endregion Properties

        #region ILiteralParameter
        ParameterType ILiteralParameter.LiteralType => this.LiteralType;

        LiteralParameterInputStyle ILiteralParameter.Control => this.Control;

        string ILiteralParameter.PropertySource => this.PropertySource;

        string ILiteralParameter.PropertySourceParameter => this.PropertySourceParameter;

        List<string> ILiteralParameter.Domain => this.Domain;

        string ILiteralParameter.Name => this.Name;

        bool ILiteralParameter.IsOptional => this.IsOptional;

        string ILiteralParameter.Comments => this.Comments;
        #endregion ILiteralParameter

        #region IComparableParameter
        string IComparableParameter.Name => this.Name;

        bool IComparableParameter.UseForEquality => this.UseForEquality;

        bool IComparableParameter.UseForHashCode => this.UseForHashCode;

        bool IComparableParameter.UseForToString => this.UseForToString;

        ParameterCategory IComparableParameter.ParameterCategory => this.ParameterCategory;
        #endregion IComparableParameter

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder);
            xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALPARAMETERELEMENT);
                xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                xmlTextWriter.WriteElementString(XmlDataConstants.LITERALTYPEELEMENT, Enum.GetName(typeof(ParameterType), this.LiteralType));
                xmlTextWriter.WriteElementString(XmlDataConstants.CONTROLELEMENT, Enum.GetName(typeof(LiteralParameterInputStyle), this.Control));
                xmlTextWriter.WriteElementString(XmlDataConstants.OPTIONALELEMENT, this.IsOptional.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
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
                xmlTextWriter.WriteElementString(XmlDataConstants.COMMENTSELEMENT, this.Comments);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

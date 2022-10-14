using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class ListOfLiteralsVariable : VariableBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ListOfLiteralsVariable(
                    IEnumHelper enumHelper,
                    IStringHelper stringHelper,
                    IXmlDocumentHelpers xmlDocumentHelpers,
                    string name,
                    string memberName,
                    VariableCategory variableCategory,
                    string castVariableAs,
                    string typeName,
                    string referenceName,
                    string referenceDefinition,
                    string castReferenceAs,
                    ReferenceCategories referenceCategory,
                    string comments,
                    LiteralVariableType literalType,
                    ListType listType,
                    ListVariableInputStyle control,
                    LiteralVariableInputStyle elementControl,
                    string propertySource,
                    List<string> defaultValue,
                    List<string> domain)
            : base(enumHelper,
                    stringHelper,
                    name,
                    memberName,
                    variableCategory,
                    castVariableAs,
                    typeName,
                    referenceName,
                    referenceDefinition,
                    castReferenceAs,
                    referenceCategory,
                    comments)
        {
            this.LiteralType = literalType;
            this.ListType = listType;
            this.Control = control;
            this.ElementControl = elementControl;
            this.PropertySource = propertySource;
            this.DefaultValue = defaultValue;
            this.Domain = domain;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        internal LiteralVariableType LiteralType { get; private set; }
        internal ListType ListType { get; private set; }
        internal ListVariableInputStyle Control { get; private set; }
        internal LiteralVariableInputStyle ElementControl { get; private set; }
        internal string PropertySource { get; private set; }
        internal List<string> DefaultValue { get; private set; }
        internal List<string> Domain { get; private set; }

        internal override VariableTypeCategory VariableTypeCategory => VariableTypeCategory.LiteralList;
        internal override string ToXml => this.BuildXml();
        internal override string ObjectTypeString => string.Format(CultureInfo.CurrentCulture, Strings.listVariableTypeFormat, Enum.GetName(typeof(ListType), this.ListType), Enum.GetName(typeof(LiteralVariableType), this.LiteralType));
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALLISTVARIABLEELEMENT);
                    this.WriteBaseElementString(xmlTextWriter);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LITERALTYPEELEMENT, Enum.GetName(typeof(LiteralVariableType), this.LiteralType));
                    xmlTextWriter.WriteElementString(XmlDataConstants.LISTTYPEELEMENT, Enum.GetName(typeof(ListType), this.ListType));
                    xmlTextWriter.WriteElementString(XmlDataConstants.CONTROLELEMENT, Enum.GetName(typeof(ListVariableInputStyle), this.Control));
                    xmlTextWriter.WriteElementString(XmlDataConstants.ELEMENTCONTROLELEMENT, Enum.GetName(typeof(LiteralVariableInputStyle), this.ElementControl));
                    xmlTextWriter.WriteElementString(XmlDataConstants.PROPERTYSOURCEELEMENT, this.PropertySource);
                    xmlTextWriter.WriteStartElement(XmlDataConstants.DEFAULTVALUEELEMENT);
                    this.DefaultValue.ForEach(item =>
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
        #endregion Methods
    }
}

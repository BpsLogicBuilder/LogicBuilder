using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class LiteralVariable : VariableBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal LiteralVariable(string name,
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
                    LiteralVariableInputStyle control,
                    string propertySource,
                    string defaultValue,
                    List<string> domain,
                    IContextProvider contextProvider)
            : base(name,
                    memberName,
                    variableCategory,
                    castVariableAs,
                    typeName,
                    referenceName,
                    referenceDefinition,
                    castReferenceAs,
                    referenceCategory,
                    comments,
                    contextProvider)
        {
            this.LiteralType = literalType;
            this.Control = control;
            this.PropertySource = propertySource;
            this.DefaultValue = defaultValue;
            this.Domain = domain;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        internal LiteralVariableType LiteralType { get; private set; }
        internal LiteralVariableInputStyle Control { get; private set; }
        internal string PropertySource { get; private set; }
        internal string DefaultValue { get; private set; }
        internal List<string> Domain { get; private set; }

        internal override VariableTypeCategory VariableTypeCategory => VariableTypeCategory.Literal;
        internal override string ToXml => this.BuildXml();
        internal override string ObjectTypeString => Enum.GetName(typeof(LiteralVariableType), this.LiteralType);
        #endregion Properties

        #region Methods
        string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALVARIABLEELEMENT);
                    this.WriteBaseElementString(xmlTextWriter);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LITERALTYPEELEMENT, Enum.GetName(typeof(LiteralVariableType), this.LiteralType));
                    xmlTextWriter.WriteElementString(XmlDataConstants.CONTROLELEMENT, Enum.GetName(typeof(LiteralVariableInputStyle), this.Control));
                    xmlTextWriter.WriteElementString(XmlDataConstants.PROPERTYSOURCEELEMENT, this.PropertySource);
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
        #endregion Methods
    }
}

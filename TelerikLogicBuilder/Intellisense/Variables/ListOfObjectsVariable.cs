using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class ListOfObjectsVariable : VariableBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ListOfObjectsVariable(string name,
                    string variableName,
                    VariableCategory variableCategory,
                    string castVariableAs,
                    string typeName,
                    string referenceName,
                    string referenceDefinition,
                    string castReferenceAs,
                    ReferenceCategories referenceCategory,
                    string comments,
                    string objectType,
                    ListType listType,
                    ListVariableInputStyle control,
                    IContextProvider contextProvider)
            : base(name,
                    variableName,
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
            this.ObjectType = objectType;
            this.ListType = listType;
            this.Control = control;
            this._xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        internal string ObjectType { get; private set; }
        internal ListType ListType { get; private set; }
        internal ListVariableInputStyle Control { get; private set; }

        internal override VariableTypeCategory VariableTypeCategory => VariableTypeCategory.ObjectList;
        internal override string ToXml => this.BuildXml();
        internal override string ObjectTypeString => string.Format(CultureInfo.CurrentCulture, Strings.listVariableTypeFormat, Enum.GetName(typeof(ListType), this.ListType), this.ObjectType);
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTLISTVARIABLEELEMENT);
                    this.WriteBaseElementString(xmlTextWriter);
                    xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, this.ObjectType);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LISTTYPEELEMENT, Enum.GetName(typeof(ListType), this.ListType));
                    xmlTextWriter.WriteElementString(XmlDataConstants.CONTROLELEMENT, Enum.GetName(typeof(ListVariableInputStyle), this.Control));
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

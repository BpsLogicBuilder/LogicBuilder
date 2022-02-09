using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal class ObjectVariable : VariableBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ObjectVariable(string name,
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
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        internal string ObjectType { get; private set; }

        internal override VariableTypeCategory VariableTypeCategory => VariableTypeCategory.Object;
        internal override string ToXml => this.BuildXml();
        internal override string ObjectTypeString => this.ObjectType;
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTVARIABLEELEMENT);
                    this.WriteBaseElementString(xmlTextWriter);
                    xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, this.ObjectType);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

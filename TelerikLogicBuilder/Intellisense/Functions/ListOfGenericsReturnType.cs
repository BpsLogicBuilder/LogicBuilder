using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ListOfGenericsReturnType : ReturnTypeBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;

        internal ListOfGenericsReturnType(string genericArgumentName, ListType listType, IContextProvider contextProvider)
        {
            this.GenericArgumentName = genericArgumentName;
            this.ListType = listType;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _enumHelper = contextProvider.EnumHelper;
        }

        #region Properties
        internal string GenericArgumentName { get; private set; }
        internal ListType ListType { get; private set; }
        internal override ReturnTypeCategory ReturnTypeCategory => ReturnTypeCategory.GenericList;
        internal override string ToXml => this.BuildXml();
        internal override string Description => _enumHelper.GetTypeDescription(ListType, GenericArgumentName);
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.GENERICLISTELEMENT);
                    xmlTextWriter.WriteElementString(XmlDataConstants.GENERICARGUMENTNAMEELEMENT, this.GenericArgumentName);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LISTTYPEELEMENT, Enum.GetName(typeof(ListType), this.ListType));
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

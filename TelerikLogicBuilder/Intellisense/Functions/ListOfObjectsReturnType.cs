using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ListOfObjectsReturnType : ReturnTypeBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;

        internal ListOfObjectsReturnType(string objectType, ListType listType, IContextProvider contextProvider)
        {
            this.ObjectType = objectType;
            this.ListType = listType;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _enumHelper = contextProvider.EnumHelper;
        }

        #region Properties
        internal string ObjectType { get; private set; }
        internal ListType ListType { get; private set; }
        internal override ReturnTypeCategory ReturnTypeCategory => ReturnTypeCategory.ObjectList;
        internal override string ToXml => this.BuildXml();
        internal override string Description => _enumHelper.GetTypeDescription(ListType, ObjectType);
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTLISTELEMENT);
                    xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, this.ObjectType);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LISTTYPEELEMENT, Enum.GetName(typeof(ListType), this.ListType));
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ListOfLiteralsReturnType : ReturnTypeBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;

        internal ListOfLiteralsReturnType(
            IEnumHelper enumHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            LiteralFunctionReturnType underlyingLiteralType,
            ListType listType)
        {
            this.UnderlyingLiteralType = underlyingLiteralType;
            this.ListType = listType;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _enumHelper = enumHelper;
        }

        #region Properties
        internal LiteralFunctionReturnType UnderlyingLiteralType { get; private set; }
        internal ListType ListType { get; private set; }
        internal override ReturnTypeCategory ReturnTypeCategory => ReturnTypeCategory.LiteralList;
        internal override string ToXml => this.BuildXml();
        internal override string Description => _enumHelper.GetTypeDescription(ListType, _enumHelper.GetVisibleEnumText(UnderlyingLiteralType));
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALLISTELEMENT);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LITERALTYPEELEMENT, Enum.GetName(typeof(LiteralFunctionReturnType), this.UnderlyingLiteralType));
                    xmlTextWriter.WriteElementString(XmlDataConstants.LISTTYPEELEMENT, Enum.GetName(typeof(ListType), this.ListType));
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}

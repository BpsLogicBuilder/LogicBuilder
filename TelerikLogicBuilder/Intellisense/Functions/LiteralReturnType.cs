using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class LiteralReturnType : ReturnTypeBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal LiteralReturnType(LiteralFunctionReturnType returnType, IContextProvider contextProvider)
        {
            this.ReturnType = returnType;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _enumHelper = contextProvider.EnumHelper;
        }

        #region Properties
        internal LiteralFunctionReturnType ReturnType { get; private set; }
        internal override ReturnTypeCategory ReturnTypeCategory => ReturnTypeCategory.Literal;
        internal override string ToXml => this.BuildXml();
        internal override string Description => _enumHelper.GetVisibleEnumText(ReturnType);
        #endregion Properties

        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.LITERALELEMENT);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LITERALTYPEELEMENT, Enum.GetName(typeof(LiteralFunctionReturnType), this.ReturnType));
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }
    }
}

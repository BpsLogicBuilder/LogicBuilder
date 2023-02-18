using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class LiteralListDataParser : ILiteralListDataParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public LiteralListDataParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public LiteralListData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{BD446244-51A5-4EDC-A756-1B0ED0F01023}");

            return new LiteralListData
            (
                _enumHelper.ParseEnumText<LiteralParameterType>
                (
                    xmlElement.GetAttribute(XmlDataConstants.LITERALTYPEATTRIBUTE)
                ),
                _enumHelper.ParseEnumText<ListType>
                (
                    xmlElement.GetAttribute(XmlDataConstants.LISTTYPEATTRIBUTE)
                ),
                xmlElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE),
                _xmlDocumentHelpers.GetChildElements(xmlElement),
                xmlElement
            );
        }
    }
}

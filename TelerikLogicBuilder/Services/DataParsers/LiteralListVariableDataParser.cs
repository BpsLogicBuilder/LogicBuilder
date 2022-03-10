using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class LiteralListVariableDataParser : ILiteralListVariableDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public LiteralListVariableDataParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _enumHelper = contextProvider.EnumHelper;
        }

        public LiteralListVariableData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.LITERALLISTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{1F4F20C1-714E-486A-A1D9-1B6AAC045A74}");

            return new LiteralListVariableData
            (
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement),
                xmlElement,
                _enumHelper
            );
        }
    }
}

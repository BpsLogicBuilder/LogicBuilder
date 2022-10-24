using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class LiteralListParameterDataParser : ILiteralListParameterDataParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public LiteralListParameterDataParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public LiteralListParameterData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.LITERALLISTPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{3CCFE3CD-38FA-49B7-8384-4EC13A433E21}");

            return new LiteralListParameterData
            (
                xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement),
                xmlElement,
                _enumHelper
            );
        }
    }
}

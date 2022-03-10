using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class LiteralListParameterDataParser : ILiteralListParameterDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public LiteralListParameterDataParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _enumHelper = contextProvider.EnumHelper;
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

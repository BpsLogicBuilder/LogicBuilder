using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ObjectDataParser : IObjectDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public ObjectDataParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _enumHelper = contextProvider.EnumHelper;
        }

        public ObjectData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.OBJECTELEMENT)
                throw _exceptionHelper.CriticalException("{EED534DA-5DB5-4BF8-8964-AFFA6D1AD88B}");

            return new ObjectData
            (
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement),
                xmlElement,
                _enumHelper
            );
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ObjectVariableDataParser : IObjectVariableDataParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ObjectVariableDataParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public ObjectVariableData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.OBJECTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{D8549AE2-BD53-4038-9910-CD2E76B15D50}");

            return new ObjectVariableData
            (
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement),
                xmlElement,
                _enumHelper
            );
        }
    }
}

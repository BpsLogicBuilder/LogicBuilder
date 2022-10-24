using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ObjectParameterDataParser : IObjectParameterDataParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ObjectParameterDataParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public ObjectParameterData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.OBJECTPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{287F7153-F94B-4DA0-B594-63DF421814B5}");

            return new ObjectParameterData
            (
                xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement),
                xmlElement,
                _enumHelper
            );
        }
    }
}

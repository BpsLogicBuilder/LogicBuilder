using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ObjectParameterDataParser : IObjectParameterDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public ObjectParameterDataParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _enumHelper = contextProvider.EnumHelper;
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

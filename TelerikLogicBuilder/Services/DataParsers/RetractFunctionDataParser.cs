using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class RetractFunctionDataParser : IRetractFunctionDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;

        public RetractFunctionDataParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
        }

        public RetractFunctionData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.RETRACTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{D244A138-A179-4CF7-9BE7-9CC5DA23907D}");

            return new RetractFunctionData
            (
                xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                xmlElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE),
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement),
                xmlElement
            );
        }
    }
}

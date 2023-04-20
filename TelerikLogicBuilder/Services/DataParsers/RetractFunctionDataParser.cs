using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class RetractFunctionDataParser : IRetractFunctionDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public RetractFunctionDataParser(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public RetractFunctionData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.RETRACTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{D244A138-A179-4CF7-9BE7-9CC5DA23907D}");

            return new RetractFunctionData
            (
                xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                xmlElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value,
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement),
                xmlElement
            );
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class AssertFunctionDataParser : IAssertFunctionDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public AssertFunctionDataParser(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public AssertFunctionData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.ASSERTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{65F7E278-281E-434D-B0CC-9331FF992719}");

            return new AssertFunctionData
            (
                xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                xmlElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE),
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement, c => c.Name == XmlDataConstants.VARIABLEELEMENT),
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement, c => c.Name == XmlDataConstants.VARIABLEVALUEELEMENT),
                xmlElement
            );
        }
    }
}

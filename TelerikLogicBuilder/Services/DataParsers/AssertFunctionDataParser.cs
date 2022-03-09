using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class AssertFunctionDataParser : IAssertFunctionDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;

        public AssertFunctionDataParser(IContextProvider contextProvider)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
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

using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class VariableValueDataParser : IVariableValueDataParser
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public VariableValueDataParser(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public VariableValueData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.VARIABLEVALUEELEMENT)
                throw _exceptionHelper.CriticalException("{3C060899-E676-4ADD-936B-2CECBE34B276}");

            return new VariableValueData
            (
                _xmlDocumentHelpers.GetSingleChildElement(xmlElement), 
                xmlElement,
                _enumHelper
            );
        }
    }
}

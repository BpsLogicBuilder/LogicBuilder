using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class FunctionsDataParser : IFunctionsDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FunctionsDataParser(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public FunctionsData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.FUNCTIONSELEMENT)
                throw _exceptionHelper.CriticalException("{AAF4F194-0EEA-419B-8021-59901B9B57E9}");

            HashSet<string> functionElementNames = new() 
            {
                XmlDataConstants.FUNCTIONELEMENT, 
                XmlDataConstants.ASSERTFUNCTIONELEMENT, 
                XmlDataConstants.RETRACTFUNCTIONELEMENT 
            };

            return new FunctionsData
            (
                _xmlDocumentHelpers
                    .GetChildElements(xmlElement)
                    .Select(element =>
                    {
                        if (!functionElementNames.Contains(element.Name))
                            throw _exceptionHelper.CriticalException("{18729E23-F745-4762-9430-0E543E6D6719}");

                        return element;
                    }).ToList(),
                xmlElement
            );
        }
    }
}

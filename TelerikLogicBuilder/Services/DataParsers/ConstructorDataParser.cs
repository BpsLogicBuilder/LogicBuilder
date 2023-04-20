using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ConstructorDataParser : IConstructorDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericConfigXmlParser _genericConfigXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConstructorDataParser(
            IExceptionHelper exceptionHelper,
            IGenericConfigXmlParser genericConfigXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _genericConfigXmlParser = genericConfigXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public ConstructorData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                throw _exceptionHelper.CriticalException("{11D60A3B-16EE-401C-83C6-8FB1C1CC059C}");

            return new ConstructorData
            (
                xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                xmlElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value,
                _xmlDocumentHelpers.GetChildElements
                (
                    _xmlDocumentHelpers.GetSingleChildElement
                    (
                        xmlElement, 
                        e => e.Name == XmlDataConstants.GENERICARGUMENTSELEMENT
                    )
                ).Select(e => _genericConfigXmlParser.Parse(e)).ToList(),
                _xmlDocumentHelpers.GetChildElements
                (
                    _xmlDocumentHelpers.GetSingleChildElement
                    (
                        xmlElement, 
                        e => e.Name == XmlDataConstants.PARAMETERSELEMENT
                    )
                ),
                xmlElement
            );
        }
    }
}

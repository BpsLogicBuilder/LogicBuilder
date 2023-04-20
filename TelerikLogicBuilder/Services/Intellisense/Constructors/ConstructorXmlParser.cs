using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors
{
    internal class ConstructorXmlParser : IConstructorXmlParser
    {
        private readonly IConstructorFactory _constructorFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConstructorXmlParser(IConstructorFactory constructorFactory, IExceptionHelper exceptionHelper, IParametersXmlParser parametersXmlParser, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _constructorFactory = constructorFactory;
            _exceptionHelper = exceptionHelper;
            _parametersXmlParser = parametersXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public Constructor Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                throw _exceptionHelper.CriticalException("{09D4676D-0042-46FF-AF14-BF4555B8AE41}");

            return GetConstructor
            (
                _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
            );

            Constructor GetConstructor(Dictionary<string, XmlElement> elements)
                => _constructorFactory.GetConstructor
                (
                    xmlElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                    elements[XmlDataConstants.TYPENAMEELEMENT].InnerText,
                    _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.PARAMETERSELEMENT]).Select(e => _parametersXmlParser.Parse(e)).ToList(),
                    new List<string>(_xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.GENERICARGUMENTSELEMENT]).Select(e => e.InnerText)),
                    elements[XmlDataConstants.SUMMARYELEMENT].InnerText
                );
        }
    }
}

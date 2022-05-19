using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class ProjectPropertiesXmlParserUtility
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IContextProvider _contextProvider;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IApplicationXmlParser _applicationXmlParser;
        private readonly XmlElement xmlElement;

        public ProjectPropertiesXmlParserUtility(XmlElement xmlElement, IContextProvider contextProvider, IApplicationXmlParser applicationXmlParser)
        {
            this.xmlElement = xmlElement;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _contextProvider = contextProvider;
            _applicationXmlParser = applicationXmlParser;
        }

        internal ProjectProperties GetProjectProperties(string projectName, string projectPath)
        {
            if (xmlElement.Name != XmlDataConstants.PROJECTPROPERTIESELEMENT)
                throw _exceptionHelper.CriticalException("{ABC59900-2EED-4171-B617-08E76149B059}");

            return GetProjectProperties
            (
                _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
            );

            ProjectProperties GetProjectProperties(Dictionary<string, XmlElement> elements)
            {
                return new
                (
                    projectName,
                    projectPath,
                    _xmlDocumentHelpers
                        .GetChildElements(elements[XmlDataConstants.APPLICATIONSELEMENT])
                        .Select(applicationElement => _applicationXmlParser.Parse(applicationElement))
                        .OrderBy(a => a.Nickname)
                        .ToDictionary(a => a.Name.ToLowerInvariant()),
                    new HashSet<string>
                    (
                        _xmlDocumentHelpers
                            .GetChildElements(elements[XmlDataConstants.CONNECTOROBJECTTYPESELEMENT])
                            .Select(e => e.InnerText)
                    ),
                    _contextProvider
                );
            }
        }
    }
}

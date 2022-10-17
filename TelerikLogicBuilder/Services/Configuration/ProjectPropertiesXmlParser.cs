using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ProjectPropertiesXmlParser : IProjectPropertiesXmlParser
    {
        private readonly IApplicationXmlParser _applicationXmlParser;
        private readonly IConfigurationItemFactory _configurationItemFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ProjectPropertiesXmlParser(
            IApplicationXmlParser applicationXmlParser,
            IConfigurationItemFactory configurationItemFactory,
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _applicationXmlParser = applicationXmlParser;
            _configurationItemFactory = configurationItemFactory;
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public ProjectProperties GeProjectProperties(XmlElement xmlElement, string projectName, string projectPath)
        {
            if (xmlElement.Name != XmlDataConstants.PROJECTPROPERTIESELEMENT)
                throw _exceptionHelper.CriticalException("{52526EA2-92A1-4057-9B48-EA3B85517BDA}");

            return GetProjectProperties
            (
                _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
            );

            ProjectProperties GetProjectProperties(Dictionary<string, XmlElement> elements)
            {
                return _configurationItemFactory.GetProjectProperties
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
                    )
                );
            }
        }
    }
}

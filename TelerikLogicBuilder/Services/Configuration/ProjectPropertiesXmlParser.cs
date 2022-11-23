using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ProjectPropertiesXmlParser : IProjectPropertiesXmlParser
    {
        private readonly IApplicationXmlParser _applicationXmlParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IProjectPropertiesItemFactory _projectPropertiesItemFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ProjectPropertiesXmlParser(
            IApplicationXmlParser applicationXmlParser,
            IExceptionHelper exceptionHelper,
            IFileIOHelper fileIOHelper,
            IProjectPropertiesItemFactory projectPropertiesItemFactory,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _applicationXmlParser = applicationXmlParser;
            _projectPropertiesItemFactory = projectPropertiesItemFactory;
            _exceptionHelper = exceptionHelper;
            _fileIOHelper = fileIOHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public ProjectProperties GeProjectProperties(XmlElement xmlElement, string projectName, string projectPath)
        {
            if (xmlElement.Name != XmlDataConstants.PROJECTPROPERTIESELEMENT)
                throw _exceptionHelper.CriticalException("{52526EA2-92A1-4057-9B48-EA3B85517BDA}");

            var directoryInfo = _fileIOHelper.GetNewDirectoryInfo(projectPath);
            if (!directoryInfo.Exists)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.projectPathDoesNotExistFormat, projectPath));

            if (directoryInfo.Parent == null)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.projectPathCannotBeTheRootFolderFormat, projectPath));

            return GetProjectProperties
            (
                _xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name)
            );

            ProjectProperties GetProjectProperties(Dictionary<string, XmlElement> elements)
            {
                return _projectPropertiesItemFactory.GetProjectProperties
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

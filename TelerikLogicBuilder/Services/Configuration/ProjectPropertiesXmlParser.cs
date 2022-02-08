using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ProjectPropertiesXmlParser : IProjectPropertiesXmlParser
    {
        private readonly IContextProvider _contextProvider;
        private readonly IApplicationXmlParser _applicationXmlParser;

        public ProjectPropertiesXmlParser(IContextProvider contextProvider, IApplicationXmlParser applicationXmlParser)
        {
            _contextProvider = contextProvider;
            _applicationXmlParser = applicationXmlParser;
        }

        public ProjectProperties GeProjectProperties(XmlElement xmlElement, string projectName, string projectPath) 
            => new ProjectPropertiesXmlParserUtility(xmlElement, _contextProvider, _applicationXmlParser)
                .GetProjectProperties(projectName, projectPath);
    }
}

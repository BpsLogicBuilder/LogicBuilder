using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class ProjectPropertiesXmlParser : IProjectPropertiesXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public ProjectPropertiesXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public ProjectProperties GeProjectProperties(XmlElement xmlElement, string projectName, string projectPath) 
            => new ProjectPropertiesXmlParserUtility(xmlElement, _contextProvider)
                .GetProjectProperties(projectName, projectPath);
    }
}

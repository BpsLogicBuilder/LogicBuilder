using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface IProjectPropertiesXmlParser
    {
        ProjectProperties GeProjectProperties(XmlElement xmlElement, string projectName, string projectPath);
    }
}

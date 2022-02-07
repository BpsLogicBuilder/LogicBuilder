using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface IWebApiDeploymentXmlParser
    {
        WebApiDeployment Parse(XmlElement xmlElement);
    }
}

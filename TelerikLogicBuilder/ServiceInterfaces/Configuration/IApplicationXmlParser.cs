using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface IApplicationXmlParser
    {
        Application Parse(XmlElement xmlElement);
    }
}

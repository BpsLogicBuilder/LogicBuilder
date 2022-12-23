using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface ILoadVariables
    {
        XmlDocument Load();
        XmlDocument Load(string fullPath);
    }
}

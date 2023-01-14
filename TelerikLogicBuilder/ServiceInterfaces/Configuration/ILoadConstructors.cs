using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface ILoadConstructors
    {
        XmlDocument Load();
        XmlDocument Load(string fullPath);
    }
}

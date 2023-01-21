using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface ILoadFunctions
    {
        XmlDocument Load();
        XmlDocument Load(string fullPath);
    }
}

using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface IBuiltInFunctionsLoader
    {
        XmlDocument Load();
    }
}

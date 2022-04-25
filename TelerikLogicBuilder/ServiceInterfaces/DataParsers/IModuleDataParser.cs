using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IModuleDataParser
    {
        string Parse(XmlElement xmlElement);
    }
}

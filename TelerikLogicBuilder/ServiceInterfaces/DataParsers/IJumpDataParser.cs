using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IJumpDataParser
    {
        string Parse(XmlElement xmlElement);
    }
}

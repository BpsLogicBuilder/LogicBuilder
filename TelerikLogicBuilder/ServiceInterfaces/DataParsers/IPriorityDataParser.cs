using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IPriorityDataParser
    {
        int? Parse(XmlElement xmlElement);
    }
}

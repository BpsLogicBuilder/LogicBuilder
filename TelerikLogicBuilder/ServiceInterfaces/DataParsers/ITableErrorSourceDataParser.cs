using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface ITableErrorSourceDataParser
    {
        TableErrorSourceData Parse(XmlElement xmlElement);
    }
}

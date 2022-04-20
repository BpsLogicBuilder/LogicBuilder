using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface ITableErrorSourceDataParser
    {
        TableErrorSourceData Parse(XmlElement xmlElement);
    }
}

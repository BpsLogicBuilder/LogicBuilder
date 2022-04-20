using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IDiagramErrorSourceDataParser
    {
        DiagramErrorSourceData Parse(XmlElement xmlElement);
    }
}

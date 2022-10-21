using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IDiagramErrorSourceDataParser
    {
        DiagramErrorSourceData Parse(XmlElement xmlElement);
    }
}

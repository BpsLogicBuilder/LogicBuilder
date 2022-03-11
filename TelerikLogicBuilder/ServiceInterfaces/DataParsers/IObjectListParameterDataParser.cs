using ABIS.LogicBuilder.FlowBuilder.Data;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IObjectListParameterDataParser
    {
        ObjectListParameterData Parse(XmlElement xmlElement);
    }
}

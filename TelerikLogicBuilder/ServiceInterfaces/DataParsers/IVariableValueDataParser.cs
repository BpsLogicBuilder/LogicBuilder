using ABIS.LogicBuilder.FlowBuilder.Data;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IVariableValueDataParser
    {
        VariableValueData Parse(XmlElement xmlElement);
    }
}

using ABIS.LogicBuilder.FlowBuilder.Data;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IFunctionDataParser
    {
        FunctionData Parse(XmlElement xmlElement);
    }
}

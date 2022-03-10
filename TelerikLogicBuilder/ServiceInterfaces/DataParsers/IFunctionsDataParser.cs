using ABIS.LogicBuilder.FlowBuilder.Data;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IFunctionsDataParser
    {
        FunctionsData Parse(XmlElement xmlElement);
    }
}

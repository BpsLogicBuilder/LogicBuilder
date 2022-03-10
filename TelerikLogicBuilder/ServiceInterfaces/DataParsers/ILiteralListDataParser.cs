using ABIS.LogicBuilder.FlowBuilder.Data;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface ILiteralListDataParser
    {
        LiteralListData Parse(XmlElement xmlElement);
    }
}

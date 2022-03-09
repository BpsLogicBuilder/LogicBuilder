using ABIS.LogicBuilder.FlowBuilder.Data;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers
{
    internal interface IConstructorDataParser
    {
        ConstructorData Parse(XmlElement xmlElement);
    }
}

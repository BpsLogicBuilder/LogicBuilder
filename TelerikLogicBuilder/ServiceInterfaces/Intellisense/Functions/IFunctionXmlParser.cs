using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions
{
    internal interface IFunctionXmlParser
    {
        Function Parse(XmlElement xmlElement);
    }
}

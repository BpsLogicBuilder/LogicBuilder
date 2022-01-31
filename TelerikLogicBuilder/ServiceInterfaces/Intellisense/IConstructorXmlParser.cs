using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense
{
    internal interface IConstructorXmlParser
    {
        Constructor Parse(XmlElement xmlElement);
    }
}

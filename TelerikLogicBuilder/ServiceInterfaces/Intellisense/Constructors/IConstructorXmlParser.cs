using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors
{
    internal interface IConstructorXmlParser
    {
        Constructor Parse(XmlElement xmlElement);
    }
}

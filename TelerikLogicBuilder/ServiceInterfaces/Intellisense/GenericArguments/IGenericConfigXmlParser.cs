using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments
{
    internal interface IGenericConfigXmlParser
    {
        GenericConfigBase Parse(XmlElement xmlElement);
    }
}

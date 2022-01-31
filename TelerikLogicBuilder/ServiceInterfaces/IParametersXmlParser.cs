using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IParametersXmlParser
    {
        ParameterBase Parse(XmlElement xmlElement);
    }
}

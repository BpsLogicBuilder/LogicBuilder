using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense
{
    internal interface IParametersXmlParser
    {
        ParameterBase Parse(XmlElement xmlElement);
    }
}

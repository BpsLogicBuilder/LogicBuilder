using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables
{
    internal interface IVariablesXmlParser
    {
        VariableBase Parse(XmlElement xmlElement);
    }
}

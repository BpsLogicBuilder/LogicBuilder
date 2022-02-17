using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables
{
    internal interface IVariablesXmlParser
    {
        VariableBase Parse(XmlElement xmlElement);
        IDictionary<string, VariableBase> GetVariablesDictionary(XmlDocument xmlDocument);
    }
}

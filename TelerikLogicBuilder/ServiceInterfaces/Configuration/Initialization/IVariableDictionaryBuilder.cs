using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IVariableDictionaryBuilder
    {
        IDictionary<string, VariableBase> GetDictionary(XmlDocument xmlDocument);
    }
}

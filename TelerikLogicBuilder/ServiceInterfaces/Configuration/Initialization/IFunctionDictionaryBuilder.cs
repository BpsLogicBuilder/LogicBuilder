using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IFunctionDictionaryBuilder
    {
        IDictionary<string, Function> GetDictionary(XmlDocument xmlDocument);
    }
}

using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IConstructorDictionaryBuilder
    {
        IDictionary<string, Constructor> GetDictionary(XmlDocument xmlDocument);
    }
}

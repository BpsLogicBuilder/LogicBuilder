using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization
{
    internal interface IFragmentDictionaryBuilder
    {
        IDictionary<string, Fragment> GetDictionary(XmlDocument xmlDocument);
    }
}

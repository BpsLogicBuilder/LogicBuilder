using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders
{
    internal interface IResourcesManager
    {
        string GetShortString(XmlNode xmlNode, IDictionary<string, string> resourceStrings, string moduleName);
        string GetShortString(string longString, IDictionary<string, string> resourceStrings, string moduleName);
    }
}

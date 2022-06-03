using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface ISaveResources
    {
        void Save(string sourceFile, IDictionary<string, string> resourceStrings, string documentTypeFolder);
    }
}

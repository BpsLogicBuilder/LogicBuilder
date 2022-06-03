using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface ISaveDiagramResources
    {
        void Save(string sourceFile, IDictionary<string, string> resourceStrings);
    }
}

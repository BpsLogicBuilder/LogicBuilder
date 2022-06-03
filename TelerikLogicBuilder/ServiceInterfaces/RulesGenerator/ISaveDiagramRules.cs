using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface ISaveDiagramRules
    {
        void Save(string sourceFile, IList<RuleBag> rules);
    }
}

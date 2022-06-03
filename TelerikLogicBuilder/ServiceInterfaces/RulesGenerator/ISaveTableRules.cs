using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using System.Collections.Generic;
using System.Data;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface ISaveTableRules
    {
        void Save(string sourceFile, DataSet dataSet, IList<RuleBag> rules);
    }
}

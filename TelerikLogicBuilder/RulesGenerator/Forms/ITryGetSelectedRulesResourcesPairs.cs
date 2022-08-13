using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ITryGetSelectedRulesResourcesPairs
    {
        bool Try(string selctedApplication, string title, out IList<RulesResourcesPair> sourceFiles);
    }
}

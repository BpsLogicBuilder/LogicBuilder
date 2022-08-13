using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ITryGetSelectedRules
    {
        bool Try(string selctedApplication, string title, out IList<string> selectedRules);
    }
}

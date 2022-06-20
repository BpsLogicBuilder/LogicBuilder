using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ITryGetSelectedRules
    {
        bool Try(string selctedApplication, string title, out IList<string> selectedRules, IWin32Window dialogOwner);
    }
}

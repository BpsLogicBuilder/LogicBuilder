using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms
{
    internal interface ITryGetSelectedRulesResourcesPairs
    {
        bool Try(string selctedApplication, string title, out IList<RulesResourcesPair> sourceFiles, IWin32Window dialogOwner);
    }
}

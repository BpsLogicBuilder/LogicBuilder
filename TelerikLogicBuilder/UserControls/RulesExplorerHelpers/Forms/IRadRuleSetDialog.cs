using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers.Forms
{
    internal interface IRadRuleSetDialog : IDisposable
    {
        void Setup(Type activityType, RuleSet ruleSet, List<Assembly> references);
        DialogResult ShowDialog(IWin32Window owner);
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers
{
    internal class RefreshRulesExplorerCommand : ClickCommandBase
    {
        private readonly IRulesExplorer _rulesExplorer;

        public RefreshRulesExplorerCommand(IRulesExplorer rulesExplorer)
        {
            _rulesExplorer = rulesExplorer;
        }

        public override void Execute()
        {
            _rulesExplorer.RefreshTreeView();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.ConfigurationExplorerHelpers
{
    internal class RefreshConfigurationExplorerCommand : ClickCommandBase
    {
        private readonly IConfigurationExplorer _configurationExplorer;

        public RefreshConfigurationExplorerCommand(IConfigurationExplorer configurationExplorer)
        {
            _configurationExplorer = configurationExplorer;
        }

        public override void Execute()
        {
            _configurationExplorer.RefreshTreeView();
        }
    }
}

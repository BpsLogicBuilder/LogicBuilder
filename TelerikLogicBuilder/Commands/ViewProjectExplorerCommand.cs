using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ViewProjectExplorerCommand : ClickCommandBase
    {
        private readonly IProjectExplorer _projectExplorer;

        public ViewProjectExplorerCommand(IProjectExplorer projectExplorer)
        {
            _projectExplorer = projectExplorer;
        }

        public override void Execute()
        {
            _projectExplorer.Visible = true;
        }
    }
}

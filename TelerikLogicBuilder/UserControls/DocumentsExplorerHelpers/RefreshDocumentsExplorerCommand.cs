using ABIS.LogicBuilder.FlowBuilder.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class RefreshDocumentsExplorerCommand : ClickCommandBase
    {
        private readonly IDocumentsExplorer _documentsExplorer;

        public RefreshDocumentsExplorerCommand(IDocumentsExplorer documentsExplorer)
        {
            _documentsExplorer = documentsExplorer;
        }

        public override void Execute()
        {
            _documentsExplorer.RefreshTreeView();
        }
    }
}

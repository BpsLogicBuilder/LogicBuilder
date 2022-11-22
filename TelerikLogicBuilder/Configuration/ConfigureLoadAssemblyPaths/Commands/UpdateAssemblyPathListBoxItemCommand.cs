using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Commands
{
    internal class UpdateAssemblyPathListBoxItemCommand : ClickCommandBase
    {
        private readonly ILoadAssemblyPathsItemFactory _loadAssemblyPathsItemFactory;
        private readonly IRadListBoxManager<AssemblyPath> radListBoxManager;
        private readonly HelperButtonTextBox txtPath;

        public UpdateAssemblyPathListBoxItemCommand(
            ILoadAssemblyPathsItemFactory loadAssemblyPathsItemFactory,
            IConfigureLoadAssemblyPathsControl loadAssemblyPathsControl)
        {
            _loadAssemblyPathsItemFactory = loadAssemblyPathsItemFactory;
            radListBoxManager = loadAssemblyPathsControl.RadListBoxManager;
            txtPath = loadAssemblyPathsControl.TxtPath;
        }

        public override void Execute()
        {
            radListBoxManager.Update
            (
                _loadAssemblyPathsItemFactory.GetAssemblyPath(txtPath.Text.Trim())
            );
        }
    }
}

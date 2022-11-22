using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Commands
{
    internal class AddAssemblyPathListBoxItemCommand : ClickCommandBase
    {
        private readonly ILoadAssemblyPathsItemFactory _loadAssemblyPathsItemFactory;
        private readonly IRadListBoxManager<AssemblyPath> radListBoxManager;
        private readonly HelperButtonTextBox txtPath;

        public AddAssemblyPathListBoxItemCommand(
            ILoadAssemblyPathsItemFactory loadAssemblyPathsItemFactory,
            IConfigureLoadAssemblyPathsControl loadAssemblyPathsControl)
        {
            _loadAssemblyPathsItemFactory = loadAssemblyPathsItemFactory;
            radListBoxManager = loadAssemblyPathsControl.RadListBoxManager;
            txtPath = loadAssemblyPathsControl.TxtPath;
        }

        public override void Execute()
        {
            radListBoxManager.Add
            (
                _loadAssemblyPathsItemFactory.GetAssemblyPath(txtPath.Text.Trim())
            );
        }
    }
}

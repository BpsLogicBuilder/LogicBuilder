using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands.LoadAssemblyPaths
{
    internal class UpdateAssemblyPathListBoxItemCommand : ClickCommandBase
    {
        private readonly IConfigurationItemFactory _configurationItemFactory;
        private readonly IRadListBoxManager<AssemblyPath> radListBoxManager;
        private readonly HelperButtonTextBox txtPath;

        public UpdateAssemblyPathListBoxItemCommand(
            IConfigurationItemFactory configurationItemFactory,
            ILoadAssemblyPathsControl loadAssemblyPathsControl)
        {
            _configurationItemFactory = configurationItemFactory;
            radListBoxManager = loadAssemblyPathsControl.RadListBoxManager;
            txtPath = loadAssemblyPathsControl.TxtPath;
        }

        public override void Execute()
        {
            radListBoxManager.Update
            (
                _configurationItemFactory.GetAssemblyPath(txtPath.Text.Trim())
            );
        }
    }
}

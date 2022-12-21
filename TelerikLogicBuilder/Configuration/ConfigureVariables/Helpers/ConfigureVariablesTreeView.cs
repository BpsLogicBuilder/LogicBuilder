using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal class ConfigureVariablesTreeView : RadTreeView
    {
        public ConfigureVariablesTreeView(
            IConfigureVariablesFactory configureVariablesFactory,
            IConfigureVariablesForm configureVariablesForm)
        {
            ConfigureVariablesDragDropHandler = configureVariablesFactory.GetConfigureVariablesDragDropHandler(configureVariablesForm);
        }

        public IConfigureVariablesDragDropHandler ConfigureVariablesDragDropHandler { get; }

        protected override RadTreeViewElement CreateTreeViewElement()
            => new ConfigureVariablesTreeViewElement();

        public override string ThemeClassName
            => typeof(RadTreeView).FullName!;
    }
}

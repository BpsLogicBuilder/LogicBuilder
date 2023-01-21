using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers
{
    internal class ConfigureFunctionsTreeView : RadTreeView
    {
        public ConfigureFunctionsTreeView(
            IConfigureFunctionsFactory configureFunctionsFactory,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            ConfigureFunctionsDragDropHandler = configureFunctionsFactory.GetConfigureFunctionsDragDropHandler(configureFunctionsForm);
        }

        public IConfigureFunctionsDragDropHandler ConfigureFunctionsDragDropHandler { get; }

        protected override RadTreeViewElement CreateTreeViewElement()
            => new ConfigureFunctionsTreeViewElement();

        public override string ThemeClassName
            => typeof(RadTreeView).FullName!;
    }
}

using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers
{
    internal class ConfigureConstructorsTreeView : RadTreeView
    {
        public ConfigureConstructorsTreeView(
            IConfigureConstructorsFactory configureConstructorsFactory,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            ConfigureConstructorsDragDropHandler = configureConstructorsFactory.GetConfigureConstructorsDragDropHandler(configureConstructorsForm);
        }

        public IConfigureConstructorsDragDropHandler ConfigureConstructorsDragDropHandler { get; }

        protected override RadTreeViewElement CreateTreeViewElement()
            => new ConfigureConstructorsTreeViewElement();

        public override string ThemeClassName
            => typeof(RadTreeView).FullName!;
    }
}

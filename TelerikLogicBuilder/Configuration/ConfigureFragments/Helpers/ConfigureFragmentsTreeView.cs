using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers
{
    internal class ConfigureFragmentsTreeView : RadTreeView
    {
        public ConfigureFragmentsTreeView(
            IConfigureFragmentsFactory configureFragmentsFactory,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            ConfigureFragmentsDragDropHandler = configureFragmentsFactory.GetConfigureFragmentsDragDropHandler(configureFragmentsForm);
        }

        public IConfigureFragmentsDragDropHandler ConfigureFragmentsDragDropHandler { get; }

        protected override RadTreeViewElement CreateTreeViewElement()
            => new ConfigureFragmentsTreeViewElement();

        public override string ThemeClassName
            => typeof(RadTreeView).FullName!;
    }
}

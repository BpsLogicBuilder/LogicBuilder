using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers
{
    internal class ConfigureFragmentsTreeViewElement : RadTreeViewElement
    {
        protected override Type ThemeEffectiveType
            => typeof(RadTreeViewElement);

        protected override TreeViewDragDropService CreateDragDropService()
            => new ConfigureFragmentsDragDropService(this);
    }
}

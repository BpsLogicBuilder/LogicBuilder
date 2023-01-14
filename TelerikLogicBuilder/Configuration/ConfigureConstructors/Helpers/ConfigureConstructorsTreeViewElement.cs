using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers
{
    internal class ConfigureConstructorsTreeViewElement : RadTreeViewElement
    {
        protected override Type ThemeEffectiveType
            => typeof(RadTreeViewElement);

        protected override TreeViewDragDropService CreateDragDropService()
            => new ConfigureConstructorsDragDropService(this);
    }
}

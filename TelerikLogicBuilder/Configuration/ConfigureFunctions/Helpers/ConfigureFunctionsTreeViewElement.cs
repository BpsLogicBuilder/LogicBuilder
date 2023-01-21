using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers
{
    internal class ConfigureFunctionsTreeViewElement : RadTreeViewElement
    {
        protected override Type ThemeEffectiveType
            => typeof(RadTreeViewElement);

        protected override TreeViewDragDropService CreateDragDropService()
            => new ConfigureFunctionsDragDropService(this);
    }
}

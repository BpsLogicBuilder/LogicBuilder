using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal class ConfigureVariablesTreeViewElement : RadTreeViewElement
    {
        protected override Type ThemeEffectiveType
            => typeof(RadTreeViewElement);

        protected override TreeViewDragDropService CreateDragDropService()
            => new ConfigureVariablesDragDropService(this);
    }
}

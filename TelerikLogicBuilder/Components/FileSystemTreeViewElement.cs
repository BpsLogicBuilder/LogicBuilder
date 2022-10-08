using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal class FileSystemTreeViewElement : RadTreeViewElement
    {
        protected override Type ThemeEffectiveType 
            => typeof(RadTreeViewElement);

        protected override TreeViewDragDropService CreateDragDropService() 
            => new FileSystemDragDropService(this);
    }
}

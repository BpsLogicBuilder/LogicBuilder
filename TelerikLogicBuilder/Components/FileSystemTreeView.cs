using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal class FileSystemTreeView : RadTreeView
    {
        public FileSystemTreeView(IFileSystemDragDropHandler fileSystemDragDropHandler)
        {
            FileSystemDragDropHandler = fileSystemDragDropHandler;
        }

        public IFileSystemDragDropHandler FileSystemDragDropHandler { get; }

        protected override RadTreeViewElement CreateTreeViewElement() 
            => new FileSystemTreeViewElement();

        public override string ThemeClassName 
            => typeof(RadTreeView).FullName!;
    }
}

using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal class FileSystemTreeView : RadTreeView
    {
        public FileSystemTreeView()
        {//Transient components have been known to stay referenced by the DI container.
            //https://github.com/dotnet/aspnetcore/issues/5496
            FileSystemDragDropHandler = Program.ServiceProvider.GetRequiredService<IFileSystemDragDropHandler>(); ;
        }

        public IFileSystemDragDropHandler FileSystemDragDropHandler { get; }

        protected override RadTreeViewElement CreateTreeViewElement() 
            => new FileSystemTreeViewElement();

        public override string ThemeClassName 
            => typeof(RadTreeView).FullName!;
    }
}

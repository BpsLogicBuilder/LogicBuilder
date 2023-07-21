using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Components
{
    internal interface IFileSystemTreeView
    {
        IFileSystemDragDropHandler FileSystemDragDropHandler { get; }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ComponentsServices
    {
        internal static IServiceCollection AddComponents(this IServiceCollection services)
        {
            return services
                .AddSingleton<IFileSystemDragDropHandler, FileSystemDragDropHandler>()
                .AddTransient<FileSystemTreeView>()
                .AddTransient<IFileSystemDragDropHandler, FileSystemDragDropHandler>()
                .AddTransient<ObjectRichTextBox>()
                .AddTransient<RichInputBox>();
        }
    }
}

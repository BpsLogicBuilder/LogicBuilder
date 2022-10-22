using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class TreeViewBuiildersServices
    {
        internal static IServiceCollection AddTreeViewBuiilders(this IServiceCollection services) 
            => services
                .AddSingleton<IConfigurationExplorerTreeViewBuilder, ConfigurationExplorerTreeViewBuilder>()
                .AddSingleton<IEmptyFolderRemover, EmptyFolderRemover>()
                .AddSingleton<IGetAllCheckedNodes, GetAllCheckedNodes>()
                .AddSingleton<ISelectDocunentsTreeViewBuilder, SelectDocunentsTreeViewBuilder>()
                .AddSingleton<ISelectModulesForDeploymentTreeViewBuilder, SelectModulesForDeploymentTreeViewBuilder>()
                .AddSingleton<ISelectRulesTreeViewBuilder, SelectRulesTreeViewBuilder>()
                .AddTreeViewBuiilderFactories();
    }
}

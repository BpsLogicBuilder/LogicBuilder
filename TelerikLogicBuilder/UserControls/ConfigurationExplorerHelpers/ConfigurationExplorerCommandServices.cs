using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.ConfigurationExplorerHelpers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationExplorerCommandServices
    {
        internal static IServiceCollection AddConfiguratioExplorerCommands(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigurationExplorer, EditConfigurationCommand>>
                (
                    provider =>
                    configurationExplorer => ActivatorUtilities.CreateInstance<EditConfigurationCommand>
                    (
                        provider,
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IEditConstructors>(),
                        provider.GetRequiredService<IEditFragments>(),
                        provider.GetRequiredService<IEditFunctions>(),
                        provider.GetRequiredService<IEditProjectProperties>(),
                        provider.GetRequiredService<IEditVariables>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        configurationExplorer
                    )
                )
                .AddTransient<Func<IConfigurationExplorer, RefreshConfigurationExplorerCommand>>
                (
                    provider =>
                    configurationExplorer => ActivatorUtilities.CreateInstance<RefreshConfigurationExplorerCommand>
                    (
                        provider,
                        configurationExplorer
                    )
                );
        }
    }
}

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
                .AddTransient<EditConfigurationCommand>()
                .AddTransient<RefreshConfigurationExplorerCommand>();
        }
    }
}

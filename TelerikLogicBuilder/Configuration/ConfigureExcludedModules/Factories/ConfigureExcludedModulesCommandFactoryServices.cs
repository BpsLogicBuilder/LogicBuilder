using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Factories
{
    internal static class ConfigureExcludedModulesCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureExcludedModulesCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigureExcludedModulesCommandFactory, ConfigureExcludedModulesCommandFactory>()
                .AddTransient<Func<IConfigureExcludedModules, UpdateExcludedModulesCommand>>
                (
                    provider =>
                    configureExcludedModules => new UpdateExcludedModulesCommand
                    (
                        provider.GetRequiredService<IGetAllCheckedNodes>(),
                        configureExcludedModules
                    )
                );
        }
    }
}

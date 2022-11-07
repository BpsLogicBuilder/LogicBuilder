using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System;

namespace Microsoft.Extensions.DependencyInjection
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

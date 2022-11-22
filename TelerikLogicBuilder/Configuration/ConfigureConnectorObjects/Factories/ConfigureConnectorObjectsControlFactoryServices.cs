using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureConnectorObjectsControlFactoryServices
    {
        internal static IServiceCollection AddConfigureConnectorObjectsControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConnectorObjectsForm, IConfigureConnectorObjectsControl>>
                (
                    provider =>
                    configureConnectorObjectsForm => new ConfigureConnectorObjectsControl
                    (
                        provider.GetRequiredService<IConfigureConnectorObjectsCommandFactory>(),
                        provider.GetRequiredService<IConnectorObjectsItemFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        configureConnectorObjectsForm
                    )
                )
                .AddTransient<IConfigureConnectorObjectsControlFactory, ConfigureConnectorObjectsControlFactory>();
        }
    }
}

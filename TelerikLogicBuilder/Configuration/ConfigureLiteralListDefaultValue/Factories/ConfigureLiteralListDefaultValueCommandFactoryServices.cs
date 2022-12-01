using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLiteralListDefaultValueCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureLiteralListDefaultValueCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureLiteralListDefaultValueControl, AddLiteralListDefaultValueItemCommand>>
                (
                    provider =>
                    configureLiteralListDefaultValueControl => new AddLiteralListDefaultValueItemCommand
                    (
                        provider.GetRequiredService<ILiteralListDefaultValueItemFactory>(),
                        configureLiteralListDefaultValueControl
                    )
                )
                .AddTransient<IConfigureLiteralListDefaultValueCommandFactory, ConfigureLiteralListDefaultValueCommandFactory>()
                .AddTransient<Func<IConfigureLiteralListDefaultValueControl, UpdateLiteralListDefaultValueItemCommand>>
                (
                    provider =>
                    configureLiteralListDefaultValueControl => new UpdateLiteralListDefaultValueItemCommand
                    (
                        provider.GetRequiredService<ILiteralListDefaultValueItemFactory>(),
                        configureLiteralListDefaultValueControl
                    )
                );
        }
    }
}

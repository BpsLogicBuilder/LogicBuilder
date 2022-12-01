using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLiteralDomainCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureLiteralDomainCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureLiteralDomainControl, AddLiteralDomainListBoxItemCommand>>
                (
                    provider =>
                    configureLiteralDomainControl => new AddLiteralDomainListBoxItemCommand
                    (
                        provider.GetRequiredService<ILiteralDomainItemFactory>(),
                        configureLiteralDomainControl
                    )
                )
                .AddTransient<IConfigureLiteralDomainCommandFactory, ConfigureLiteralDomainCommandFactory>()
                .AddTransient<Func<IConfigureLiteralDomainControl, UpdateLiteralDomainListBoxItemCommand>>
                (
                    provider =>
                    configureLiteralDomainControl => new UpdateLiteralDomainListBoxItemCommand
                    (
                        provider.GetRequiredService<ILiteralDomainItemFactory>(),
                        configureLiteralDomainControl
                    )
                );
        }
    }
}

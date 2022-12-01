using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLiteralDomainControlFactoryServices
    {
        internal static IServiceCollection AddConfigureLiteralDomainControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureLiteralDomainForm, IConfigureLiteralDomainControl>>
                (
                    provider =>
                    configureLiteralDomainForm => new ConfigureLiteralDomainControl
                    (
                        provider.GetRequiredService<IConfigureLiteralDomainCommandFactory>(),
                        provider.GetRequiredService<IGetPromptForLiteralDomainUpdate>(),
                        provider.GetRequiredService<ILiteralDomainItemFactory>(),
                        configureLiteralDomainForm
                    )
                )
                .AddTransient<IConfigureLiteralDomainControlFactory, ConfigureLiteralDomainControlFactory>();
        }
    }
}

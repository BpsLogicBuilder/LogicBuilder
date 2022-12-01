using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureLiteralListDefaultValueControlFactoryServices
    {
        internal static IServiceCollection AddConfigureLiteralListDefaultValueControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureLiteralListDefaultValueForm, IConfigureLiteralListDefaultValueControl>>
                (
                    provider =>
                    configureLiteralListDefaultValueForm => new ConfigureLiteralListDefaultValueControl
                    (
                        provider.GetRequiredService<IConfigureLiteralListDefaultValueCommandFactory>(),
                        provider.GetRequiredService<IGetPromptForLiteralDomainUpdate>(),
                        provider.GetRequiredService<ILiteralListDefaultValueItemFactory>(),
                        configureLiteralListDefaultValueForm
                    )
                )
                .AddTransient<IConfigureLiteralListDefaultValueControlFactory, ConfigureLiteralListDefaultValueControlFactory>();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class LiteralDomainItemFactoryServices
    {
        internal static IServiceCollection AddLiteralDomainItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, Type, LiteralDomainItem>>
                (
                    provider =>
                    (item, type) => new LiteralDomainItem
                    (
                        provider.GetRequiredService<ITypeHelper>(),
                        item, 
                        type
                    )
                )
                .AddTransient<ILiteralDomainItemFactory, LiteralDomainItemFactory>();
        }
    }
}

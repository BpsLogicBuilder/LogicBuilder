using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class LiteralListDefaultValueItemFactoryServices
    {
        internal static IServiceCollection AddLiteralListDefaultValueItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, Type, LiteralListDefaultValueItem>>
                (
                    provider =>
                    (item, type) => new LiteralListDefaultValueItem
                    (
                        provider.GetRequiredService<ITypeHelper>(),
                        item,
                        type
                    )
                )
                .AddTransient<ILiteralListDefaultValueItemFactory, LiteralListDefaultValueItemFactory>();
        }
    }
}

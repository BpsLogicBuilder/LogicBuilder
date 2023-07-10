using ABIS.LogicBuilder.FlowBuilder.AttributeReaders.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class AttributeReaderFactoryServices
    {
        internal static IServiceCollection AddAttributeReaderFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IAttributeReaderFactory, AttributeReaderFactory>();
        }
    }
}

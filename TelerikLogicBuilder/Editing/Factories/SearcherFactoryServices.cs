using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class SearcherFactoryServices
    {
        internal static IServiceCollection AddSearcherFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<ISearcherFactory, SearcherFactory>();
        }
    }
}

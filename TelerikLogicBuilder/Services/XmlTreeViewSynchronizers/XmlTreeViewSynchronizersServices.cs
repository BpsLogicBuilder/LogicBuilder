namespace Microsoft.Extensions.DependencyInjection
{
    internal static class XmlTreeViewSynchronizersServices
    {
        internal static IServiceCollection AddXmlTreeViewSynchronizers(this IServiceCollection services)
        {
            return services
                .AddXmlTreeViewSynchronizerFactories();
        }
    }
}

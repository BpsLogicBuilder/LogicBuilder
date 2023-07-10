using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditXmlHelperFactoryServices
    {
        internal static IServiceCollection AddEditXmlHelperFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditXmlHelperFactory, EditXmlHelperFactory>();
        }
    }
}

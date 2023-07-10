using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditXmlFormFactoryServices
    {
        internal static IServiceCollection AddEditXmlFormFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditXmlFormFactory, EditXmlFormFactory>();
        }
    }
}

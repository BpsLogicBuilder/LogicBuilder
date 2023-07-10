using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FieldControlHelperFactoryServices
    {
        internal static IServiceCollection AddFieldControlHelperFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IFieldControlHelperFactory, FieldControlHelperFactory>();
        }
    }
}

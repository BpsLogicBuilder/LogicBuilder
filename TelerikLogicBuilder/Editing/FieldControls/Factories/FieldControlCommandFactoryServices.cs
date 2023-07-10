using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FieldControlCommandFactoryServices
    {
        internal static IServiceCollection AddFieldControlCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IFieldControlCommandFactory, FieldControlCommandFactory>();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ParameterFieldControlFactoryServices
    {
        internal static IServiceCollection AddParameterFieldControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IParameterFieldControlFactory, ParameterFieldControlFactory>();
        }
    }
}

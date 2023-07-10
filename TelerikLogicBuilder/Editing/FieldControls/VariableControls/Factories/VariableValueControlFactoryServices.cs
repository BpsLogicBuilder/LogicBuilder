using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class VariableValueControlFactoryServices
    {
        internal static IServiceCollection AddVariableValueControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IVariableValueControlFactory, VariableValueControlFactory>();
        }
    }
}

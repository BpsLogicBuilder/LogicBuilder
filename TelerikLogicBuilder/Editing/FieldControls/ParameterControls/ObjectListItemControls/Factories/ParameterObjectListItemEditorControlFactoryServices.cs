using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.ObjectListItemControls.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ParameterObjectListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddParameterObjectListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IParameterObjectListItemEditorControlFactory, ParameterObjectListItemEditorControlFactory>();
        }
    }
}

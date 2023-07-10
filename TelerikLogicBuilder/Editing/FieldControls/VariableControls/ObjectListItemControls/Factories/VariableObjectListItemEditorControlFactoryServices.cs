using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.ObjectListItemControls.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class VariableObjectListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddVariableObjectListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IVariableObjectListItemEditorControlFactory, VariableObjectListItemEditorControlFactory>();
        }
    }
}

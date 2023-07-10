using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.LiteralListItemControls.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class VariableLiteralListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddVariableLiteralListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IVariableLiteralListItemEditorControlFactory, VariableLiteralListItemEditorControlFactory>();
        }
    }
}

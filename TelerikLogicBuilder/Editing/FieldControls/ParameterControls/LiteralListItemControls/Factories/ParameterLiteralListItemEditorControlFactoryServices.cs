using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.LiteralListItemControls.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ParameterLiteralListItemEditorControlFactoryServices
    {
        internal static IServiceCollection AddParameterLiteralListItemEditorControlFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IParameterLiteralListItemEditorControlFactory, ParameterLiteralListItemEditorControlFactory>();
        }
    }
}

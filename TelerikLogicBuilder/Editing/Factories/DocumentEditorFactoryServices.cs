using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DocumentEditorFactoryServices
    {
        internal static IServiceCollection AddDocumentEditorFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDocumentEditorFactory, DocumentEditorFactory>();
        }
    }
}

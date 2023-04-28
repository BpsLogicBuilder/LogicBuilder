using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditCell;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditShape;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DocumentEditorFactoryServices
    {
        internal static IServiceCollection AddDocumentEditorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IDocumentEditorFactory, DocumentEditorFactory>()
                .AddTransient<Func<string, bool, TableControl>>
                (
                    provider =>
                    (tableSourceFile, openedAsReadOnly) => new TableControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<ITableEditor>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IUiNotificationService>(),
                        tableSourceFile,
                        openedAsReadOnly
                    )
                )
                .AddTransient<Func<string, bool, VisioControl>>
                (
                    provider =>
                    (visioSourceFile, openedAsReadOnly) => new VisioControl
                    (
                        provider.GetRequiredService<IDiagramEditor>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IUiNotificationService>(),
                        visioSourceFile,
                        openedAsReadOnly
                    )
                );
        }
    }
}

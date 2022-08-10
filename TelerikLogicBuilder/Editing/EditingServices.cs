using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Editing.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingServices
    {
        internal static IServiceCollection AddEditingControls(this IServiceCollection services)
        {
            return services
                .AddSingleton<IFindAndReplaceHelper, FindAndReplaceHelper>()
                .AddTransient<FindCell>()
                .AddTransient<FindConstructorInCell>()
                .AddTransient<FindConstructorInShape>()
                .AddTransient<FindFunctionInCell>()
                .AddTransient<FindFunctionInShape>()
                .AddTransient<FindShape>()
                .AddTransient<FindTextInCell>()
                .AddTransient<FindTextInShape>()
                .AddTransient<FindVariableInCell>()
                .AddTransient<FindVariableInShape>()
                .AddSingleton<IFunctionsFormFieldSetHelper, FunctionsFormFieldSetHelper>()
                .AddSingleton<ISearchFunctions, SearchFunctions>()
                .AddTransient<Func<string, bool, TableControl>>
                (
                    provider =>
                    (tableSourceFile, openedAsReadOnly) => ActivatorUtilities.CreateInstance<TableControl>
                    (
                        provider,
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        tableSourceFile,
                        openedAsReadOnly
                    )
                )
                .AddTransient<Func<string, bool, VisioControl>>
                (
                    provider =>
                    (visioSourceFile, openedAsReadOnly) => ActivatorUtilities.CreateInstance<VisioControl>
                    (
                        provider,
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        visioSourceFile,
                        openedAsReadOnly
                    )
                );
        }
    }
}

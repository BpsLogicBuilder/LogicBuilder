using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using Telerik.WinControls.UI;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class MdiParentCommandServices
    {
        internal static IServiceCollection AddMdiParentCommands(this IServiceCollection services)
        {
            return services.AddTransient<BuildActiveDocumentCommand>()
            .AddTransient<BuildSaveConsolidateSelectedDocumentsCommand>()
            .AddTransient<Func<IMDIParent, string, BuildActiveDocumentCommand>>
            (
                provider =>
                (mdiParent, sourceFile) => ActivatorUtilities.CreateInstance<BuildActiveDocumentCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<IBuildSaveAssembleRulesForSelectedDocuments>(),
                    mdiParent,
                    sourceFile
                )
            )
            .AddTransient<Func<IMDIParent, string, DeleteSelectedFilesFromApiCommand>>
            (
                provider =>
                (mdiParent, applicationName) => ActivatorUtilities.CreateInstance<DeleteSelectedFilesFromApiCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<ITryGetSelectedRulesResourcesPairs>(),
                    provider.GetRequiredService<IDeleteSelectedFilesFromApi>(),
                    mdiParent,
                    applicationName
                )
            )
            .AddTransient<Func<IMDIParent, string, DeleteSelectedFilesFromFileSystemCommand>>
            (
                provider =>
                (mdiParent, applicationName) => ActivatorUtilities.CreateInstance<DeleteSelectedFilesFromFileSystemCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<ITryGetSelectedRulesResourcesPairs>(),
                    provider.GetRequiredService<IDeleteSelectedFilesFromFileSystem>(),
                    mdiParent,
                    applicationName
                )
            )
            .AddTransient<Func<IMDIParent, string, DeploySelectedFilesToApiCommand>>
            (
                provider =>
                (mdiParent, applicationName) => ActivatorUtilities.CreateInstance<DeploySelectedFilesToApiCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<ITryGetSelectedRulesResourcesPairs>(),
                    provider.GetRequiredService<IDeploySelectedFilesToApi>(),
                    mdiParent,
                    applicationName
                )
            )
            .AddTransient<Func<IMDIParent, string, DeploySelectedFilesToFileSystemCommand>>
            (
                provider =>
                (mdiParent, applicationName) => ActivatorUtilities.CreateInstance<DeploySelectedFilesToFileSystemCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<ITryGetSelectedRulesResourcesPairs>(),
                    provider.GetRequiredService<IDeploySelectedFilesToFileSystem>(),
                    mdiParent,
                    applicationName
                )
            )
            .AddTransient<ExitCommand>()
            .AddTransient<FindConstructorCommand>()
            .AddTransient<FindConstructorInFilesCommand>()
            .AddTransient<FindCellCommand>()
            .AddTransient<FindFunctionCommand>()
            .AddTransient<FindFunctionInFilesCommand>()
            .AddTransient<FindShapeCommand>()
            .AddTransient<FindTextCommand>()
            .AddTransient<FindTextInFilesCommand>()
            .AddTransient<FindVariableCommand>()
            .AddTransient<FindVariableInFilesCommand>()
            .AddTransient<Func<RadMenuItem, string, SetSelectedApplicationCommand>>
            (
                provider =>
                (selectApplicationMenuItem, applicationName) => ActivatorUtilities.CreateInstance<SetSelectedApplicationCommand>
                (
                    provider,
                    provider.GetRequiredService<ICheckSelectedApplication>(),
                    provider.GetRequiredService<IConfigurationService>(),
                    selectApplicationMenuItem,
                    applicationName
                )
            )
            .AddTransient<Func<RadMenuItem, string, SetThemeCommand>>
            (
                provider =>
                (themeMenuItem, themeName) => ActivatorUtilities.CreateInstance<SetThemeCommand>
                (
                    provider,
                    provider.GetRequiredService<IThemeManager>(),
                    themeMenuItem,
                    themeName
                )
            )
            .AddTransient<ValidateSelectedDocumentsCommand>()
            .AddTransient<ValidateActiveDocumentCommand>()
            .AddTransient<Func<IMDIParent, string, ValidateSelectedRulesCommand>>
            (
                provider =>
                (mdiParent, applicationName) => ActivatorUtilities.CreateInstance<ValidateSelectedRulesCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<ITryGetSelectedRules>(),
                    provider.GetRequiredService<IValidateSelectedRules>(),
                    mdiParent,
                    applicationName
                )
            )
            .AddTransient<ViewApplicationsStencilCommand>()
            .AddTransient<ViewFlowDiagramStencilCommand>()
            .AddTransient<Func<IMessages, ViewMessagesCommand>>
            (
                provider =>
                messages => ActivatorUtilities.CreateInstance<ViewMessagesCommand>
                (
                    provider,
                    messages
                )
            )
            .AddTransient<Func<IProjectExplorer, ViewProjectExplorerCommand>>
            (
                provider =>
                projectExplorer => ActivatorUtilities.CreateInstance<ViewProjectExplorerCommand>
                (
                    provider,
                    projectExplorer
                )
            );
        }
    }
}

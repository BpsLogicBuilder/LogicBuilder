using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using System;
using Telerik.WinControls.UI;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class MdiParentCommandServices
    {
        internal static IServiceCollection AddMdiParentCommands(this IServiceCollection services)
        {
            return services.AddTransient<Func<IMDIParent, BuildActiveDocumentCommand>>
            (
                provider =>
                mdiParent => ActivatorUtilities.CreateInstance<BuildActiveDocumentCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<IBuildSaveAssembleRulesForSelectedDocuments>(),
                    provider.GetRequiredService<UiNotificationService>(),
                    mdiParent
                )
            )
            .AddTransient<Func<MDIParent, BuildSaveConsolidateSelectedDocumentsCommand>>
            (
                provider =>
                mdiParent => ActivatorUtilities.CreateInstance<BuildSaveConsolidateSelectedDocumentsCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<ITryGetSelectedDocuments>(),
                    provider.GetRequiredService<IBuildSaveAssembleRulesForSelectedDocuments>(),
                    provider.GetRequiredService<UiNotificationService>(),
                    mdiParent
                )
            )
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
            .AddTransient<Func<MDIParent, string, DeleteSelectedFilesFromApiCommand>>
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
            .AddTransient<Func<MDIParent, string, DeleteSelectedFilesFromFileSystemCommand>>
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
            .AddTransient<Func<MDIParent, string, DeploySelectedFilesToApiCommand>>
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
            .AddTransient<Func<MDIParent, string, DeploySelectedFilesToFileSystemCommand>>
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
            .AddTransient<Func<MDIParent, ValidateSelectedDocumentsCommand>>
            (
                provider =>
                mdiParent => ActivatorUtilities.CreateInstance<ValidateSelectedDocumentsCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<ITryGetSelectedDocuments>(),
                    provider.GetRequiredService<IValidateSelectedDocuments>(),
                    mdiParent
                )
            )
            .AddTransient<Func<IMDIParent, ValidateActiveDocumentCommand>>
            (
                provider =>
                mdiParent => ActivatorUtilities.CreateInstance<ValidateActiveDocumentCommand>
                (
                    provider,
                    provider.GetRequiredService<IConfigurationService>(),
                    provider.GetRequiredService<IValidateSelectedDocuments>(),
                    mdiParent
                )
            )
            .AddTransient<Func<MDIParent, string, ValidateSelectedRulesCommand>>
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
            );
        }
    }
}

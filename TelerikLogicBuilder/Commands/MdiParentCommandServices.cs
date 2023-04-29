using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Commands.Factories;
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
            => services
                .AddTransient<IApplicationCommandsFactory, ApplicationCommandsFactory>()
                .AddTransient<BuildActiveDocumentCommand>()
                .AddTransient<BuildSaveConsolidateSelectedDocumentsCommand>()
                .AddTransient<CloseProjectCommand>()
                .AddTransient<Func<string, DeleteSelectedFilesFromApiCommand>>
                (
                    provider =>
                    applicationName => new DeleteSelectedFilesFromApiCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ITryGetSelectedRulesResourcesPairs>(),
                        provider.GetRequiredService<IDeleteSelectedFilesFromApi>(),
                        provider.GetRequiredService<IMainWindow>(),
                        applicationName
                    )
                )
                .AddTransient<Func<string, DeleteSelectedFilesFromFileSystemCommand>>
                (
                    provider =>
                    applicationName => new DeleteSelectedFilesFromFileSystemCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ITryGetSelectedRulesResourcesPairs>(),
                        provider.GetRequiredService<IDeleteSelectedFilesFromFileSystem>(),
                        provider.GetRequiredService<IMainWindow>(),
                        applicationName
                    )
                )
                .AddTransient<Func<string, DeploySelectedFilesToApiCommand>>
                (
                    provider =>
                    applicationName => new DeploySelectedFilesToApiCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ITryGetSelectedRulesResourcesPairs>(),
                        provider.GetRequiredService<IDeploySelectedFilesToApi>(),
                        provider.GetRequiredService<IMainWindow>(),
                        applicationName
                    )
                )
                .AddTransient<Func<string, DeploySelectedFilesToFileSystemCommand>>
                (
                    provider =>
                    applicationName => new DeploySelectedFilesToFileSystemCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ITryGetSelectedRulesResourcesPairs>(),
                        provider.GetRequiredService<IDeploySelectedFilesToFileSystem>(),
                        provider.GetRequiredService<IMainWindow>(),
                        applicationName
                    )
                )
                .AddTransient<DisplayIndexInformationCommand>()
                .AddTransient<EditActiveDocumentCommand>()
                .AddTransient<EditConnectorObjectTypesCommand>()
                .AddTransient<EditConstructorsCommand>()
                .AddTransient<EditFragmentsCommand>()
                .AddTransient<EditFunctionsCommand>()
                .AddTransient<EditProjectPropertiesCommand>()
                .AddTransient<EditVariablesCommand>()
                .AddTransient<ExitCommand>()
                .AddTransient<FindConstructorCommand>()
                .AddTransient<FindConstructorInFilesCommand>()
                .AddTransient<FindCellCommand>()
                .AddTransient<FindFunctionCommand>()
                .AddTransient<FindFunctionInFilesCommand>()
                .AddTransient<FindReplaceConstructorCommand>()
                .AddTransient<FindReplaceFunctionCommand>()
                .AddTransient<FindReplaceTextCommand>()
                .AddTransient<FindReplaceVariableCommand>()
                .AddTransient<FindShapeCommand>()
                .AddTransient<FindTextCommand>()
                .AddTransient<FindTextInFilesCommand>()
                .AddTransient<FindVariableCommand>()
                .AddTransient<FindVariableInFilesCommand>()
                .AddTransient<NewProjectCommand>()
                .AddTransient<OpenProjectCommand>()
                .AddTransient<SaveActiveDocumentCommand>()
                .AddTransient<Func<RadMenuItem, RadMenuItem, string, SetColorThemeCommand>>
                (
                    provider =>
                    (colorThemeMenuItem, fontSizeMenuItem, colorTheme) => new SetColorThemeCommand
                    (
                        provider.GetRequiredService<IThemeManager>(),
                        colorThemeMenuItem,
                        fontSizeMenuItem,
                        colorTheme
                    )
                )
                .AddTransient<Func<RadMenuItem, RadMenuItem, int, SetFontSizeCommand>>
                (
                    provider =>
                    (colorThemeMenuItem, fontSizeMenuItem, fontSize) => new SetFontSizeCommand
                    (
                        provider.GetRequiredService<IThemeManager>(),
                        colorThemeMenuItem,
                        fontSizeMenuItem,
                        fontSize
                    )
                )
                .AddTransient<Func<RadMenuItem, string, SetSelectedApplicationCommand>>
                (
                    provider =>
                    (selectApplicationMenuItem, applicationName) => new SetSelectedApplicationCommand
                    (
                        provider.GetRequiredService<ICheckSelectedApplication>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        selectApplicationMenuItem,
                        applicationName
                    )
                )
                .AddTransient<ValidateSelectedDocumentsCommand>()
                .AddTransient<ValidateActiveDocumentCommand>()
                .AddTransient<Func<string, ValidateSelectedRulesCommand>>
                (
                    provider =>
                    applicationName => new ValidateSelectedRulesCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<ITryGetSelectedRules>(),
                        provider.GetRequiredService<IValidateSelectedRules>(),
                        provider.GetRequiredService<IMainWindow>(),
                        applicationName
                    )
                )
                .AddTransient<ViewApplicationsStencilCommand>()
                .AddTransient<ViewFlowDiagramStencilCommand>()
                .AddTransient<ViewMessagesCommand>()
                .AddTransient<ViewProjectExplorerCommand>();
    }
}

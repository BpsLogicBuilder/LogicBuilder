using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RulesExplorerHelpers.Forms;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RulesExplorerCommandServices
    {
        internal static IServiceCollection AddRulesExplorerCommands(this IServiceCollection services)
        {
            return services
                .AddTransient<RadRuleSetDialog>()
                .AddTransient<Func<IRulesExplorer, DeleteAllRulesCommand>>
                (
                    provider =>
                    rulesExplorer => ActivatorUtilities.CreateInstance<DeleteAllRulesCommand>
                    (
                        provider,
                        provider.GetRequiredService<IFileIOHelper>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        rulesExplorer
                    )
                )
                .AddTransient<Func<IRulesExplorer, DeleteRulesExplorerFileCommand>>
                (
                    provider =>
                    rulesExplorer => ActivatorUtilities.CreateInstance<DeleteRulesExplorerFileCommand>
                    (
                        provider,
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFileIOHelper>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        rulesExplorer
                    )
                )
                .AddTransient<Func<IRulesExplorer, RefreshRulesExplorerCommand>>
                (
                    provider =>
                    rulesExplorer => ActivatorUtilities.CreateInstance<RefreshRulesExplorerCommand>
                    (
                        provider,
                        rulesExplorer
                    )
                )
                .AddTransient<Func<IRulesExplorer, ValidateCommand>>
                (
                    provider =>
                    rulesExplorer => ActivatorUtilities.CreateInstance<ValidateCommand>
                    (
                        provider,
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IValidateSelectedRules>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        rulesExplorer
                    )
                )
                .AddTransient<Func<IRulesExplorer, ViewCommand>>
                (
                    provider =>
                    rulesExplorer => ActivatorUtilities.CreateInstance<ViewCommand>
                    (
                        provider,
                        provider.GetRequiredService<IApplicationTypeInfoManager>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFileIOHelper>(),
                        provider.GetRequiredService<ILongStringManager>(),
                        provider.GetRequiredService<IMainWindow>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IRuleSetLoader>(),
                        provider.GetRequiredService<IRulesValidator>(),
                        provider.GetRequiredService<UiNotificationService>(),
                        rulesExplorer
                    )
                );
        }
    }
}

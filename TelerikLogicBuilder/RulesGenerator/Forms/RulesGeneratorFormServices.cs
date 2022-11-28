using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RulesGeneratorFormServices
    {
        internal static IServiceCollection AddRulesGeneratorForms(this IServiceCollection services)
            => services
                .AddTransient<ICreateSelectRulesFormFactory, CreateSelectRulesFormFactory>()
                .AddTransient<ISelectRulesResourcesPairFormFactory, SelectRulesResourcesPairFormFactory>()
                .AddTransient<ISelectRulesFormFactory, SelectRulesFormFactory>()
                .AddTransient<ISelectDocumentsForm, SelectDocumentsForm>()
                .AddTransient<ITryGetSelectedDocuments, TryGetSelectedDocuments>()
                .AddTransient<ITryGetSelectedRules, TryGetSelectedRules>()
                .AddTransient<ITryGetSelectedRulesResourcesPairs, TryGetSelectedRulesResourcesPairs>()
                .AddTransient<Func<string, SelectRulesForm>>
                (
                    provider =>
                    applicationName => new SelectRulesForm
                    (
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IGetAllCheckedNodes>(),
                        provider.GetRequiredService<ISelectRulesTreeViewBuilder>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        applicationName
                    )
                )
                .AddTransient<Func<string, SelectRulesResourcesPairForm>>
                (
                    provider =>
                    applicationName => new SelectRulesResourcesPairForm
                    (
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IGetAllCheckedNodes>(),
                        provider.GetRequiredService<ISelectModulesForDeploymentTreeViewBuilder>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        applicationName
                    )
                 );
    }
}

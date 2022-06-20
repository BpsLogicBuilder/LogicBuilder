using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RulesGeneratorFormServices
    {
        internal static IServiceCollection AddRulesGeneratorForms(this IServiceCollection services)
            => services
                .AddTransient<ISelectRulesResourcesPairFormFactory, SelectRulesResourcesPairFormFactory>()
                .AddTransient<ISelectRulesFormFactory, SelectRulesFormFactory>()
                .AddTransient<SelectDocumentsForm>()
                .AddTransient<ITryGetSelectedDocuments, TryGetSelectedDocuments>()
                .AddTransient<ITryGetSelectedRules, TryGetSelectedRules>()
                .AddTransient<ITryGetSelectedRulesResourcesPairs, TryGetSelectedRulesResourcesPairs>()
                .AddTransient<Func<string, SelectRulesForm>>
                (
                    provider =>
                    applicationName => ActivatorUtilities.CreateInstance<SelectRulesForm>
                    (
                        provider,
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IGetAllCheckedNodes>(),
                        provider.GetRequiredService<ISelectRulesTreeViewBuilder>(),
                        applicationName
                    )
                )
                .AddTransient<Func<string, SelectRulesResourcesPairForm>>
                (
                    provider =>
                    applicationName => ActivatorUtilities.CreateInstance<SelectRulesResourcesPairForm>
                    (
                        provider,
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IGetAllCheckedNodes>(),
                        provider.GetRequiredService<ISelectModulesForDeploymentTreeViewBuilder>(),
                        applicationName
                    )
                 )
                .AddTransient<SelectRulesForm>()
                .AddTransient<SelectRulesResourcesPairForm>();
    }
}

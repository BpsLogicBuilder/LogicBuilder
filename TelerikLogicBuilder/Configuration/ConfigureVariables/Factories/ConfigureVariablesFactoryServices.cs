using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureVariablesFactoryServices
    {
        internal static IServiceCollection AddConfigureVariablesFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureVariablesForm, IConfigureVariablesDragDropHandler>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesDragDropHandler
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<IConfigureVariablesFactory, ConfigureVariablesFactory>()
                .AddTransient<Func<IConfigureVariablesForm, ConfigureVariablesTreeView>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesTreeView
                    (
                        provider.GetRequiredService<IConfigureVariablesFactory>(),
                        configureVariablesForm
                    )
                );
        }
    }
}

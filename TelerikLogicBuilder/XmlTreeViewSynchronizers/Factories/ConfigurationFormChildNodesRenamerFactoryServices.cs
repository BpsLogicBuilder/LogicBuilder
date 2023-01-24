using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationFormChildNodesRenamerFactoryServices
    {
        internal static IServiceCollection AddConfigurationFormChildNodesRenamerFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConstructorsForm, IConfigureConstructorsChildNodesRenamer>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsChildNodesRenamer
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureFragmentsForm, IConfigureFragmentsChildNodesRenamer>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFragmentsChildNodesRenamer
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, IConfigureFunctionsChildNodesRenamer>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsChildNodesRenamer
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, IConfigureVariablesChildNodesRenamer>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesChildNodesRenamer
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<IConfigurationFormChildNodesRenamerFactory, ConfigurationFormChildNodesRenamerFactory>();
        }
    }
}

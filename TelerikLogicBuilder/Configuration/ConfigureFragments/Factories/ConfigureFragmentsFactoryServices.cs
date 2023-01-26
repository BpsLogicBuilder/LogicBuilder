using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureFragmentsFactoryServices
    {
        internal static IServiceCollection AddConfigureFragmentsFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureFragmentsForm, IConfigureFragmentsDragDropHandler>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsDragDropHandler
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFragmentsForm
                    )
                )
                .AddTransient<IConfigureFragmentsFactory, ConfigureFragmentsFactory>()
                .AddTransient<Func<IConfigureFragmentsForm, ConfigureFragmentsTreeView>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsTreeView
                    (
                        provider.GetRequiredService<IConfigureFragmentsFactory>(),
                        configureFragmentsForm
                    )
                );
        }
    }
}

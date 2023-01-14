using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureConstructorsFactoryServices
    {
        internal static IServiceCollection AddConfigureConstructorsFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConstructorsForm, IConfigureConstructorsDragDropHandler>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsDragDropHandler
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, ConfigureConstructorsTreeView>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsTreeView
                    (
                        provider.GetRequiredService<IConfigureConstructorsFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<IConfigureConstructorsFactory, ConfigureConstructorsFactory>();
        }
    }
}

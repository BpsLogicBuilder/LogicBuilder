using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureFunctionsFactoryServices
    {
        internal static IServiceCollection AddConfigureFunctionsFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureFunctionsForm, IConfigureFunctionsDragDropHandler>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsDragDropHandler
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<IConfigureFunctionsFactory, ConfigureFunctionsFactory>()
                .AddTransient<Func<IConfigureFunctionsForm, ConfigureFunctionsTreeView>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsTreeView
                    (
                        provider.GetRequiredService<IConfigureFunctionsFactory>(),
                        configureFunctionsForm
                    )
                );
        }
    }
}

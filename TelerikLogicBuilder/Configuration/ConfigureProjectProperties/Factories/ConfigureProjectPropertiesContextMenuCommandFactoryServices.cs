using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal static class ConfigureProjectPropertiesContextMenuCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureProjectPropertiesContextMenuCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureProjectPropertiesForm, AddApplicationCommand>>
                (
                    provider =>
                    configureProjectProperties => new AddApplicationCommand
                    (
                        provider.GetRequiredService<ICreateDefaultApplication>(),
                        provider.GetRequiredService<IGetNextApplicationNumber>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureProjectProperties
                    )
                )
                .AddTransient<IConfigureProjectPropertiesContextMenuCommandFactory, ConfigureProjectPropertiesContextMenuCommandFactory>()
                .AddTransient<Func<IConfigureProjectPropertiesForm, DeleteApplicationCommand>>
                (
                    provider =>
                    configureProjectProperties => new DeleteApplicationCommand
                    (
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureProjectProperties
                    )
                );
        }
    }
}

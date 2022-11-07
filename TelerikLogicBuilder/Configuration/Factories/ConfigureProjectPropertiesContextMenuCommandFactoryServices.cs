using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureProjectPropertiesContextMenuCommandFactoryServices
    {
        internal static IServiceCollection AddConfigureProjectPropertiesContextMenuCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureProjectProperties, AddApplicationCommand>>
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
                .AddTransient<Func<IConfigureProjectProperties, DeleteApplicationCommand>>
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

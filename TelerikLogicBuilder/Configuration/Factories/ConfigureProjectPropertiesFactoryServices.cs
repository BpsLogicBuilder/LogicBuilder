using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureProjectPropertiesFactoryServices
    {
        internal static IServiceCollection AddConfigureProjectPropertiesFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<bool, ConfigureProjectProperties>>
                (
                    provider =>
                    openedAsReadOnly => new ConfigureProjectProperties
                    (
                        provider.GetRequiredService<IConfigurationControlFactory>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConfigureProjectPropertiesContextMenuCommandFactory>(),
                        provider.GetRequiredService<IConfigureProjectPropertiesTreeviewBuilder>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILoadProjectProperties>(),
                        provider.GetRequiredService<IProjectPropertiesXmlParser>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IUpdateProjectProperties>(),
                        provider.GetRequiredService<DialogFormMessageControl>(),
                        openedAsReadOnly
                    )
                )
                .AddTransient<IConfigurationFormFactory, ConfigurationFormFactory>();
        }
    }
}

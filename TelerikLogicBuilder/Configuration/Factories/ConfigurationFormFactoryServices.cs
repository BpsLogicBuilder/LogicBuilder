using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationFormFactoryServices
    {
        internal static IServiceCollection AddConfigurationFormFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigurationFormFactory, ConfigurationFormFactory>()
                .AddTransient<Func<IList<string>, ConfigureExcludedModules>>
                (
                    provider =>
                    excludedModules => new ConfigureExcludedModules
                    (
                        provider.GetRequiredService<IConfigureExcludedModulesCommandFactory>(),
                        provider.GetRequiredService<IExcludedModulesTreeViewBuilder>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IGetAllCheckedNodes>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        excludedModules
                    )
                )
                .AddTransient<Func<IList<string>, ConfigureLoadAssemblyPaths>>
                (
                    provider =>
                    existingPaths => new ConfigureLoadAssemblyPaths
                    (
                        provider.GetRequiredService<IConfigurationItemFactory>(),
                        provider.GetRequiredService<IConfigurationControlFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<DialogFormMessageControl>(),
                        existingPaths
                    )
                )
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
                .AddTransient<Func<WebApiDeployment, ConfigureWebApiDeployment>>
                (
                    provider =>
                    webApiDeployment => new ConfigureWebApiDeployment
                    (
                        provider.GetRequiredService<IConfigurationItemFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<DialogFormMessageControl>(),
                        webApiDeployment
                    )
                );
        }
    }
}

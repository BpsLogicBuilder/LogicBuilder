﻿using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationFormFactoryServices
    {
        internal static IServiceCollection AddConfigurationFormFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IConfigurationFormFactory, ConfigurationFormFactory>()
                .AddTransient<Func<bool, ConfigureConnectorObjectsForm>>
                (
                    provider =>
                    openedAsReadOnly => new ConfigureConnectorObjectsForm
                    (
                        provider.GetRequiredService<IConfigurationControlFactory>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<ILoadProjectProperties>(),
                        provider.GetRequiredService<IProjectPropertiesXmlParser>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IUpdateProjectProperties>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        openedAsReadOnly
                    )
                )
                .AddTransient<Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureConstructorGenericArgumentsForm>>
                (
                    provider =>
                    (xmlDocument, configuredGenericArgumentNames, memberParameters, genericTypeDefinition) => new ConfigureConstructorGenericArgumentsForm
                    (
                        provider.GetRequiredService<IConfigureConstructorGenericArgumentsTreeViewBuilder>(),
                        provider.GetRequiredService<IConfigureGenericArgumentsCommandFactory>(),
                        provider.GetRequiredService<IConfigureGenericArgumentsControlFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IGenericConfigXmlParser>(),
                        provider.GetRequiredService<IGenericsConfigrationValidator>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        xmlDocument,
                        configuredGenericArgumentNames,
                        memberParameters,
                        genericTypeDefinition
                    )
                )
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
                .AddTransient<Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureFunctionGenericArgumentsForm>>
                (
                    provider =>
                    (xmlDocument, configuredGenericArgumentNames, memberParameters, genericTypeDefinition) => new ConfigureFunctionGenericArgumentsForm
                    (
                        xmlDocument,
                        configuredGenericArgumentNames,
                        memberParameters,
                        genericTypeDefinition
                    )
                )
                .AddTransient<Func<IList<string>, ConfigureLoadAssemblyPaths>>
                (
                    provider =>
                    existingPaths => new ConfigureLoadAssemblyPaths
                    (
                        provider.GetRequiredService<IConfigurationControlFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
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
                        provider.GetRequiredService<IDialogFormMessageControl>(),
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
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        webApiDeployment
                    )
                );
        }
    }
}

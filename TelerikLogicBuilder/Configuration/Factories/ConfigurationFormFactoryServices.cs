using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.TreeViewBuiilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
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
                .AddTransient<Func<bool, IConfigureConnectorObjectsForm>>
                (
                    provider =>
                    openedAsReadOnly => new ConfigureConnectorObjectsForm
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConfigureConnectorObjectsControlFactory>(),
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
                .AddTransient<Func<bool, IConfigureConstructorsForm>>
                (
                    provider =>
                    openedAsReadOnly => new ConfigureConstructorsForm
                    (
                        provider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>(),
                        provider.GetRequiredService<IConfigureConstructorsCommandFactory>(),
                        provider.GetRequiredService<IConfigureConstructorsControlFactory>(),
                        provider.GetRequiredService<IConfigureConstructorsFactory>(),
                        provider.GetRequiredService<IConstructorXmlParser>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IHelperStatusBuilderFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILoadConstructors>(),
                        provider.GetRequiredService<IParametersControlFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewBuilderFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IUpdateConstructors>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        openedAsReadOnly
                    )
                )
                .AddTransient<Func<IList<string>, IConfigureExcludedModulesForm>>
                (
                    provider =>
                    excludedModules => new ConfigureExcludedModulesForm
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
                .AddTransient<Func<bool, IConfigureFragmentsForm>>
                (
                    provider =>
                    openedAsReadOnly => new ConfigureFragmentsForm
                    (
                        provider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>(),
                        provider.GetRequiredService<IConfigureFragmentsCommandFactory>(),
                        provider.GetRequiredService<IConfigureFragmentsControlFactory>(),
                        provider.GetRequiredService<IConfigureFragmentsFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILoadFragments>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewBuilderFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IUpdateFragments>(),
                        openedAsReadOnly
                    )
                )
                .AddTransient<Func<bool, IConfigureFunctionsForm>>
                (
                    provider =>
                    openedAsReadOnly => new ConfigureFunctionsForm
                    (
                        provider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>(),
                        provider.GetRequiredService<IConfigureFunctionsCommandFactory>(),
                        provider.GetRequiredService<IConfigureFunctionsControlFactory>(),
                        provider.GetRequiredService<IConfigureFunctionsFactory>(),
                        provider.GetRequiredService<IConstructorXmlParser>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IHelperStatusBuilderFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILoadConstructors>(),
                        provider.GetRequiredService<ILoadFunctions>(),
                        provider.GetRequiredService<ILoadVariables>(),
                        provider.GetRequiredService<IParametersControlFactory>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewBuilderFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IUpdateConstructors>(),
                        provider.GetRequiredService<IUpdateFunctions>(),
                        provider.GetRequiredService<IVariablesXmlParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        openedAsReadOnly
                    )
                )
                .AddTransient<Func<IList<string>, Type, IConfigureLiteralDomainForm>>
                (
                    provider =>
                    (existingDomainItems, type) => new ConfigureLiteralDomainForm
                    (
                        provider.GetRequiredService<IConfigureLiteralDomainControlFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        existingDomainItems, 
                        type
                    )
                )
                .AddTransient<Func<IList<string>, Type, IConfigureLiteralListDefaultValueForm>>
                (
                    provider =>
                    (existingDefaultValueItems, type) => new ConfigureLiteralListDefaultValueForm
                    (
                        provider.GetRequiredService<IConfigureLiteralListDefaultValueControlFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        existingDefaultValueItems,
                        type
                    )
                )
                .AddTransient<Func<IList<string>, IConfigureLoadAssemblyPathsForm>>
                (
                    provider =>
                    existingPaths => new ConfigureLoadAssemblyPathsForm
                    (
                        provider.GetRequiredService<IConfigureLoadAssemblyPathsControlFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        existingPaths
                    )
                )
                .AddTransient<Func<bool, IConfigureProjectPropertiesForm>>
                (
                    provider =>
                    openedAsReadOnly => new ConfigureProjectPropertiesForm
                    (
                        provider.GetRequiredService<IConfigureProjectPropertiesControlFactory>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConfigureProjectPropertiesContextMenuCommandFactory>(),
                        provider.GetRequiredService<IConfigureProjectPropertiesTreeViewBuilder>(),
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
                .AddTransient<Func<IList<string>, ReturnTypeBase, IConfigureReturnTypeForm>>
                (
                    provider =>
                    (genericArguments, returnType) => new ConfigureReturnTypeForm
                    (
                        provider.GetRequiredService<IConfigureReturnTypeControlFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRadDropDownListHelper>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        genericArguments, 
                        returnType
                    )
                )
                .AddTransient<Func<bool, IConfigureVariablesForm>>
                (
                    provider =>
                    openedAsReadOnly => new ConfigureVariablesForm
                    (
                        provider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>(),
                        provider.GetRequiredService<IConfigureVariablesCommandFactory>(),
                        provider.GetRequiredService<IConfigureVariablesControlFactory>(),
                        provider.GetRequiredService<IConfigureVariablesFactory>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IHelperStatusBuilderFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILoadVariables>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<ITreeViewBuilderFactory>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IUpdateVariables>(),
                        provider.GetRequiredService<IVariablesXmlParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        openedAsReadOnly
                    )
                )
                .AddTransient<Func<WebApiDeployment, IConfigureWebApiDeploymentForm>>
                (
                    provider =>
                    webApiDeployment => new ConfigureWebApiDeploymentForm
                    (
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IWebApiDeploymentItemFactory>(),
                        webApiDeployment
                    )
                )
                .AddTransient<Func<IList<string>, IEditGenericArgumentsForm>>
                (
                    provider =>
                    existingArguments => new EditGenericArgumentsForm
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditGenericArgumentsControlFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        existingArguments
                    )
                );
        }
    }
}

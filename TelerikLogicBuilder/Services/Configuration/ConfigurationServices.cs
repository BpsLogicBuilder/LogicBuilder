﻿using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationServices
    {
        internal static IServiceCollection AddConfiguration(this IServiceCollection services)
            => services
                .AddSingleton<IApplicationXmlParser, ApplicationXmlParser>()
                .AddSingleton<IBuiltInFunctionsLoader, BuiltInFunctionsLoader>()
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<IConfigureConstructorsCutImageHelper, ConfigureConstructorsCutImageHelper>()
                .AddSingleton<IConfigureFragmentsCutImageHelper, ConfigureFragmentsCutImageHelper>()
                .AddSingleton<IConfigureFunctionsCutImageHelper, ConfigureFunctionsCutImageHelper>()
                .AddSingleton<IConfigureVariablesCutImageHelper, ConfigureVariablesCutImageHelper>()
                .AddSingleton<ICreateConstructors, CreateConstructors>()
                .AddSingleton<ICreateDefaultApplication, CreateDefaultApplication>()
                .AddSingleton<ICreateFragments, CreateFragments>()
                .AddSingleton<ICreateFunctions, CreateFunctions>()
                .AddSingleton<ICreateProjectProperties, CreateProjectProperties>()
                .AddSingleton<ICreateVariables, CreateVariables>()
                .AddTransient<IEditConnectorObjectTypes, EditConnectorObjectTypes>()
                .AddSingleton<IEditConstructors, EditConstructors>()
                .AddSingleton<IEditFragments, EditFragments>()
                .AddSingleton<IEditFunctions, EditFunctions>()
                .AddTransient<IEditProjectProperties, EditProjectProperties>()
                .AddSingleton<IEditVariables, EditVariables>()
                .AddSingleton<IFragmentXmlParser, FragmentXmlParser>()
                .AddSingleton<ILoadConstructors, LoadConstructors>()
                .AddSingleton<ILoadFragments, LoadFragments>()
                .AddSingleton<ILoadFunctions, LoadFunctions>()
                .AddSingleton<ILoadProjectProperties, LoadProjectProperties>()
                .AddSingleton<ILoadVariables, LoadVariables>()
                .AddSingleton<IProjectPropertiesXmlParser, ProjectPropertiesXmlParser>()
                .AddSingleton<IUpdateConstructors, UpdateConstructors>()
                .AddSingleton<IUpdateFragments, UpdateFragments>()
                .AddSingleton<IUpdateFunctions, UpdateFunctions>()
                .AddSingleton<IUpdateProjectProperties, UpdateProjectProperties>()
                .AddSingleton<IUpdateVariables, UpdateVariables>()
                .AddSingleton<IWebApiDeploymentXmlParser, WebApiDeploymentXmlParser>()
                .AddApplicationControlCommandFactories()
                .AddConfigurationFormFactories()
                .AddConfigurationInitialization()
                .AddConfigureConnectorObjectsCommandFactories()
                .AddConfigureConnectorObjectsControlFactories()
                .AddConfigureConstructorControlCommandFactories()
                .AddConfigureConstructorsCommandFactories()
                .AddConfigureConstructorsControlFactories()
                .AddConfigureConstructorsFactories()
                .AddConfigureExcludedModulesCommandFactories()
                .AddConfigureFragmentsCommandFactories()
                .AddConfigureFragmentsControlFactories()
                .AddConfigureFragmentsFactories()
                .AddConfigureFunctionControlCommandFactories()
                .AddConfigureFunctionsCommandFactories()
                .AddConfigureFunctionsControlFactories()
                .AddConfigureFunctionsFactories()
                .AddConfigureGenericArgumentsCommandFactories()
                .AddConfigureGenericArgumentsControlFactories()
                .AddConfigureGenericLiteralArgumentCommandFactories()
                .AddConfigureLiteralDomainCommandFactories()
                .AddConfigureLiteralDomainControlFactories()
                .AddConfigureLiteralListParameterCommandFactories()
                .AddConfigureLiteralListVariableCommandFactories()
                .AddConfigureLiteralParameterCommandFactories()
                .AddConfigureLiteralVariableCommandFactories()
                .AddConfigureGenericLiteralListArgumentCommandFactories()
                .AddConfigureLiteralListDefaultValueCommandFactories()
                .AddConfigureLiteralListDefaultValueControlFactories()
                .AddConfigureLoadAssemblyPathsCommandFactories()
                .AddConfigureLoadAssemblyPathsControlFactories()
                .AddConfigureProjectPropertiesControlFactories()
                .AddConfigureProjectPropertiesHelpers()
                .AddConfigureProjectPropertiesContextMenuCommandFactories()
                .AddConfigureReturnTypeControlFactories()
                .AddConfigureVariablesCommandFactories()
                .AddConfigureVariablesControlFactories()
                .AddConfigureVariablesFactories()
                .AddConnectorObjectsItemFactories()
                .AddConstructorControlValidatorFactories()
                .AddEditGenericArgumentsCommandFactories()
                .AddEditGenericArgumentsControlFactories()
                .AddFragmentItemFactories()
                .AddFunctionControlValidatorFactories()
                .AddLiteralDomainItemFactories()
                .AddLiteralListDefaultValueItemFactories()
                .AddLoadAssemblyPathsItemFactories()
                .AddParameterControlValidatorFactories()
                .AddParametersControlFactories()
                .AddProjectPropertiesItemFactories()
                .AddVariableControlValidatorFactories()
                .AddWebApiDeploymentItemFactories();
    }
}

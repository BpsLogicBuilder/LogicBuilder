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
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class ConfigurationFormFactory : IConfigurationFormFactory
    {
        public IConfigureConnectorObjectsForm GetConfigureConnectorObjectsForm(bool openedAsReadOnly)
        {
            return new ConfigureConnectorObjectsForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IConfigureConnectorObjectsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<ILoadProjectProperties>(),
                Program.ServiceProvider.GetRequiredService<IProjectPropertiesXmlParser>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<IUpdateProjectProperties>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                openedAsReadOnly
            );
        }

        public IConfigureGenericArgumentsForm GetConfigureConstructorGenericArgumentsForm(
            XmlDocument xmlDocument,
            IList<string> configuredGenericArgumentNames,
            IList<ParameterBase> memberParameters,
            Type genericTypeDefinition)
        {
            return new ConfigureConstructorGenericArgumentsForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigureConstructorGenericArgumentsTreeViewBuilder>(),
                Program.ServiceProvider.GetRequiredService<IConfigureGenericArgumentsCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureGenericArgumentsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IGenericConfigXmlParser>(),
                Program.ServiceProvider.GetRequiredService<IGenericsConfigrationValidator>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                xmlDocument,
                configuredGenericArgumentNames,
                memberParameters,
                genericTypeDefinition
            );
        }

        public IConfigureConstructorsForm GetConfigureConstructorsForm(bool openedAsReadOnly)
        {
            return new ConfigureConstructorsForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureConstructorsCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureConstructorsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureConstructorsFactory>(),
                Program.ServiceProvider.GetRequiredService<IConstructorXmlParser>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IHelperStatusBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILoadConstructors>(),
                Program.ServiceProvider.GetRequiredService<IParametersControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IUpdateConstructors>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                openedAsReadOnly
            );
        }

        public IConfigureExcludedModulesForm GetConfigureExcludedModules(IList<string> excludedModules)
        {
            return new ConfigureExcludedModulesForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigureExcludedModulesCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IExcludedModulesTreeViewBuilder>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IGetAllCheckedNodes>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                excludedModules
            );
        }

        public IConfigureGenericArgumentsForm GetConfigureFunctionGenericArgumentsForm(XmlDocument xmlDocument, IList<string> configuredGenericArgumentNames, IList<ParameterBase> memberParameters, Type genericTypeDefinition)
        {
            return new ConfigureFunctionGenericArgumentsForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigureFunctionGenericArgumentsTreeViewBuilder>(),
                Program.ServiceProvider.GetRequiredService<IConfigureGenericArgumentsCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureGenericArgumentsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IGenericConfigXmlParser>(),
                Program.ServiceProvider.GetRequiredService<IGenericsConfigrationValidator>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                xmlDocument,
                configuredGenericArgumentNames,
                memberParameters,
                genericTypeDefinition
            );
        }

        public IConfigureFragmentsForm GetConfigureFragmentsForm(bool openedAsReadOnly)
        {
            return new ConfigureFragmentsForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureFragmentsCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureFragmentsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureFragmentsFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILoadFragments>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IUpdateFragments>(),
                openedAsReadOnly
            );
        }

        public IConfigureFunctionsForm GetConfigureFunctionsForm(bool openedAsReadOnly)
        {
            return new ConfigureFunctionsForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureFunctionsCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureFunctionsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureFunctionsFactory>(),
                Program.ServiceProvider.GetRequiredService<IConstructorXmlParser>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IHelperStatusBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILoadConstructors>(),
                Program.ServiceProvider.GetRequiredService<ILoadFunctions>(),
                Program.ServiceProvider.GetRequiredService<ILoadVariables>(),
                Program.ServiceProvider.GetRequiredService<IParametersControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IUpdateConstructors>(),
                Program.ServiceProvider.GetRequiredService<IUpdateFunctions>(),
                Program.ServiceProvider.GetRequiredService<IVariablesXmlParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                openedAsReadOnly
            );
        }

        public IConfigureLiteralDomainForm GetConfigureLiteralDomainForm(IList<string> existingDomainItems, Type type)
        {
            return new ConfigureLiteralDomainForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigureLiteralDomainControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                existingDomainItems,
                type
            );
        }

        public IConfigureLiteralListDefaultValueForm GetConfigureLiteralListDefaultValueForm(IList<string> existingDefaultValueItems, Type type)
        {
            return new ConfigureLiteralListDefaultValueForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigureLiteralListDefaultValueControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                existingDefaultValueItems,
                type
            );
        }

        public IConfigureLoadAssemblyPathsForm GetConfigureLoadAssemblyPaths(IList<string> existingPaths)
        {
            return new ConfigureLoadAssemblyPathsForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigureLoadAssemblyPathsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                existingPaths
            );
        }

        public IConfigureProjectPropertiesForm GetConfigureProjectProperties(bool openedAsReadOnly)
        {
            return new ConfigureProjectPropertiesForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigureProjectPropertiesControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IConfigureProjectPropertiesContextMenuCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureProjectPropertiesTreeViewBuilder>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILoadProjectProperties>(),
                Program.ServiceProvider.GetRequiredService<IProjectPropertiesXmlParser>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IUpdateProjectProperties>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                openedAsReadOnly
            );
        }

        public IConfigureReturnTypeForm GetConfigureReturnTypeForm(IList<string> genericArguments, ReturnTypeBase returnType)
        {
            return new ConfigureReturnTypeForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigureReturnTypeControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IRadDropDownListHelper>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                genericArguments,
                returnType
            );
        }

        public IConfigureVariablesForm GetConfigureVariablesForm(bool openedAsReadOnly)
        {
            return new ConfigureVariablesForm
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationFormChildNodesRenamerFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureVariablesCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureVariablesControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IConfigureVariablesFactory>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IHelperStatusBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ILoadVariables>(),
                Program.ServiceProvider.GetRequiredService<IServiceFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewBuilderFactory>(),
                Program.ServiceProvider.GetRequiredService<ITreeViewService>(),
                Program.ServiceProvider.GetRequiredService<IUpdateVariables>(),
                Program.ServiceProvider.GetRequiredService<IVariablesXmlParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                openedAsReadOnly
            );
        }

        public IConfigureWebApiDeploymentForm GetConfigureWebApiDeploymentForm(WebApiDeployment webApiDeployment)
        {
            return new ConfigureWebApiDeploymentForm
            (
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IWebApiDeploymentItemFactory>(),
                webApiDeployment
            );
        }

        public IEditGenericArgumentsForm GetEditGenericArgumentsForm(IList<string> existingArguments)
        {
            return new EditGenericArgumentsForm
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IEditGenericArgumentsControlFactory>(),
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                existingArguments
            );
        }
    }
}

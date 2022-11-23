using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class ConfigurationFormFactory : IConfigurationFormFactory
    {
        private Form? _scopedService;
        private readonly Func<bool, ConfigureConnectorObjectsForm> _getConfigureConnectorObjectsForm;
        private readonly Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureConstructorGenericArgumentsForm> _getConfigureConstructorGenericArgumentsForm;
        private readonly Func<IList<string>, ConfigureExcludedModulesForm> _getConfigureExcludedModules;
        private readonly Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureFunctionGenericArgumentsForm> _getConfigureFunctionGenericArgumentsForm;
        private readonly Func<IList<string>, ConfigureLoadAssemblyPathsForm> _getConfigureLoadAssemblyPaths;
        private readonly Func<bool, ConfigureProjectProperties> _getConfigureProjectProperties;
        private readonly Func<WebApiDeployment, ConfigureWebApiDeploymentForm> _getConfigureWebApiDeployment;
        private readonly IServiceScope _scope;

        public ConfigurationFormFactory(
            IServiceScopeFactory serviceScopeFactory,
            Func<bool, ConfigureConnectorObjectsForm> getConfigureConnectorObjectsForm,
            Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureConstructorGenericArgumentsForm> getConfigureConstructorGenericArgumentsForm,
            Func<IList<string>, ConfigureExcludedModulesForm> getConfigureExcludedModules,
            Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureFunctionGenericArgumentsForm> getConfigureFunctionGenericArgumentsForm,
            Func<IList<string>, ConfigureLoadAssemblyPathsForm> getConfigureLoadAssemblyPaths,
            Func<bool, ConfigureProjectProperties> getConfigureProjectProperties,
            Func<WebApiDeployment, ConfigureWebApiDeploymentForm> getConfigureWebApiDeployment)
        {
            _scope = serviceScopeFactory.CreateScope();
            _getConfigureConnectorObjectsForm = getConfigureConnectorObjectsForm;
            _getConfigureConstructorGenericArgumentsForm = getConfigureConstructorGenericArgumentsForm;
            _getConfigureExcludedModules = getConfigureExcludedModules;
            _getConfigureFunctionGenericArgumentsForm= getConfigureFunctionGenericArgumentsForm;
            _getConfigureLoadAssemblyPaths = getConfigureLoadAssemblyPaths;
            _getConfigureProjectProperties = getConfigureProjectProperties;
            _getConfigureWebApiDeployment = getConfigureWebApiDeployment;
        }

        public ConfigureConnectorObjectsForm GetConfigureConnectorObjectsForm(bool openedAsReadOnly)
        {
            _scopedService = _getConfigureConnectorObjectsForm(openedAsReadOnly);
            return (ConfigureConnectorObjectsForm)_scopedService;
        }

        public ConfigureConstructorGenericArgumentsForm GetConfigureConstructorGenericArgumentsForm(
            XmlDocument xmlDocument,
            IList<string> configuredGenericArgumentNames,
            IList<ParameterBase> memberParameters,
            Type genericTypeDefinition)
        {
            _scopedService = _getConfigureConstructorGenericArgumentsForm(xmlDocument, configuredGenericArgumentNames, memberParameters, genericTypeDefinition);
            return (ConfigureConstructorGenericArgumentsForm)_scopedService;
        }

        public ConfigureExcludedModulesForm GetConfigureExcludedModules(IList<string> excludedModules)
        {
            _scopedService = _getConfigureExcludedModules(excludedModules);
            return (ConfigureExcludedModulesForm)_scopedService;
        }

        public ConfigureFunctionGenericArgumentsForm GetConfigureFunctionGenericArgumentsForm(XmlDocument xmlDocument, IList<string> configuredGenericArgumentNames, IList<ParameterBase> memberParameters, Type genericTypeDefinition)
        {
            _scopedService = _getConfigureFunctionGenericArgumentsForm(xmlDocument, configuredGenericArgumentNames, memberParameters, genericTypeDefinition);
            return (ConfigureFunctionGenericArgumentsForm)_scopedService;
        }

        public ConfigureLoadAssemblyPathsForm GetConfigureLoadAssemblyPaths(IList<string> existingPaths)
        {
            _scopedService = _getConfigureLoadAssemblyPaths(existingPaths);
            return (ConfigureLoadAssemblyPathsForm)_scopedService;
        }

        public ConfigureProjectProperties GetConfigureProjectProperties(bool openedAsReadOnly)
        {
            _scopedService = _getConfigureProjectProperties(openedAsReadOnly);
            return (ConfigureProjectProperties)_scopedService;
        }

        public ConfigureWebApiDeploymentForm GetConfigureWebApiDeploymentForm(WebApiDeployment webApiDeployment)
        {
            _scopedService = _getConfigureWebApiDeployment(webApiDeployment);
            return (ConfigureWebApiDeploymentForm)_scopedService;
        }

        public void Dispose()
        {
            //The factory methods uses new() (outside the container) because of the parameter
            //so we have to dispose of the service manually (_scope.Dispose() will not dispose _scopedService).
            _scopedService?.Dispose();
            _scope.Dispose();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class ConfigurationFormFactory : IConfigurationFormFactory
    {
        private IDisposable? _scopedService;
        private readonly Func<bool, IConfigureConnectorObjectsForm> _getConfigureConnectorObjectsForm;
        private readonly Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureConstructorGenericArgumentsForm> _getConfigureConstructorGenericArgumentsForm;
        //using the concrete type ConfigureConstructorGenericArgumentsForm here to be distinct from ConfigureFunctionGenericArgumentsForm
        private readonly Func<IList<string>, IConfigureExcludedModulesForm> _getConfigureExcludedModules;
        private readonly Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureFunctionGenericArgumentsForm> _getConfigureFunctionGenericArgumentsForm;
        //using the concrete type ConfigureFunctionGenericArgumentsForm here to be distinct from ConfigureConstructorGenericArgumentsForm
        private readonly Func<IList<string>, ConfigureLoadAssemblyPathsForm> _getConfigureLoadAssemblyPaths;
        private readonly Func<bool, ConfigureProjectPropertiesForm> _getConfigureProjectProperties;
        private readonly Func<WebApiDeployment, IConfigureWebApiDeploymentForm> _getConfigureWebApiDeployment;
        private readonly IServiceScope _scope;

        public ConfigurationFormFactory(
            IServiceScopeFactory serviceScopeFactory,
            Func<bool, IConfigureConnectorObjectsForm> getConfigureConnectorObjectsForm,
            Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureConstructorGenericArgumentsForm> getConfigureConstructorGenericArgumentsForm,
            //using the concrete type ConfigureConstructorGenericArgumentsForm here to be distinct from ConfigureFunctionGenericArgumentsForm
            Func<IList<string>, IConfigureExcludedModulesForm> getConfigureExcludedModules,
            Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureFunctionGenericArgumentsForm> getConfigureFunctionGenericArgumentsForm,
            //using the concrete type ConfigureFunctionGenericArgumentsForm here to be distinct from ConfigureConstructorGenericArgumentsForm
            Func<IList<string>, ConfigureLoadAssemblyPathsForm> getConfigureLoadAssemblyPaths,
            Func<bool, ConfigureProjectPropertiesForm> getConfigureProjectProperties,
            Func<WebApiDeployment, IConfigureWebApiDeploymentForm> getConfigureWebApiDeployment)
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

        public IConfigureConnectorObjectsForm GetConfigureConnectorObjectsForm(bool openedAsReadOnly)
        {
            _scopedService = _getConfigureConnectorObjectsForm(openedAsReadOnly);
            return (IConfigureConnectorObjectsForm)_scopedService;
        }

        public IConfigureGenericArgumentsForm GetConfigureConstructorGenericArgumentsForm(
            XmlDocument xmlDocument,
            IList<string> configuredGenericArgumentNames,
            IList<ParameterBase> memberParameters,
            Type genericTypeDefinition)
        {
            _scopedService = _getConfigureConstructorGenericArgumentsForm(xmlDocument, configuredGenericArgumentNames, memberParameters, genericTypeDefinition);
            return (IConfigureGenericArgumentsForm)_scopedService;
        }

        public IConfigureExcludedModulesForm GetConfigureExcludedModules(IList<string> excludedModules)
        {
            _scopedService = _getConfigureExcludedModules(excludedModules);
            return (IConfigureExcludedModulesForm)_scopedService;
        }

        public IConfigureGenericArgumentsForm GetConfigureFunctionGenericArgumentsForm(XmlDocument xmlDocument, IList<string> configuredGenericArgumentNames, IList<ParameterBase> memberParameters, Type genericTypeDefinition)
        {
            _scopedService = _getConfigureFunctionGenericArgumentsForm(xmlDocument, configuredGenericArgumentNames, memberParameters, genericTypeDefinition);
            return (IConfigureGenericArgumentsForm)_scopedService;
        }

        public ConfigureLoadAssemblyPathsForm GetConfigureLoadAssemblyPaths(IList<string> existingPaths)
        {
            _scopedService = _getConfigureLoadAssemblyPaths(existingPaths);
            return (ConfigureLoadAssemblyPathsForm)_scopedService;
        }

        public ConfigureProjectPropertiesForm GetConfigureProjectProperties(bool openedAsReadOnly)
        {
            _scopedService = _getConfigureProjectProperties(openedAsReadOnly);
            return (ConfigureProjectPropertiesForm)_scopedService;
        }

        public IConfigureWebApiDeploymentForm GetConfigureWebApiDeploymentForm(WebApiDeployment webApiDeployment)
        {
            _scopedService = _getConfigureWebApiDeployment(webApiDeployment);
            return (IConfigureWebApiDeploymentForm)_scopedService;
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

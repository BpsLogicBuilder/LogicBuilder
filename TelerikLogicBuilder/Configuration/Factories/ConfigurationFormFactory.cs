using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
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
        private readonly Func<bool, IConfigureConstructorsForm> _getConfigureConstructorsForm;
        private readonly Func<IList<string>, IConfigureExcludedModulesForm> _getConfigureExcludedModules;
        private readonly Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureFunctionGenericArgumentsForm> _getConfigureFunctionGenericArgumentsForm;
        //using the concrete type ConfigureFunctionGenericArgumentsForm here to be distinct from ConfigureConstructorGenericArgumentsForm
        private readonly Func<bool, IConfigureFunctionsForm> _getConfigureFunctionsForm;
        private readonly Func<IList<string>, Type, IConfigureLiteralDomainForm> _getConfigureLiteralDomainForm;
        private readonly Func<IList<string>, Type, IConfigureLiteralListDefaultValueForm> _getConfigureLiteralListDefaultValueForm;
        private readonly Func<IList<string>, IConfigureLoadAssemblyPathsForm> _getConfigureLoadAssemblyPaths;
        private readonly Func<bool, IConfigureProjectPropertiesForm> _getConfigureProjectProperties;
        private readonly Func<bool, IConfigureVariablesForm> _getConfigureVariablesForm;
        private readonly Func<WebApiDeployment, IConfigureWebApiDeploymentForm> _getConfigureWebApiDeployment;
        private readonly Func<IList<string>, IEditGenericArgumentsForm> _getEditGenericArgumentsForm;

        public ConfigurationFormFactory(
            Func<bool, IConfigureConnectorObjectsForm> getConfigureConnectorObjectsForm,
            Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureConstructorGenericArgumentsForm> getConfigureConstructorGenericArgumentsForm,
            //using the concrete type ConfigureConstructorGenericArgumentsForm here to be distinct from ConfigureFunctionGenericArgumentsForm

            Func<bool, IConfigureConstructorsForm> getConfigureConstructorsForm,
            Func<IList<string>, IConfigureExcludedModulesForm> getConfigureExcludedModules,
            Func<XmlDocument, IList<string>, IList<ParameterBase>, Type, ConfigureFunctionGenericArgumentsForm> getConfigureFunctionGenericArgumentsForm,
            //using the concrete type ConfigureFunctionGenericArgumentsForm here to be distinct from ConfigureConstructorGenericArgumentsForm

            Func<bool, IConfigureFunctionsForm> getConfigureFunctionsForm,
            Func<IList<string>, Type, IConfigureLiteralDomainForm> getConfigureLiteralDomainForm,
            Func<IList<string>, Type, IConfigureLiteralListDefaultValueForm> getConfigureLiteralListDefaultValueForm,
            Func<IList<string>, IConfigureLoadAssemblyPathsForm> getConfigureLoadAssemblyPaths,
            Func<bool, IConfigureProjectPropertiesForm> getConfigureProjectProperties,
            Func<bool, IConfigureVariablesForm> getConfigureVariablesForm,
            Func<WebApiDeployment, IConfigureWebApiDeploymentForm> getConfigureWebApiDeployment,
            Func<IList<string>, IEditGenericArgumentsForm> getEditGenericArgumentsForm)
        {
            _getConfigureConnectorObjectsForm = getConfigureConnectorObjectsForm;
            _getConfigureConstructorGenericArgumentsForm = getConfigureConstructorGenericArgumentsForm;
            _getConfigureConstructorsForm = getConfigureConstructorsForm;
            _getConfigureExcludedModules = getConfigureExcludedModules;
            _getConfigureFunctionGenericArgumentsForm = getConfigureFunctionGenericArgumentsForm;
            _getConfigureFunctionsForm = getConfigureFunctionsForm;
            _getConfigureLiteralDomainForm = getConfigureLiteralDomainForm;
            _getConfigureLiteralListDefaultValueForm = getConfigureLiteralListDefaultValueForm;
            _getConfigureLoadAssemblyPaths = getConfigureLoadAssemblyPaths;
            _getConfigureProjectProperties = getConfigureProjectProperties;
            _getConfigureVariablesForm = getConfigureVariablesForm;
            _getConfigureWebApiDeployment = getConfigureWebApiDeployment;
            _getEditGenericArgumentsForm = getEditGenericArgumentsForm;
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

        public IConfigureConstructorsForm GetConfigureConstructorsForm(bool openedAsReadOnly)
        {
            _scopedService = _getConfigureConstructorsForm(openedAsReadOnly);
            return (IConfigureConstructorsForm)_scopedService;
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

        public IConfigureFunctionsForm GetConfigureFunctionsForm(bool openedAsReadOnly)
        {
            _scopedService = _getConfigureFunctionsForm(openedAsReadOnly);
            return (IConfigureFunctionsForm)_scopedService;
        }

        public IConfigureLiteralListDefaultValueForm GetConfigureLiteralListDefaultValueForm(IList<string> existingDefaultValueItems, Type type)
        {
            _scopedService = _getConfigureLiteralListDefaultValueForm(existingDefaultValueItems, type);
            return (IConfigureLiteralListDefaultValueForm)_scopedService;
        }

        public IConfigureLiteralDomainForm GetConfigureLiteralDomainForm(IList<string> existingDomainItems, Type type)
        {
            _scopedService = _getConfigureLiteralDomainForm(existingDomainItems, type);
            return (IConfigureLiteralDomainForm)_scopedService;
        }

        public IConfigureLoadAssemblyPathsForm GetConfigureLoadAssemblyPaths(IList<string> existingPaths)
        {
            _scopedService = _getConfigureLoadAssemblyPaths(existingPaths);
            return (IConfigureLoadAssemblyPathsForm)_scopedService;
        }

        public IConfigureProjectPropertiesForm GetConfigureProjectProperties(bool openedAsReadOnly)
        {
            _scopedService = _getConfigureProjectProperties(openedAsReadOnly);
            return (IConfigureProjectPropertiesForm)_scopedService;
        }

        public IConfigureVariablesForm GetConfigureVariablesForm(bool openedAsReadOnly)
        {
            _scopedService = _getConfigureVariablesForm(openedAsReadOnly);
            return (IConfigureVariablesForm)_scopedService;
        }

        public IConfigureWebApiDeploymentForm GetConfigureWebApiDeploymentForm(WebApiDeployment webApiDeployment)
        {
            _scopedService = _getConfigureWebApiDeployment(webApiDeployment);
            return (IConfigureWebApiDeploymentForm)_scopedService;
        }

        public IEditGenericArgumentsForm GetEditGenericArgumentsForm(IList<string> existingArguments)
        {
            _scopedService = _getEditGenericArgumentsForm(existingArguments);
            return (IEditGenericArgumentsForm)_scopedService;
        }

        public void Dispose()
        {
            //The factory methods uses new() (outside the container) because of the parameter
            //so we have to dispose of the service manually (_scope.Dispose() will not dispose _scopedService).
            _scopedService?.Dispose();
        }
    }
}

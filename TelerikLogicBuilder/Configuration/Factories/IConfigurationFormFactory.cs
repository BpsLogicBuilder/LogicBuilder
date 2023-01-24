using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface IConfigurationFormFactory : IDisposable
    {
        IConfigureConnectorObjectsForm GetConfigureConnectorObjectsForm(bool openedAsReadOnly);
        IConfigureGenericArgumentsForm GetConfigureConstructorGenericArgumentsForm(XmlDocument xmlDocument,
            IList<string> configuredGenericArgumentNames,
            IList<ParameterBase> memberParameters,
            Type genericTypeDefinition);
        IConfigureConstructorsForm GetConfigureConstructorsForm(bool openedAsReadOnly);
        IConfigureExcludedModulesForm GetConfigureExcludedModules(IList<string> excludedModules);
        IConfigureGenericArgumentsForm GetConfigureFunctionGenericArgumentsForm(XmlDocument xmlDocument,
            IList<string> configuredGenericArgumentNames,
            IList<ParameterBase> memberParameters,
            Type genericTypeDefinition);
        IConfigureFunctionsForm GetConfigureFunctionsForm(bool openedAsReadOnly);
        IConfigureLiteralDomainForm GetConfigureLiteralDomainForm(IList<string> existingDomainItems, Type type);
        IConfigureLiteralListDefaultValueForm GetConfigureLiteralListDefaultValueForm(IList<string> existingDefaultValueItems, Type type);
        IConfigureLoadAssemblyPathsForm GetConfigureLoadAssemblyPaths(IList<string> existingPaths);
        IConfigureProjectPropertiesForm GetConfigureProjectProperties(bool openedAsReadOnly);
        IConfigureReturnTypeForm GetConfigureReturnTypeForm(IList<string> genericArguments, ReturnTypeBase returnType);
        IConfigureVariablesForm GetConfigureVariablesForm(bool openedAsReadOnly);
        IConfigureWebApiDeploymentForm GetConfigureWebApiDeploymentForm(WebApiDeployment webApiDeployment);
        IEditGenericArgumentsForm GetEditGenericArgumentsForm(IList<string> existingArguments);
    }
}

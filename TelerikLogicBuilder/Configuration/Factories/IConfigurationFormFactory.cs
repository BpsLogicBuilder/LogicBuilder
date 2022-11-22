using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConnectorObjects;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureExcludedModules;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLoadAssemblyPaths;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface IConfigurationFormFactory : IDisposable
    {
        ConfigureConnectorObjectsForm GetConfigureConnectorObjectsForm(bool openedAsReadOnly);
        ConfigureConstructorGenericArgumentsForm GetConfigureConstructorGenericArgumentsForm(XmlDocument xmlDocument,
            IList<string> configuredGenericArgumentNames,
            IList<ParameterBase> memberParameters,
            Type genericTypeDefinition);
        ConfigureExcludedModulesForm GetConfigureExcludedModules(IList<string> excludedModules);
        ConfigureFunctionGenericArgumentsForm GetConfigureFunctionGenericArgumentsForm(XmlDocument xmlDocument,
            IList<string> configuredGenericArgumentNames,
            IList<ParameterBase> memberParameters,
            Type genericTypeDefinition);
        ConfigureLoadAssemblyPathsForm GetConfigureLoadAssemblyPaths(IList<string> existingPaths);
        ConfigureProjectProperties GetConfigureProjectProperties(bool openedAsReadOnly);
        ConfigureWebApiDeployment GetConfigureWebApiDeployment(WebApiDeployment webApiDeployment);
    }
}

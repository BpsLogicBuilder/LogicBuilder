using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface IConfigurationFormFactory : IDisposable
    {
        ConfigureExcludedModules GetConfigureExcludedModules(IList<string> excludedModules);
        ConfigureLoadAssemblyPaths GetConfigureLoadAssemblyPaths(IList<string> existingPaths);
        ConfigureProjectProperties GetConfigureProjectProperties(bool openedAsReadOnly);
        ConfigureWebApiDeployment GetConfigureWebApiDeployment(WebApiDeployment webApiDeployment);
    }
}

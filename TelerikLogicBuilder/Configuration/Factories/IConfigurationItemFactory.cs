using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface IConfigurationItemFactory
    {
        Application GetApplication(string name, string nickname, string activityAssembly, string activityAssemblyPath, RuntimeType runtime, List<string> loadAssemblyPaths, string activityClass, string applicationExcecutable, string applicationExcecutablePath, List<string> startupArguments, string resourceFile, string resourceFileDeploymentPath, string rulesFile, string rulesDeploymentPath, List<string> modules, WebApiDeployment webApiDeployment);
        Fragment GetFragment(string name, string xml);
        ProjectProperties GetProjectProperties(string projectName, string projectPath, Dictionary<string, Application> applicationList, HashSet<string> connectorObjectTypes);
        WebApiDeployment GetWebApiDeployment(string postFileDataUrl, string postVariablesMetaUrl, string deleteRulesUrl, string deleteAllRulesUrl);
    }
}

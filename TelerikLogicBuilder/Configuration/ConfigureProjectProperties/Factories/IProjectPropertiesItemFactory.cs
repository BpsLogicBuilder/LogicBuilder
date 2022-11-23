using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal interface IProjectPropertiesItemFactory
    {
        Application GetApplication(string name, string nickname, string activityAssembly, string activityAssemblyPath, RuntimeType runtime, List<string> loadAssemblyPaths, string activityClass, string applicationExcecutable, string applicationExcecutablePath, List<string> startupArguments, string resourceFile, string resourceFileDeploymentPath, string rulesFile, string rulesDeploymentPath, List<string> modules, WebApiDeployment webApiDeployment);
        ProjectProperties GetProjectProperties(string projectName, string projectPath, Dictionary<string, Application> applicationList, HashSet<string> connectorObjectTypes);
    }
}

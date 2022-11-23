using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal class ProjectPropertiesItemFactory : IProjectPropertiesItemFactory
    {
        private readonly Func<string, string, string, string, RuntimeType, List<string>, string, string, string, List<string>, string, string, string, string, List<string>, WebApiDeployment, Application> _getApplication;
        private readonly Func<string, string, Dictionary<string, Application>, HashSet<string>, ProjectProperties> _getProjectProperties;

        public ProjectPropertiesItemFactory(
            Func<string, string, string, string, RuntimeType, List<string>, string, string, string, List<string>, string, string, string, string, List<string>, WebApiDeployment, Application> getApplication,
            Func<string, string, Dictionary<string, Application>, HashSet<string>, ProjectProperties> getProjectProperties)
        {
            _getApplication = getApplication;
            _getProjectProperties = getProjectProperties;
        }

        public Application GetApplication(
            string name,
            string nickname,
            string activityAssembly,
            string activityAssemblyPath,
            RuntimeType runtime,
            List<string> loadAssemblyPaths,
            string activityClass,
            string applicationExcecutable,
            string applicationExcecutablePath,
            List<string> startupArguments,
            string resourceFile,
            string resourceFileDeploymentPath,
            string rulesFile,
            string rulesDeploymentPath,
            List<string> modules,
            WebApiDeployment webApiDeployment)
            => _getApplication(
                name,
                nickname,
                activityAssembly,
                activityAssemblyPath,
                runtime,
                loadAssemblyPaths,
                activityClass,
                applicationExcecutable,
                applicationExcecutablePath,
                startupArguments,
                resourceFile,
                resourceFileDeploymentPath,
                rulesFile,
                rulesDeploymentPath,
                modules,
                webApiDeployment);

        public ProjectProperties GetProjectProperties(string projectName, string projectPath, Dictionary<string, Application> applicationList, HashSet<string> connectorObjectTypes)
            => _getProjectProperties(projectName, projectPath, applicationList, connectorObjectTypes);
    }
}

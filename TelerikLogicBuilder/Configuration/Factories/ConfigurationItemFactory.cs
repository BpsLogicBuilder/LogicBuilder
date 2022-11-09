using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal class ConfigurationItemFactory : IConfigurationItemFactory
    {
        private readonly Func<string, string, string, string, RuntimeType, List<string>, string, string, string, List<string>, string, string, string, string, List<string>, WebApiDeployment, Application> _getApplication;
        private readonly Func<string, AssemblyPath> _getAssemblyPath;
        private readonly Func<string, string, Fragment> _getFragment;
        private readonly Func<string, string, Dictionary<string, Application>, HashSet<string>, ProjectProperties> _getProjectProperties;
        private readonly Func<string, string, string, string, WebApiDeployment> _getWebApiDeployment;

        public ConfigurationItemFactory(
            Func<string, string, string, string, RuntimeType, List<string>, string, string, string, List<string>, string, string, string, string, List<string>, WebApiDeployment, Application> getApplication,
            Func<string, AssemblyPath> getAssemblyPath,
            Func<string, string, Fragment> getFragment,
            Func<string, string, Dictionary<string, Application>, HashSet<string>, ProjectProperties> getProjectProperties,
            Func<string, string, string, string, WebApiDeployment> getWebApiDeployment)
        {
            _getApplication = getApplication;
            _getAssemblyPath = getAssemblyPath;
            _getFragment = getFragment;
            _getProjectProperties = getProjectProperties;
            _getWebApiDeployment = getWebApiDeployment;
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

        public AssemblyPath GetAssemblyPath(string path)
            => _getAssemblyPath(path);

        public Fragment GetFragment(string name, string xml)
            => _getFragment(name, xml);

        public ProjectProperties GetProjectProperties(string projectName, string projectPath, Dictionary<string, Application> applicationList, HashSet<string> connectorObjectTypes)
            => _getProjectProperties(projectName, projectPath, applicationList, connectorObjectTypes);

        public WebApiDeployment GetWebApiDeployment(string postFileDataUrl, string postVariablesMetaUrl, string deleteRulesUrl, string deleteAllRulesUrl)
            => _getWebApiDeployment(postFileDataUrl, postVariablesMetaUrl, deleteRulesUrl, deleteAllRulesUrl);
    }
}

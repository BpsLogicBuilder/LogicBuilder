using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration
{
    internal class CreateDefaultApplication : ICreateDefaultApplication
    {
        private readonly IPathHelper _pathHelper;
        private readonly IProjectPropertiesItemFactory _projectPropertiesItemFactory;
        private readonly IWebApiDeploymentItemFactory _webApiDeploymentItemFactory;

        public CreateDefaultApplication(
            IPathHelper pathHelper,
            IProjectPropertiesItemFactory projectPropertiesItemFactory,
            IWebApiDeploymentItemFactory webApiDeploymentItemFactory)
        {
            _projectPropertiesItemFactory = projectPropertiesItemFactory;
            _pathHelper = pathHelper;
            _webApiDeploymentItemFactory = webApiDeploymentItemFactory;
        }

        public Application Create(string applicationNameString)
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            return _projectPropertiesItemFactory.GetApplication
            (
                applicationNameString,
                applicationNameString,
                string.Concat(applicationNameString, Strings.dotExe),
                _pathHelper.CombinePaths(programFilesPath, applicationNameString, Strings.defaultActivityAssemblyFolder),
                RuntimeType.NetCore,
                new List<string>(),
                Strings.defaultActivityClass,
                string.Concat(applicationNameString, Strings.dotExe),
                _pathHelper.CombinePaths(programFilesPath, applicationNameString),
                new List<string>(),
                Strings.defaultResourcesFile,
                _pathHelper.CombinePaths(programFilesPath, applicationNameString, Strings.defaultResourcesFolder),
                Strings.defaultRulesFile,
                _pathHelper.CombinePaths(programFilesPath, applicationNameString, Strings.defaultRulesFolder),
                new List<string>(),
                _webApiDeploymentItemFactory.GetWebApiDeployment
                (
                    Strings.defaultPostFileDataUrl,
                    Strings.defaultPostVariableMetaDataUrl,
                    Strings.defaultDeleteRulesUrl,
                    Strings.defaultDeleteAllRulesUrl
                )
            );
        }
    }
}

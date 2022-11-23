using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class UpdateProjectPropertiesTest
    {
        public UpdateProjectPropertiesTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanUpdateProjectProperties()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            IUpdateProjectProperties updateProjectProperties = serviceProvider.GetRequiredService<IUpdateProjectProperties>();
            ILoadProjectProperties loadProjectProperties = serviceProvider.GetRequiredService<ILoadProjectProperties>();
            IProjectPropertiesItemFactory projectPropertiesItemFactory = serviceProvider.GetRequiredService<IProjectPropertiesItemFactory>();
            IWebApiDeploymentItemFactory webApiDeploymentItemFactory = serviceProvider.GetRequiredService<IWebApiDeploymentItemFactory>();
            ProjectProperties projectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanUpdateProjectProperties)
            );

            //act
            updateProjectProperties.Update
            (
                projectProperties.ProjectFileFullName,
                new Dictionary<string, Application>
                {
                    ["App01"] = projectPropertiesItemFactory.GetApplication
                    (
                        "App01",
                        "App01",
                        "CanUpdateProjectProperties.dll",
                        "PathToAssembly",
                        RuntimeType.NetCore,
                        new List<string>(),
                        "CanUpdateProjectProperties.FlowActivity",
                        "CanUpdateProjectProperties.exe",
                        "PathToRxe",
                        new List<string>(),
                        "ResourceFileName",
                        "PathToResourceFile",
                        "RulesFileName",
                        "PathToRulesFile",
                        new List<string>(),
                        webApiDeploymentItemFactory.GetWebApiDeployment
                        (
                            "http://localhost:3677/api/transfer/PostFileData",
                            "http://localhost:3677/api/transfer/PostVariableMetaData",
                            "http://localhost:3677/api/transfer/DeleteRules",
                            "http://localhost:3677/api/transfer/DeleteAllRules"
                        )
                    )
                }, 
                new HashSet<string> { "Flow1.ConnectorData", "Flow2.ConnectorData" }
            );
            ProjectProperties loadedProperties = loadProjectProperties.Load(projectProperties.ProjectFileFullName);

            //assert
            Assert.Equal("App01", loadedProperties.ApplicationList.First().Value.Name);
            Assert.True(loadedProperties.ConnectorObjectTypes.SetEquals(new List<string> { "Flow1.ConnectorData", "Flow2.ConnectorData" }));
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class LoadProjectPropertiesTest
    {
        public LoadProjectPropertiesTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void CanLoadProjectProperties()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ILoadProjectProperties loadProjectProperties = serviceProvider.GetRequiredService<ILoadProjectProperties>();
            ProjectProperties projectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanLoadProjectProperties)
            );

            //act
            ProjectProperties loadedProperties = loadProjectProperties.Load(projectProperties.ProjectFileFullName);

            //assert
            Assert.Equal(projectProperties.ProjectFileFullName, loadedProperties.ProjectFileFullName);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class LoadFragmentsTest
    {
        public LoadFragmentsTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanLoadFragmentsFile()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ICreateFragments createFragments = serviceProvider.GetRequiredService<ICreateFragments>();
            ILoadFragments loadFragments = serviceProvider.GetRequiredService<ILoadFragments>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanLoadFragmentsFile)
            );
            createFragments.Create();

            //act
            var result = loadFragments.Load();

            //assert
            Assert.Equal(XmlDataConstants.FOLDERELEMENT, result.DocumentElement!.Name);
        }
    }
}

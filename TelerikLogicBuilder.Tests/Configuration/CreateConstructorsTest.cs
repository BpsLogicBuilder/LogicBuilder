using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class CreateConstructorsTest
    {
        public CreateConstructorsTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateConstructor()
        {
            //arrange
            ICreateProjectProperties createProjectProperties = serviceProvider.GetRequiredService<ICreateProjectProperties>();
            IConfigurationService configurationService = serviceProvider.GetRequiredService<IConfigurationService>();
            IPathHelper pathHelper = serviceProvider.GetRequiredService<IPathHelper>();
            ICreateConstructors createConstructors = serviceProvider.GetRequiredService<ICreateConstructors>();
            configurationService.ProjectProperties = createProjectProperties.Create
            (
                pathHelper.CombinePaths(TestFolders.LogicBuilderTests, this.GetType().Name),
                nameof(CanCreateConstructor)
            );

            //act
            var result = createConstructors.Create();

            //assert
            Assert.Equal(XmlDataConstants.FORMELEMENT, result.DocumentElement.Name);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

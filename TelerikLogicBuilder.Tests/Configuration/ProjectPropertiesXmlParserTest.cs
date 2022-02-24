using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class ProjectPropertiesXmlParserTest
    {
        public ProjectPropertiesXmlParserTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateProjectPropertiesXmlParser()
        {
            //arrange
            IProjectPropertiesXmlParser projectPropertiesXmlParser = serviceProvider.GetRequiredService<IProjectPropertiesXmlParser>();

            //assert
            Assert.NotNull(projectPropertiesXmlParser);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

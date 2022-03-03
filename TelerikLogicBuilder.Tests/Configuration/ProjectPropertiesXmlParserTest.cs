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
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateProjectPropertiesXmlParser()
        {
            //arrange
            IProjectPropertiesXmlParser projectPropertiesXmlParser = serviceProvider.GetRequiredService<IProjectPropertiesXmlParser>();

            //assert
            Assert.NotNull(projectPropertiesXmlParser);
        }
    }
}

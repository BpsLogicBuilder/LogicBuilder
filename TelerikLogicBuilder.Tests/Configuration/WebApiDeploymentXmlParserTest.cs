using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class WebApiDeploymentXmlParserTest
    {
        public WebApiDeploymentXmlParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateWebApiDeploymentXmlParser()
        {
            //arrange
            IWebApiDeploymentXmlParser webApiDeploymentXmlParser = serviceProvider.GetRequiredService<IWebApiDeploymentXmlParser>();

            //assert
            Assert.NotNull(webApiDeploymentXmlParser);
        }
    }
}

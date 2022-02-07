using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class WebApiDeploymentXmlParserTest
    {
        public WebApiDeploymentXmlParserTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void CanCreateWebApiDeploymentXmlParser()
        {
            //arrange
            IWebApiDeploymentXmlParser webApiDeploymentXmlParser = serviceProvider.GetRequiredService<IWebApiDeploymentXmlParser>();

            //assert
            Assert.NotNull(webApiDeploymentXmlParser);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

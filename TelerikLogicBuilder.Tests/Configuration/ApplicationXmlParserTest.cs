using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class ApplicationXmlParserTest
    {
        public ApplicationXmlParserTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateApplicationXmlParser()
        {
            //arrange
            IApplicationXmlParser applicationXmlParser = serviceProvider.GetRequiredService<IApplicationXmlParser>();

            //assert
            Assert.NotNull(applicationXmlParser);
        }
    }
}

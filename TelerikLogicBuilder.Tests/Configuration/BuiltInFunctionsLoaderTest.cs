using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class BuiltInFunctionsLoaderTest
    {
        public BuiltInFunctionsLoaderTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void BuiltInFunctionsLoaderWorks()
        {
            //arrange
            IBuiltInFunctionsLoader builtInFunctionsLoader = serviceProvider.GetRequiredService<IBuiltInFunctionsLoader>();

            //act
            var result = builtInFunctionsLoader.Load();
            var builtInFuctionsNodeElement = result.SelectSingleNode($"/form[@name='{XmlDataConstants.BUILTINFUNCTIONSFORMROOTNODENAME}']");

            //assert
            Assert.NotNull(builtInFuctionsNodeElement);
        }
    }
}

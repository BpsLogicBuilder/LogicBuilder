using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration
{
    public class BuiltInFunctionsLoaderTest
    {
        public BuiltInFunctionsLoaderTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

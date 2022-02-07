using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Contoso.Forms.Parameters.DataForm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense
{
    public class ConstructorManagerTest
    {
        public ConstructorManagerTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.IntegrationTest)]
        public void CreateConstructor()
        {
            //arrange
            IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
            ConstructorInfo constructorInfo = typeof(DataFormSettingsParameters).GetConstructors().First();

            //act
            Constructor result = contextProvider.ConstructorManager.CreateConstructor(constructorInfo.Name, constructorInfo);

            //assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Parameters.Count);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

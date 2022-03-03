using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using Contoso.Forms.Parameters.DataForm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Constructors
{
    public class ConstructorManagerTest
    {
        public ConstructorManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CreateConstructor()
        {
            //arrange
            IConstructorManager constructorManager = serviceProvider.GetRequiredService<IConstructorManager>();
            ConstructorInfo constructorInfo = typeof(DataFormSettingsParameters).GetConstructors().First();

            //act
            Constructor? result = constructorManager.CreateConstructor(constructorInfo.Name, constructorInfo);

            //assert
            Assert.NotNull(result);
            Assert.Equal(10, result!.Parameters.Count);
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
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
            ConstructorInfo constructorInfo = typeof(TestClassWithChildContructor).GetConstructors().First();

            //act
            Constructor? result = constructorManager.CreateConstructor(constructorInfo.Name, constructorInfo);

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result!.Parameters.Count);
        }

        private class TestClassWithChildContructor
        {
            public TestClassWithChildContructor(string stringProperty, ChildContructor childContructor)
            {
                StringProperty = stringProperty;
                ChildContructor = childContructor;
            }

            public string StringProperty { get; set; }
            public ChildContructor ChildContructor { get; set; }
        }

        private class ChildContructor
        {
            public ChildContructor(string stringProperty)
            {
                StringProperty = stringProperty;
            }

            public string StringProperty { get; set; }
        }
    }
}

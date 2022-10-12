using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Constructors
{
    public class ExistingConstructorFinderTest
    {
        public ExistingConstructorFinderTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void ReturnsExistingConstructor()
        {
            //arrange
            IConstructorManager constructorManager = serviceProvider.GetRequiredService<IConstructorManager>();
            IExistingConstructorFinder existingConstructorFinder = serviceProvider.GetRequiredService<IExistingConstructorFinder>();
            IConstructorFactory constructorFactory = serviceProvider.GetRequiredService<IConstructorFactory>();
            ConstructorInfo constructorInfo = typeof(TestClassWithContructor).GetConstructors().First();
            Constructor? constructor = constructorManager.CreateConstructor(constructorInfo.Name, constructorInfo);
            Dictionary<string, Constructor> existingConstructors = new()
            {
                [constructorInfo.Name] = constructorFactory.GetConstructor
                (
                    constructor!.Name,
                    constructor.TypeName,
                    constructor.Parameters,
                    constructor.GenericArguments,
                    constructor.Summary
                )
            };

            //act
            Constructor? foundConstructor = existingConstructorFinder.FindExisting(constructorInfo, existingConstructors);

            //assert
            Assert.NotNull(foundConstructor);
        }

        [Fact]
        public void ReturnsNullWhenConstructorNotFound()
        {
            //arrange
            IConstructorManager constructorManager = serviceProvider.GetRequiredService<IConstructorManager>();
            IExistingConstructorFinder existingConstructorFinder = serviceProvider.GetRequiredService<IExistingConstructorFinder>();
            IConstructorFactory constructorFactory = serviceProvider.GetRequiredService<IConstructorFactory>();
            ConstructorInfo constructorInfo = typeof(TestClassWithContructor).GetConstructors().First();
            Constructor? constructor = constructorManager.CreateConstructor(constructorInfo.Name, constructorInfo);
            Dictionary<string, Constructor> existingConstructors = new()
            {
                [constructorInfo.Name] = constructorFactory.GetConstructor
                (
                    constructor!.Name,
                    constructor.TypeName,
                    constructor.Parameters,
                    constructor.GenericArguments,
                    constructor.Summary
                )
            };

            //act
            Constructor? foundConstructor = existingConstructorFinder.FindExisting(constructorInfo, existingConstructors);

            //assert
            Assert.NotNull(foundConstructor);
        }

        private class TestClassWithContructor
        {
            public TestClassWithContructor(string stringProperty)
            {
                StringProperty = stringProperty;
            }

            public string StringProperty { get; set; }
        }
    }
}

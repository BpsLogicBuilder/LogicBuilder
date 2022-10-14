using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Constructors
{
    public class ChildConstructorFinderTest
    {
        public ChildConstructorFinderTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void AddChildConstructorsAddsExpectedConstructors()
        {
            //arrange
            Dictionary<string, Constructor> existingConstructors = new();
            IChildConstructorFinder finder = serviceProvider.GetRequiredService<IChildConstructorFinderFactory>().GetChildConstructorFinder(existingConstructors);
            ParameterInfo[] parameters = typeof(TestClassWithChildContructor).GetConstructors().First().GetParameters();

            //act
            finder.AddChildConstructors(parameters);

            //assert
            Assert.NotEmpty(existingConstructors);
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

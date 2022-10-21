using ABIS.LogicBuilder.FlowBuilder.Reflection.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.Reflection
{
    public class LoadAssemblyFromNameTest
    {
        public LoadAssemblyFromNameTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateReflectionFactory()
        {
            //arrange
            IReflectionFactory reflectionFactory = serviceProvider.GetRequiredService<IReflectionFactory>();

            //assert
            Assert.NotNull(reflectionFactory);
        }

        [Fact]
        public void CreateLoadAssemblyFromNameThrows()
        {
            //assert
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<ILoadAssemblyFromName>());
        }
    }
}

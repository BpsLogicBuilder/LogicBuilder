using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.Reflection
{
    public class LoadAssemblyFromPathTest
    {
        public LoadAssemblyFromPathTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateAssemblyLoader()
        {
            //arrange
            ILoadAssemblyFromPath assemblyLoader = serviceProvider.GetRequiredService<ILoadAssemblyFromPath>();

            //assert
            Assert.NotNull(assemblyLoader);
        }
    }
}

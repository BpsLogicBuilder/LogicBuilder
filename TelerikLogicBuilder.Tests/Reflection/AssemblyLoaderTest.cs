using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.Reflection
{
    public class AssemblyLoaderTest
    {
        public AssemblyLoaderTest()
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
            IAssemblyLoader assemblyLoader = serviceProvider.GetRequiredService<IAssemblyLoader>();

            //assert
            Assert.NotNull(assemblyLoader);
        }
    }
}

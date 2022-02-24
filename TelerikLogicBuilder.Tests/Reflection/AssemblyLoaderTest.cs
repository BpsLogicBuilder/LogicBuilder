using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Reflection
{
    public class AssemblyLoaderTest
    {
        public AssemblyLoaderTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateAssemblyLoader()
        {
            //arrange
            IAssemblyLoader assemblyLoader = serviceProvider.GetRequiredService<IAssemblyLoader>();

            //assert
            Assert.NotNull(assemblyLoader);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

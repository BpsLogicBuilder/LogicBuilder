using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.Reflection
{
    public class AssemblyHelperTest
    {
        public AssemblyHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateAssemblyHelper()
        {
            //arrange
            IAssemblyHelper assemblyHelper = serviceProvider.GetRequiredService<IAssemblyHelper>();

            //assert
            Assert.NotNull(assemblyHelper);
        }
    }
}

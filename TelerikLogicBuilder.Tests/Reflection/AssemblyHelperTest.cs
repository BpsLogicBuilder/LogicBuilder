using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Reflection
{
    public class AssemblyHelperTest
    {
        public AssemblyHelperTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateAssemblyHelper()
        {
            //arrange
            IAssemblyHelper assemblyHelper = serviceProvider.GetRequiredService<IAssemblyHelper>();

            //assert
            Assert.NotNull(assemblyHelper);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Reflection
{
    public class ApplicationTypeInfoManagerTest
    {
        public ApplicationTypeInfoManagerTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateApplicationTypeInfoManager()
        {
            //arrange
            IApplicationTypeInfoManager applicationTypeInfoManager = serviceProvider.GetRequiredService<IApplicationTypeInfoManager>();

            //assert
            Assert.NotNull(applicationTypeInfoManager);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

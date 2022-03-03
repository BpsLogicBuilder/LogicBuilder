using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.Reflection
{
    public class ApplicationTypeInfoManagerTest
    {
        public ApplicationTypeInfoManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateApplicationTypeInfoManager()
        {
            //arrange
            IApplicationTypeInfoManager applicationTypeInfoManager = serviceProvider.GetRequiredService<IApplicationTypeInfoManager>();

            //assert
            Assert.NotNull(applicationTypeInfoManager);
        }
    }
}

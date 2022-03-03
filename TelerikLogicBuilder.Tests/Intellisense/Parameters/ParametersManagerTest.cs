using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using Contoso.Forms.Parameters.DataForm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Parameters
{
    public class ParametersManagerTest
    {
        public ParametersManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void GetParameterNodeInfos()
        {
            //arrange
            IParametersManager parametersManager = serviceProvider.GetRequiredService<IParametersManager>();
            ParameterInfo[] parameters = typeof(DataFormSettingsParameters).GetConstructors().First().GetParameters();

            //act
            ICollection<ParameterNodeInfoBase> result = parametersManager.GetParameterNodeInfos(parameters);

            //assert
            Assert.NotEmpty(result);
        }
    }
}

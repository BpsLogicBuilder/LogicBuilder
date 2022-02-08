using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using Contoso.Forms.Parameters.DataForm;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense
{
    public class ParametersManagerTest
    {
        public ParametersManagerTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.IntegrationTest)]
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

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

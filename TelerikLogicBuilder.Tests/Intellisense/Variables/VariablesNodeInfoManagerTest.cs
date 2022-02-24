using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using Contoso.Parameters.Expansions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Variables
{
    public class VariablesNodeInfoManagerTest
    {
        public VariablesNodeInfoManagerTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData("MemberName", typeof(LiteralVariableNodeInfo))]
        [InlineData("Filter", typeof(ObjectVariableNodeInfo))]
        [InlineData("Selects", typeof(ListOfLiteralsVariableNodeInfo))]
        [InlineData("ExpandedItems", typeof(ListOfObjectsVariableNodeInfo))]
        public void GetVariableWorks(string memberName, Type nodeInfoType)
        {
            //arrange
            IVariablesNodeInfoManager variablesManager = serviceProvider.GetRequiredService<IVariablesNodeInfoManager>();
            PropertyInfo propertyInfo = typeof(SelectExpandItemParameters).GetProperty(memberName);

            //act
            var result = variablesManager.GetVariableNodeInfo(propertyInfo, propertyInfo.PropertyType);

            //assert
            Assert.Equal(nodeInfoType, result.GetType());
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

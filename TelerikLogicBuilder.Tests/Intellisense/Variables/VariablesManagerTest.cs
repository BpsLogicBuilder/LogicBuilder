using ABIS.LogicBuilder.FlowBuilder.Enums;
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
    public class VariablesManagerTest
    {
        public VariablesManagerTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData("MemberName", typeof(LiteralVariableNodeInfo), typeof(LiteralVariable))]
        [InlineData("Filter", typeof(ObjectVariableNodeInfo), typeof(ObjectVariable))]
        [InlineData("Selects", typeof(ListOfLiteralsVariableNodeInfo), typeof(ListOfLiteralsVariable))]
        [InlineData("ExpandedItems", typeof(ListOfObjectsVariableNodeInfo), typeof(ListOfObjectsVariable))]
        [Trait(TraitTypes.TestCategory, TestCategories.IntegrationTest)]
        public void GetVariableWorks(string memberName, Type nodeInfoType, Type variableType)
        {
            //arrange
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            PropertyInfo propertyInfo = typeof(SelectExpandItemParameters).GetProperty(memberName);

            //act
            var result = variablesManager.GetVariableNodeInfo(propertyInfo, propertyInfo.PropertyType);
            var variable = result.GetVariable
            (
                propertyInfo.Name, 
                propertyInfo.Name, 
                VariableCategory.Property, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                ReferenceCategories.This
            );

            //assert
            Assert.Equal(nodeInfoType, result.GetType());
            Assert.Equal(variableType, variable.GetType());
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

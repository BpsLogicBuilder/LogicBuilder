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
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData("MemberName", typeof(LiteralVariable))]
        [InlineData("Filter", typeof(ObjectVariable))]
        [InlineData("Selects", typeof(ListOfLiteralsVariable))]
        [InlineData("ExpandedItems", typeof(ListOfObjectsVariable))]
        public void GetVariableWorks(string memberName, Type variableType)
        {
            //arrange
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            PropertyInfo propertyInfo = typeof(SelectExpandItemParameters).GetProperty(memberName)!;

            //act
            var variable = variablesManager.GetVariable
            (
                propertyInfo.Name, 
                propertyInfo.Name, 
                VariableCategory.Property, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                string.Empty, 
                ReferenceCategories.This,
                propertyInfo, 
                propertyInfo.PropertyType
            );

            //assert
            Assert.Equal(variableType, variable.GetType());
        }
    }
}

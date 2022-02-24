using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Variables
{
    public class VariableHelperTest
    {
        public VariableHelperTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData("Name", false)]
        [InlineData("MyInt", true)]
        [InlineData("MyNullableInt", false)]
        [InlineData("MyShort", true)]
        [InlineData("MyByte", true)]
        public void CanBeIntegerWorks(string memberName, bool expectedResult)
        {
            //arrange
            IVariableHelper helper = serviceProvider.GetRequiredService<IVariableHelper>();
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            PropertyInfo propertyInfo = typeof(TestVariableClass).GetProperty(memberName);

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
            var result = helper.CanBeInteger(variable);

            //assert
            Assert.Equal(expectedResult, result);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        class TestVariableClass
        {
            public string Name { get; set; }
            public int MyInt { get; set; }
            public int? MyNullableInt { get; set; }
            public short MyShort { get; set; }
            public byte MyByte { get; set; }
        }
    }
}

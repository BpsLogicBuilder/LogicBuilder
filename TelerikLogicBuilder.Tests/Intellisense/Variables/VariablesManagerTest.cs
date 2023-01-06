using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        [InlineData("StringProperty", typeof(LiteralVariable))]
        [InlineData("ChildContructor", typeof(ObjectVariable))]
        [InlineData("LiteralList", typeof(ListOfLiteralsVariable))]
        [InlineData("ObjectList", typeof(ListOfObjectsVariable))]
        public void GetVariableWorks(string memberName, Type variableType)
        {
            //arrange
            IVariablesManager variablesManager = serviceProvider.GetRequiredService<IVariablesManager>();
            PropertyInfo propertyInfo = typeof(TestClassWithChildContructor).GetProperty(memberName)!;

            //act
            var variable = variablesManager.GetVariable
            (
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

        private class TestClassWithChildContructor
        {
            public TestClassWithChildContructor(string stringProperty, ChildContructor childContructor, List<string> literalList, List<ChildContructor> objectList)
            {
                StringProperty = stringProperty;
                ChildContructor = childContructor;
                LiteralList = literalList;
                ObjectList = objectList;
            }

            public string StringProperty { get; set; }
            public ChildContructor ChildContructor { get; set; }
            public List<string> LiteralList { get; set; }
            public List<ChildContructor> ObjectList { get; set; }
        }

        private class ChildContructor
        {
            public ChildContructor(string stringProperty)
            {
                StringProperty = stringProperty;
            }

            public string StringProperty { get; set; }
        }
    }
}

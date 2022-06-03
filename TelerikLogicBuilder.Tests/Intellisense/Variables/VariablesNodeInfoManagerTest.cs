using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Variables
{
    public class VariablesNodeInfoManagerTest
    {
        public VariablesNodeInfoManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData("StringProperty", typeof(LiteralVariableNodeInfo))]
        [InlineData("ChildContructor", typeof(ObjectVariableNodeInfo))]
        [InlineData("LiteralList", typeof(ListOfLiteralsVariableNodeInfo))]
        [InlineData("ObjectList", typeof(ListOfObjectsVariableNodeInfo))]
        public void GetVariableWorks(string memberName, Type nodeInfoType)
        {
            //arrange
            IVariablesNodeInfoManager variablesManager = serviceProvider.GetRequiredService<IVariablesNodeInfoManager>();
            PropertyInfo propertyInfo = typeof(TestClassWithChildContructor).GetProperty(memberName)!;

            //act
            var result = variablesManager.GetVariableNodeInfo(propertyInfo, propertyInfo.PropertyType);

            //assert
            Assert.Equal(nodeInfoType, result.GetType());
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

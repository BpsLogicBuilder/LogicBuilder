using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Functions
{
    public class ReturnTypeManagerTest
    {
        public ReturnTypeManagerTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData("LiteralMethod", ReturnTypeCategory.Literal)]
        [InlineData("ObjectMethod", ReturnTypeCategory.Object)]
        [InlineData("GenericMethod", ReturnTypeCategory.Generic)]
        [InlineData("LiteralListMethod", ReturnTypeCategory.LiteralList)]
        [InlineData("ObjectListMethod", ReturnTypeCategory.ObjectList)]
        [InlineData("GenericListMethod", ReturnTypeCategory.GenericList)]
        internal void GetReturnTypeInfoReturnsExpectedResult(string methodName, ReturnTypeCategory expectedResult)
        {
            //arrange
            IReturnTypeManager returnTypeManager = serviceProvider.GetRequiredService<IReturnTypeManager>();
            MethodInfo methodInfo = typeof(TestParameterClass<>).GetMethod(methodName);

            //act
            var result = returnTypeManager.GetReturnTypeInfo(methodInfo).GetReturnType();

            //assert
            Assert.Equal(expectedResult, result.ReturnTypeCategory);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        class TestParameterClass<T>
        {
            public void LiteralMethod()
            {
                throw new NotImplementedException();
            }

            public object ObjectMethod()
            {
                throw new NotImplementedException();
            }

            public T GenericMethod()
            {
                throw new NotImplementedException();
            }

            public List<string> LiteralListMethod()
            {
                throw new NotImplementedException();
            }

            public object[] ObjectListMethod()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> GenericListMethod()
            {
                throw new NotImplementedException();
            }
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Functions
{
    public class FunctionManagerTest
    {
        public FunctionManagerTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData("LiteralMethod", 2, ReturnTypeCategory.Literal)]
        [InlineData("ObjectMethod", 2, ReturnTypeCategory.Object)]
        [InlineData("GenericMethod", 2, ReturnTypeCategory.Generic)]
        [InlineData("LiteralListMethod", 2, ReturnTypeCategory.LiteralList)]
        [InlineData("ObjectListMethod", 2, ReturnTypeCategory.ObjectList)]
        [InlineData("GenericListMethod", 2, ReturnTypeCategory.GenericList)]
        internal void CreateFunction(string methodName, int expectedParameterCount, ReturnTypeCategory expectedReturnTypeCategory)
        {
            //arrange
            IFunctionManager functionManager = serviceProvider.GetRequiredService<IFunctionManager>();
            MethodInfo methodInfo = typeof(TestParameterClass<>).GetMethod(methodName);

            //act
            Function result = functionManager.GetFunction(methodInfo.Name, methodInfo.Name, FunctionCategories.Standard, string.Empty, string.Empty, string.Empty, string.Empty, ReferenceCategories.This, ParametersLayout.Sequential, methodInfo);

            //assert
            Assert.Equal(expectedParameterCount, result.Parameters.Count);
            Assert.Equal(expectedReturnTypeCategory, result.ReturnType.ReturnTypeCategory);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        class TestParameterClass<T>
        {
            public void LiteralMethod(string parameter1, object parameter2)
            {
                throw new NotImplementedException();
            }

            public object ObjectMethod(string parameter1, object parameter2)
            {
                throw new NotImplementedException();
            }

            public T GenericMethod(string parameter1, object parameter2)
            {
                throw new NotImplementedException();
            }

            public List<string> LiteralListMethod(string parameter1, object parameter2)
            {
                throw new NotImplementedException();
            }

            public object[] ObjectListMethod(string parameter1, object parameter2)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> GenericListMethod(string parameter1, object parameter2)
            {
                throw new NotImplementedException();
            }
        }
    }
}

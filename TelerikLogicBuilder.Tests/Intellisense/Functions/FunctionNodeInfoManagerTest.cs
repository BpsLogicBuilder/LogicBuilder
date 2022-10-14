using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Functions
{
    public class FunctionNodeInfoManagerTest
    {
        public FunctionNodeInfoManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData("LiteralMethod")]
        [InlineData("ObjectMethod")]
        [InlineData("GenericMethod")]
        [InlineData("LiteralListMMethod")]
        [InlineData("ObjectListMMethod")]
        [InlineData("GenericListMMethod")]
        internal void CreateFunction(string methodName)
        {
            //arrange
            IFunctionNodeInfoFactory functionNodeInfoFactory = serviceProvider.GetRequiredService<IFunctionNodeInfoFactory>();
            MethodInfo methodInfo = typeof(TestParameterClass<>).GetMethod(methodName)!;

            //act
            var result = functionNodeInfoFactory.GetFunctionNodeInfo(methodInfo);

            //assert
            Assert.NotNull(result);
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

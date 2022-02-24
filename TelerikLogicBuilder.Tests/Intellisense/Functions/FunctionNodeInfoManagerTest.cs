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
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
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
            IFunctionNodeInfoManager functionManager = serviceProvider.GetRequiredService<IFunctionNodeInfoManager>();
            MethodInfo methodInfo = typeof(TestParameterClass<>).GetMethod(methodName);

            //act
            var result = functionManager.GetFunctionNodeInfo(methodInfo);

            //assert
            Assert.NotNull(result);
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

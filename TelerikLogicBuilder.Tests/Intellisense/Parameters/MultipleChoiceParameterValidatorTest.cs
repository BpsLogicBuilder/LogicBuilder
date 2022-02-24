using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using LogicBuilder.Forms.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Parameters
{
    public class MultipleChoiceParameterValidatorTest
    {
        public MultipleChoiceParameterValidatorTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData("ValidMethod", true)]
        [InlineData("ValidMethod2", true)]
        [InlineData("ValidMethod3", true)]
        [InlineData("InvalidMethod", false)]
        public void ValidateMultipleChoiceParameterReturnsExpectedResult(string methodName, bool expectedResult)
        {
            //arrange
            IMultipleChoiceParameterValidator validator = serviceProvider.GetRequiredService<IMultipleChoiceParameterValidator>();
            MethodInfo methodInfo = typeof(TestParameterClass).GetMethod(methodName);

            //act
            var result = validator.ValidateMultipleChoiceParameter(methodInfo);

            //assert
            Assert.Equal(expectedResult, result);
        }

        private void Initialize()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        class TestParameterClass
        {
            public void ValidMethod(string arg1, int arg2, ConnectorParameters[] connectors)
            {
                throw new NotImplementedException();
            }

            public void ValidMethod2(string arg1, int arg2)
            {
                throw new NotImplementedException();
            }

            public void ValidMethod3()
            {
                throw new NotImplementedException();
            }

            public void InvalidMethod(string arg1, List<ConnectorParameters> connectors, int arg2)
            {
                throw new NotImplementedException();
            }
        }
    }
}

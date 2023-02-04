using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace TelerikLogicBuilder.Tests.Configuration.Initialization
{
    public class FunctionListMatcherTest
    {
        public FunctionListMatcherTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateFunctionListMatcher()
        {
            //arrange
            IFunctionListMatcher service = serviceProvider.GetRequiredService<IFunctionListMatcher>();

            //assert
            Assert.NotNull(service);
        }

        [Theory]
        [InlineData("ClearErrorMessages", false)]
        [InlineData("DisplayString", false)]
        [InlineData("IsTrue", true)]
        public void IsBoolFunctionReturnsTheExpectedResults(string functionName, bool expectedResult)
        {
            //arrange
            IDictionary<string, Function> functions = GetFunctions();
            IFunctionListMatcher service = serviceProvider.GetRequiredService<IFunctionListMatcher>();

            //act
            var result = service.IsBoolFunction(functions[functionName]);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("ClearErrorMessages", false)]
        [InlineData("DisplayString", true)]
        [InlineData("IsTrue", false)]
        public void IsDialogFunctionReturnsTheExpectedResults(string functionName, bool expectedResult)
        {
            //arrange
            IDictionary<string, Function> functions = GetFunctions();
            IFunctionListMatcher service = serviceProvider.GetRequiredService<IFunctionListMatcher>();

            //act
            var result = service.IsDialogFunction(functions[functionName]);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("ClearErrorMessages", true)]
        [InlineData("DisplayString", false)]
        [InlineData("IsTrue", false)]
        public void IsTableFunctionReturnsTheExpectedResults(string functionName, bool expectedResult)
        {
            //arrange
            IDictionary<string, Function> functions = GetFunctions();
            IFunctionListMatcher service = serviceProvider.GetRequiredService<IFunctionListMatcher>();

            //act
            var result = service.IsTableFunction(functions[functionName]);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("ClearErrorMessages", false)]
        [InlineData("DisplayString", false)]
        [InlineData("IsTrue", true)]
        public void IsValueFunctionReturnsTheExpectedResults(string functionName, bool expectedResult)
        {
            //arrange
            IDictionary<string, Function> functions = GetFunctions();
            IFunctionListMatcher service = serviceProvider.GetRequiredService<IFunctionListMatcher>();

            //act
            var result = service.IsValueFunction(functions[functionName]);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("ClearErrorMessages", true)]
        [InlineData("DisplayString", false)]
        [InlineData("IsTrue", false)]
        public void IsVoidFunctionReturnsTheExpectedResults(string functionName, bool expectedResult)
        {
            //arrange
            IDictionary<string, Function> functions = GetFunctions();
            IFunctionListMatcher service = serviceProvider.GetRequiredService<IFunctionListMatcher>();

            //act
            var result = service.IsVoidFunction(functions[functionName]);

            //assert
            Assert.Equal(expectedResult, result);
        }

        private IDictionary<string, Function> GetFunctions()
        {
            IFunctionFactory functionFactory = serviceProvider.GetRequiredService<IFunctionFactory>();
            IReturnTypeFactory returnTypeFactory = serviceProvider.GetRequiredService<IReturnTypeFactory>();
            return new Dictionary<string, Function>
            {
                ["ClearErrorMessages"] = functionFactory.GetFunction
                (
                    "ClearErrorMessages",
                    "Clear",
                    FunctionCategories.Standard,
                    "",
                    "flowManager.FlowDataCache.Response.ErrorMessages",
                    "Field.Property.Property.Property",
                    "",
                    ReferenceCategories.InstanceReference,
                    ParametersLayout.Sequential,
                    new List<ParameterBase>()
                    {
                    },
                    new List<string> { },
                    returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                    ""
                ),
                ["DisplayString"] = functionFactory.GetFunction
                (
                    "DisplayString",
                    "DisplayString",
                    FunctionCategories.DialogForm,
                    "",
                    "flowManager.CustomDialogs",
                    "Field.Property",
                    "",
                    ReferenceCategories.InstanceReference,
                    ParametersLayout.Sequential,
                    new List<ParameterBase>()
                    {
                    },
                    new List<string> { },
                    returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Void),
                    ""
                ),
                ["IsTrue"] = functionFactory.GetFunction
                (
                    "IsTrue",
                    "IsTrue",
                    FunctionCategories.Standard,
                    "",
                    "flowManager.FlowDataCache.Response.ErrorMessages",
                    "Field.Property.Property.Property",
                    "",
                    ReferenceCategories.InstanceReference,
                    ParametersLayout.Sequential,
                    new List<ParameterBase>()
                    {
                    },
                    new List<string> { },
                    returnTypeFactory.GetLiteralReturnType(LiteralFunctionReturnType.Boolean),
                    ""
                ),
            };
        }
    }
}

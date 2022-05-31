using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using LogicBuilder.Attributes;
using LogicBuilder.Forms.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Functions
{
    public class FunctionHelperTest
    {
        public FunctionHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateFunctionHelper()
        {
            //arrange
            IFunctionHelper helper = serviceProvider.GetRequiredService<IFunctionHelper>();

            //assert
            Assert.NotNull(helper);
        }

        [Theory]
        [InlineData("LiteralMethod", false)]
        [InlineData("BooleanMethod", true)]
        [InlineData("BooleanMethodNullable", false)]
        [InlineData("DisplayString", false)]
        [InlineData("DoDisplay", false)]
        [InlineData("ObjectMethod", false)]
        [InlineData("GenericMethod", false)]
        [InlineData("LiteralListMethod", false)]
        [InlineData("ObjectListMethod", false)]
        [InlineData("GenericListMethod", false)]
        public void IsBooleanWorks(string methodName, bool expectedResult)
        {
            //arrange
            IFunctionManager functionManager = serviceProvider.GetRequiredService<IFunctionManager>();
            MethodInfo methodInfo = typeof(TestMethodClass<>).GetMethod(methodName)!;
            IFunctionHelper helper = serviceProvider.GetRequiredService<IFunctionHelper>();
            Function? function = functionManager.GetFunction(methodInfo.Name, methodInfo.Name, string.Empty, string.Empty, string.Empty, string.Empty, ReferenceCategories.This, ParametersLayout.Sequential, methodInfo);

            //act
            var result = helper.IsBoolean(function!);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("LiteralMethod", false)]
        [InlineData("BooleanMethod", false)]
        [InlineData("BooleanMethodNullable", false)]
        [InlineData("DisplayString", true)]
        [InlineData("DoDisplay", true)]
        [InlineData("ObjectMethod", false)]
        [InlineData("GenericMethod", false)]
        [InlineData("LiteralListMethod", false)]
        [InlineData("ObjectListMethod", false)]
        [InlineData("GenericListMethod", false)]
        public void IsDialogWorks(string methodName, bool expectedResult)
        {
            //arrange
            IFunctionManager functionManager = serviceProvider.GetRequiredService<IFunctionManager>();
            MethodInfo methodInfo = typeof(TestMethodClass<>).GetMethod(methodName)!;
            IFunctionHelper helper = serviceProvider.GetRequiredService<IFunctionHelper>();
            Function? function = functionManager.GetFunction(methodInfo.Name, methodInfo.Name, string.Empty, string.Empty, string.Empty, string.Empty, ReferenceCategories.This, ParametersLayout.Sequential, methodInfo);

            //act
            var result = helper.IsDialog(function!);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("LiteralMethod", true)]
        [InlineData("BooleanMethod", false)]
        [InlineData("BooleanMethodNullable", false)]
        [InlineData("DisplayString", true)]
        [InlineData("DoDisplay", true)]
        [InlineData("ObjectMethod", false)]
        [InlineData("GenericMethod", false)]
        [InlineData("LiteralListMethod", false)]
        [InlineData("ObjectListMethod", false)]
        [InlineData("GenericListMethod", false)]
        public void IsVoidWorks(string methodName, bool expectedResult)
        {
            //arrange
            IFunctionManager functionManager = serviceProvider.GetRequiredService<IFunctionManager>();
            MethodInfo methodInfo = typeof(TestMethodClass<>).GetMethod(methodName)!;
            IFunctionHelper helper = serviceProvider.GetRequiredService<IFunctionHelper>();
            Function? function = functionManager.GetFunction(methodInfo.Name, methodInfo.Name, string.Empty, string.Empty, string.Empty, string.Empty, ReferenceCategories.This, ParametersLayout.Sequential, methodInfo);

            //act
            var result = helper.IsVoid(function!);

            Assert.Equal(expectedResult, result);
        }

        class TestMethodClass<T>
        {
            public void LiteralMethod(string parameter1, object parameter2)
            {
                throw new NotImplementedException();
            }

            public bool BooleanMethod(string parameter1, object parameter2)
            {
                throw new NotImplementedException();
            }

            public bool? BooleanMethodNullable(string parameter1, object parameter2)
            {
                throw new NotImplementedException();
            }

            [FunctionGroup(FunctionGroup.DialogForm)]
            public void DisplayString(string setting, [ListEditorControl(ListControlType.Connectors)] ICollection<ConnectorParameters> buttons)
            {
                throw new NotImplementedException();
            }

            public void DoDisplay([ListEditorControl(ListControlType.Connectors)] ICollection<ConnectorParameters> buttons)
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

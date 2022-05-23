using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace TelerikLogicBuilder.Tests.RulesGenerator.RuleBuilders
{
    public class LongStringManagerTest
    {
        public LongStringManagerTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateLongStringManager()
        {
            //arrange
            ILongStringManager helper = serviceProvider.GetRequiredService<ILongStringManager>();

            //assert
            Assert.NotNull(helper);
        }

        [Fact]
        public void GetLongStringForBinaryWorks()
        {
            //arrange
            ILongStringManager helper = serviceProvider.GetRequiredService<ILongStringManager>();

            //act
            var result = helper.GetLongStringForBinary("A\\r\\nB\\nC&#123;D&#125;E&#92;F", RuntimeType.NetCore);

            //assert
            Assert.Equal
            (
                $"A{Environment.NewLine}B{Environment.NewLine}C{{D}}E\\F", 
                result
            );
        }

        [Fact]
        public void GetLongStringForTextWorks()
        {
            //arrange
            ILongStringManager helper = serviceProvider.GetRequiredService<ILongStringManager>();

            //act
            var result = helper.GetLongStringForText("A\\r\\nB\\nC&#123;D&#125;E&#92;F", RuntimeType.NetCore);

            //assert
            Assert.Equal
            (
                $"A\\r\\nB\\r\\nC{{D}}E\\F",
                result
            );
        }

        [Theory]
        [InlineData
        (
            "System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
            RuntimeType.NetCore,
            "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
        )]
        [InlineData
        (
            "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", 
            RuntimeType.Xamarin, 
            "mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
        )]
        [InlineData
        (
            "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
            RuntimeType.NetFramework, 
            "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        )]
        [InlineData
        (
            "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", 
            RuntimeType.NetNative, 
            "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        )]
        [InlineData
        (
            "System.CodeDom, Version=5.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51",
            RuntimeType.NetCore,
            "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"
        )]
        [InlineData
        (
            "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51",
            RuntimeType.Xamarin,
            "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"
        )]
        [InlineData
        (
            "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51",
            RuntimeType.NetFramework,
            "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        )]
        [InlineData
        (
            "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51",
            RuntimeType.NetNative,
            "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"
        )]
        [InlineData
        (
            "System.Linq.Expressions, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            RuntimeType.NetCore,
            "System.Linq.Expressions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        )]
        [InlineData
        (
            "System.Linq.Expressions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            RuntimeType.Xamarin,
            "System.Core, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
        )]
        [InlineData
        (
            "System.Linq.Expressions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            RuntimeType.NetFramework,
            "System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        )]
        [InlineData
        (
            "System.Linq.Expressions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            RuntimeType.NetNative,
            "System.Linq.Expressions, Version=4.2.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        )]
        internal void UpdateStrongNameByPlatFormWorks(string initial, RuntimeType runtimeType, string expectedResult)
        {
            //arrange
            ILongStringManager helper = serviceProvider.GetRequiredService<ILongStringManager>();

            //act
            var result = helper.UpdateStrongNameByPlatForm(initial, runtimeType);

            //assert
            Assert.Equal
            (
                expectedResult,
                result
            );
        }

        [Theory]
        [InlineData
        (
            "mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
            "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
        )]
        [InlineData
        (
            "mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
            "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
        )]
        [InlineData
        (
            "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            "System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e"
        )]
        [InlineData
        (
            "System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
            "System.CodeDom, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"
        )]
        [InlineData
        (
            "System.Core, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
            "System.Linq.Expressions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        )]
        [InlineData
        (
            "System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
            "System.Linq.Expressions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        )]
        [InlineData
        (
            "System.Linq.Expressions, Version=4.2.1.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            "System.Linq.Expressions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
        )]
        internal void UpdateStrongNameToNetCoreWorks(string initial, string expectedResult)
        {
            //arrange
            ILongStringManager helper = serviceProvider.GetRequiredService<ILongStringManager>();

            //act
            var result = helper.UpdateStrongNameToNetCore(initial);

            //assert
            Assert.Equal
            (
                expectedResult,
                result
            );
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace TelerikLogicBuilder.Tests.Intellisense.Parameters
{
    public class ParameterHelperTest
    {
        static ParameterHelperTest()
        {
            serviceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private static readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        public void CanCreateParameterHelper()
        {
            //act
            IParameterHelper helper = serviceProvider.GetRequiredService<IParameterHelper>();

            //assert
            Assert.NotNull(helper);
        }

        public static List<object[]> Parameters_Data
        {
            get
            {
                IParameterFactory parameterFactory = serviceProvider.GetRequiredService<IParameterFactory>();
                return new List<object[]>
                {
                    new object[]
                    {
                        parameterFactory.GetLiteralParameter
                        (
                            "stringProperty",
                            false,
                            "",
                            LiteralParameterType.Any,
                            LiteralParameterInputStyle.SingleLineTextBox,
                            true,
                            false,
                            true,
                            "",
                            "",
                            "",
                            new List<string>()
                        ),
                        true
                    },
                    new object[]
                    {
                        parameterFactory.GetLiteralParameter
                        (
                            "stringProperty",
                            false,
                            "",
                            LiteralParameterType.String,
                            LiteralParameterInputStyle.SingleLineTextBox,
                            true,
                            false,
                            true,
                            "",
                            "",
                            "",
                            new List<string>()
                        ),
                        false
                    },
                    new object[]
                    {
                        parameterFactory.GetObjectParameter
                        (
                            "objectProperty",
                            false,
                            "",
                            "System.Object",
                            true,
                            false,
                            true
                        ),
                        false
                    },
                    new object[]
                    {
                        parameterFactory.GetGenericParameter
                        (
                            "aProperty",
                            false,
                            "",
                            "A"
                        ),
                        false
                    },
                    new object[]
                    {
                        parameterFactory.GetListOfLiteralsParameter
                        (
                            "charArray",
                            false,
                            "",
                            LiteralParameterType.String,
                            ListType.Array,
                            ListParameterInputStyle.HashSetForm,
                            LiteralParameterInputStyle.SingleLineTextBox,
                            "",
                            "",
                            new List<string>(),
                            new char[] { ',' },
                            new List<string>()
                        ),
                        false
                    },
                    new object[]
                    {
                        parameterFactory.GetListOfObjectsParameter
                        (
                            "objectArray",
                            false,
                            "",
                            "System.Object",
                            ListType.Array,
                            ListParameterInputStyle.HashSetForm
                        ),
                        false
                    },
                    new object[]
                    {
                        parameterFactory.GetListOfGenericsParameter
                        (
                            "objectArray",
                            false,
                            "",
                            "A",
                            ListType.Array,
                            ListParameterInputStyle.HashSetForm
                        ),
                        false
                    }
                };
            }
        }

        [Theory]
        [MemberData(nameof(Parameters_Data))]
        internal void CanConvertGenericParameterFromAllGenericConfigBaseTypes(ParameterBase parameter, bool expectedResult)
        {
            //arrange
            IParameterHelper helper = serviceProvider.GetRequiredService<IParameterHelper>();

            //act
            var result = helper.IsParameterAny(parameter);

            //assert
            Assert.Equal(expectedResult, result);
        }
    }
}

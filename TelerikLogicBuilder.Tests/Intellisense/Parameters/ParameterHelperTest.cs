using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
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
                IContextProvider contextProvider = serviceProvider.GetRequiredService<IContextProvider>();
                return new List<object[]>
                {
                    new object[]
                    {
                        new LiteralParameter
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
                            new List<string>(),
                            contextProvider
                        ),
                        true
                    },
                    new object[]
                    {
                        new LiteralParameter
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
                            new List<string>(),
                            contextProvider
                        ),
                        false
                    },
                    new object[]
                    {
                        new ObjectParameter
                        (
                            "objectProperty",
                            false,
                            "",
                            "System.Object",
                            true,
                            false,
                            true,
                           contextProvider
                        ),
                        false
                    },
                    new object[]
                    {
                        new GenericParameter
                        (
                            "aProperty",
                            false,
                            "",
                            "A",
                            contextProvider
                        ),
                        false
                    },
                    new object[]
                    {
                        new ListOfLiteralsParameter
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
                            new List<string>(),
                            contextProvider
                        ),
                        false
                    },
                    new object[]
                    {
                        new ListOfObjectsParameter
                        (
                            "objectArray",
                            false,
                            "",
                            "System.Object",
                            ListType.Array,
                            ListParameterInputStyle.HashSetForm,
                            contextProvider
                        ),
                        false
                    },
                    new object[]
                    {
                        new ListOfGenericsParameter
                        (
                            "objectArray",
                            false,
                            "",
                            "A",
                            ListType.Array,
                            ListParameterInputStyle.HashSetForm,
                            contextProvider
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

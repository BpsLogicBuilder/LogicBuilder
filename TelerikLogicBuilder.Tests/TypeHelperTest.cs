using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Forms.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TelerikLogicBuilder.Tests.AttributeSamples;
using TelerikLogicBuilder.Tests.Structures;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class TypeHelperTest
    {
        public TypeHelperTest()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData(typeof(string), "String")]
        [InlineData(typeof(int?), "Nullable`1[Int32]")]
        [InlineData(typeof(List<string>), "List`1[String]")]
        [InlineData(typeof(List<int?>), "List`1[Nullable`1[Int32]]")]
        public void GetTypeDescriptionReturnsTheExpectedDescription(Type type, string expectedDescription)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var description = helper.GetTypeDescription(type);

            //assert
            Assert.Equal(expectedDescription, description);
        }

        [Theory]
        [InlineData(typeof(string[]), typeof(string))]
        [InlineData(typeof(List<int?>), typeof(int?))]
        public void GetUndelyingTypeForValidListReturnsTheExpectedDescription(Type type, Type expectedType)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.GetUndelyingTypeForValidList(type);

            //assert
            Assert.Equal(expectedType, result);
        }

        [Fact]
        public void GetUndelyingTypeForValidListThrowaCriticalExceptionorInvalidType()
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => helper.GetUndelyingTypeForValidList(typeof(string)));
        }

        [Theory]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(List<int?>), false)]
        public void IsLiteralTypeReturnsTheExpectedBoolean(Type type, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.IsLiteralType(type);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(string), true)]
        [InlineData(typeof(int?), true)]
        [InlineData(typeof(void), true)]
        [InlineData(typeof(List<int?>), false)]
        public void IsValidLiteralReturnTypeReturnsTheExpectedBoolean(Type type, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.IsValidLiteralReturnType(type);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(int?), true)]
        public void IsNullableReturnsTheExpectedBoolean(Type type, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.IsNullable(type);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(List<int?>), true)]
        [InlineData(typeof(IList<int?>), true)]
        [InlineData(typeof(Collection<int?>), true)]
        [InlineData(typeof(ICollection<int?>), true)]
        [InlineData(typeof(IEnumerable<int?>), true)]
        [InlineData(typeof(string[]), true)]
        public void IsValidListReturnsTheExpectedBoolean(Type type, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.IsValidList(type);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(string), false)]
        [InlineData(typeof(List<ConnectorParameters>), true)]
        [InlineData(typeof(IList<ConnectorParameters>), true)]
        [InlineData(typeof(Collection<ConnectorParameters>), true)]
        [InlineData(typeof(ICollection<ConnectorParameters>), true)]
        [InlineData(typeof(IEnumerable<ConnectorParameters>), true)]
        [InlineData(typeof(ConnectorParameters[]), true)]
        public void IsValidConnectorListReturnsTheExpectedBoolean(Type type, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.IsValidConnectorList(type);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(string), "System.String")]
        [InlineData(typeof(InstructorModel), "TelerikLogicBuilder.Tests.AttributeSamples.InstructorModel")]
        public void ToIdReturnsTheExpectedDescriptionForNonGenerics(Type type, string expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.ToId(type);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(int?), "System.Nullable`1[[System.Int32")]
        [InlineData(typeof(List<string>), "System.Collections.Generic.List`1[[System.String")]
        [InlineData(typeof(List<int?>), "System.Collections.Generic.List`1[[System.Nullable`1[[System.Int32")]
        public void ToIdReturnsTheExpectedDescriptionGenerics(Type type, string expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.ToId(type);

            //assert
            Assert.StartsWith(expectedResult, result);
        }

        [Theory]
        [InlineData("1", typeof(int?), 1, true)]
        [InlineData("20.12", typeof(double), 20.12, true)]
        public void TryParseReturnsExpectedResult(string toParse, Type type, object expectedParsedResult, bool expectedSuccess)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.TryParse(toParse, type, out object? parsedResult);

            //assert
            Assert.Equal(expectedSuccess, result);
            Assert.Equal(expectedParsedResult, parsedResult);
        }

        [Theory]
        [InlineData("1", typeof(List<int?>))]
        [InlineData("20.12", typeof(object))]
        public void TryParseThrowsExceptionForInvalidType(string toParse, Type type)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => helper.TryParse(toParse, type, out object? parsedResult));
        }

        [Theory]
        [InlineData(typeof(object), typeof(int), true)]
        [InlineData(typeof(int), typeof(byte), true)]
        [InlineData(typeof(int), typeof(sbyte), true)]
        [InlineData(typeof(int), typeof(char), true)]
        [InlineData(typeof(int), typeof(ushort), true)]
        [InlineData(typeof(int), typeof(short), true)]
        [InlineData(typeof(int), typeof(int), true)]
        [InlineData(typeof(int), typeof(DateTime), false)]
        [InlineData(typeof(int), typeof(long), false)]
        [InlineData(typeof(int), typeof(int?), false)]
        [InlineData(typeof(int), typeof(decimal), false)]
        [InlineData(typeof(int), typeof(float), false)]
        [InlineData(typeof(int), typeof(short?), false)]
        public void AssignableTestsWorkAsExpected(Type to, Type from, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.AssignableFrom(to, from);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(int?), typeof(int), true)]
        [InlineData(typeof(int), typeof(int?), true)]
        [InlineData(typeof(int), typeof(short?), true)]
        [InlineData(typeof(short), typeof(sbyte), true)]
        [InlineData(typeof(sbyte), typeof(short), true)]
        [InlineData(typeof(int), typeof(short), true)]
        [InlineData(typeof(short), typeof(int), true)]
        [InlineData(typeof(int?), typeof(short), true)]
        [InlineData(typeof(short), typeof(int?), true)]
        [InlineData(typeof(int?), typeof(short?), true)]
        [InlineData(typeof(short?), typeof(int?), true)]
        [InlineData(typeof(CustString), typeof(string), false)]
        [InlineData(typeof(string), typeof(CustString), false)]
        [InlineData(typeof(DoubleDigit), typeof(double), false)]
        [InlineData(typeof(double), typeof(DoubleDigit), false)]
        [InlineData(typeof(IList<string>), typeof(List<string>), false)]
        [InlineData(typeof(List<string>), typeof(IList<string>), false)]
        [InlineData(typeof(CustString?), typeof(CustString), false)]
        [InlineData(typeof(IntDigit?), typeof(short), true)]
        [InlineData(typeof(IntDigit?), typeof(long), true)]
        [InlineData(typeof(IntDigit?), typeof(long?), true)]
        [InlineData(typeof(int), typeof(string), false)]
        [InlineData(typeof(object), typeof(string), false)]
        [InlineData(typeof(DateTime), typeof(DateTime?), false)]
        [InlineData(typeof(bool), typeof(bool), true)]
        public void CompatibleForBitwiseAndOp(Type t1, Type t2, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.AreCompatibleForOperation(t1, t2, CodeBinaryOperatorType.BitwiseAnd);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(int?), typeof(int), false)]
        [InlineData(typeof(int), typeof(int?), false)]
        [InlineData(typeof(int), typeof(short?), false)]
        [InlineData(typeof(short), typeof(sbyte), false)]
        [InlineData(typeof(sbyte), typeof(short), false)]
        [InlineData(typeof(int), typeof(short), false)]
        [InlineData(typeof(short), typeof(int), false)]
        [InlineData(typeof(int?), typeof(short), false)]
        [InlineData(typeof(short), typeof(int?), false)]
        [InlineData(typeof(int?), typeof(short?), false)]
        [InlineData(typeof(short?), typeof(int?), false)]
        [InlineData(typeof(CustString), typeof(string), false)]
        [InlineData(typeof(string), typeof(CustString), false)]
        [InlineData(typeof(DoubleDigit), typeof(double), false)]
        [InlineData(typeof(double), typeof(DoubleDigit), false)]
        [InlineData(typeof(IList<string>), typeof(List<string>), false)]
        [InlineData(typeof(List<string>), typeof(IList<string>), false)]
        [InlineData(typeof(CustString?), typeof(CustString), false)]
        [InlineData(typeof(IntDigit?), typeof(short), false)]
        [InlineData(typeof(IntDigit?), typeof(long), false)]
        [InlineData(typeof(IntDigit?), typeof(long?), false)]
        [InlineData(typeof(int), typeof(string), false)]
        [InlineData(typeof(object), typeof(string), false)]
        [InlineData(typeof(DateTime), typeof(DateTime?), false)]
        [InlineData(typeof(bool), typeof(bool), true)]
        public void CompatibleForBooleanAndOp(Type t1, Type t2, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.AreCompatibleForOperation(t1, t2, CodeBinaryOperatorType.BooleanAnd);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(object), typeof(int), true)]
        [InlineData(typeof(int), typeof(byte), true)]
        [InlineData(typeof(int), typeof(sbyte), true)]
        [InlineData(typeof(int), typeof(char), true)]
        [InlineData(typeof(int), typeof(ushort), true)]
        [InlineData(typeof(int), typeof(short), true)]
        [InlineData(typeof(int), typeof(int), true)]
        [InlineData(typeof(int), typeof(DateTime), false)]
        [InlineData(typeof(int), typeof(long), true)]
        [InlineData(typeof(int), typeof(int?), true)]
        [InlineData(typeof(int), typeof(decimal), true)]
        [InlineData(typeof(int), typeof(float), true)]
        [InlineData(typeof(int), typeof(short?), true)]
        [InlineData(typeof(IList<string>), typeof(List<string>), true)]
        [InlineData(typeof(List<string>), typeof(IList<string>), true)]
        [InlineData(typeof(object), typeof(string), true)]
        public void CompatibleForIdentityEqualityOp(Type t1, Type t2, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.AreCompatibleForOperation(t1, t2, CodeBinaryOperatorType.IdentityEquality);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(object), typeof(int), true)]
        [InlineData(typeof(int), typeof(byte), true)]
        [InlineData(typeof(int), typeof(sbyte), true)]
        [InlineData(typeof(int), typeof(char), true)]
        [InlineData(typeof(int), typeof(ushort), true)]
        [InlineData(typeof(int), typeof(short), true)]
        [InlineData(typeof(int), typeof(int), true)]
        [InlineData(typeof(int), typeof(DateTime), false)]
        [InlineData(typeof(DoubleDigit), typeof(double), true)]
        [InlineData(typeof(double), typeof(DoubleDigit), true)]
        [InlineData(typeof(DoubleDigit), typeof(short?), true)]
        [InlineData(typeof(short?), typeof(DoubleDigit), true)]
        [InlineData(typeof(int), typeof(long), true)]
        [InlineData(typeof(int), typeof(int?), true)]
        [InlineData(typeof(int), typeof(decimal), true)]
        [InlineData(typeof(int), typeof(float), true)]
        [InlineData(typeof(int), typeof(short?), true)]
        [InlineData(typeof(IList<string>), typeof(List<string>), true)]
        [InlineData(typeof(List<string>), typeof(IList<string>), true)]
        [InlineData(typeof(object), typeof(string), true)]
        [InlineData(typeof(CustString), typeof(string), true)]
        [InlineData(typeof(string), typeof(CustString), true)]
        [InlineData(typeof(CustString), typeof(short), false)]
        [InlineData(typeof(short), typeof(CustString), false)]
        public void CompatibleForIdentityInequalityOp(Type t1, Type t2, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.AreCompatibleForOperation(t1, t2, CodeBinaryOperatorType.IdentityInequality);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(byte), typeof(byte), true)]
        [InlineData(typeof(string), typeof(string), false)]
        [InlineData(typeof(int?), typeof(int), true)]
        [InlineData(typeof(int), typeof(int?), true)]
        [InlineData(typeof(int), typeof(short?), true)]
        [InlineData(typeof(short), typeof(sbyte), true)]
        [InlineData(typeof(sbyte), typeof(short), true)]
        [InlineData(typeof(int), typeof(short), true)]
        [InlineData(typeof(short), typeof(int), true)]
        [InlineData(typeof(int?), typeof(short), true)]
        [InlineData(typeof(short), typeof(int?), true)]
        [InlineData(typeof(int?), typeof(short?), true)]
        [InlineData(typeof(short?), typeof(int?), true)]
        [InlineData(typeof(CustString), typeof(string), true)]
        [InlineData(typeof(string), typeof(CustString), true)]
        [InlineData(typeof(DoubleDigit), typeof(double), true)]
        [InlineData(typeof(double), typeof(DoubleDigit), true)]
        [InlineData(typeof(DoubleDigit), typeof(short?), true)]
        [InlineData(typeof(short?), typeof(DoubleDigit), true)]
        [InlineData(typeof(IList<string>), typeof(List<string>), false)]
        [InlineData(typeof(List<string>), typeof(IList<string>), false)]
        [InlineData(typeof(CustString?), typeof(CustString), true)]
        [InlineData(typeof(IntDigit?), typeof(short), true)]
        [InlineData(typeof(IntDigit), typeof(long), true)]
        [InlineData(typeof(IntDigit), typeof(long?), true)]
        [InlineData(typeof(int), typeof(string), false)]
        [InlineData(typeof(object), typeof(string), false)]
        [InlineData(typeof(DateTime), typeof(DateTime?), true)]
        [InlineData(typeof(bool), typeof(bool), false)]
        public void CompatibleForLessThanOp(Type t1, Type t2, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.AreCompatibleForOperation(t1, t2, CodeBinaryOperatorType.LessThan);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(byte), typeof(byte), true)]
        [InlineData(typeof(string), typeof(string), true)]
        [InlineData(typeof(int?), typeof(int), true)]
        [InlineData(typeof(int), typeof(int?), true)]
        [InlineData(typeof(int), typeof(short?), true)]
        [InlineData(typeof(short), typeof(sbyte), true)]
        [InlineData(typeof(sbyte), typeof(short), true)]
        [InlineData(typeof(int), typeof(short), true)]
        [InlineData(typeof(short), typeof(int), true)]
        [InlineData(typeof(int?), typeof(short), true)]
        [InlineData(typeof(short), typeof(int?), true)]
        [InlineData(typeof(int?), typeof(short?), true)]
        [InlineData(typeof(short?), typeof(int?), true)]
        [InlineData(typeof(CustString), typeof(string), true)]
        [InlineData(typeof(string), typeof(CustString), true)]
        [InlineData(typeof(DoubleDigit), typeof(double), true)]
        [InlineData(typeof(double), typeof(DoubleDigit), true)]
        [InlineData(typeof(DoubleDigit), typeof(short?), true)]
        [InlineData(typeof(short?), typeof(DoubleDigit), true)]
        [InlineData(typeof(IList<string>), typeof(List<string>), false)]
        [InlineData(typeof(List<string>), typeof(IList<string>), false)]
        [InlineData(typeof(CustString?), typeof(CustString), true)]
        [InlineData(typeof(IntDigit?), typeof(short), true)]
        [InlineData(typeof(IntDigit), typeof(long), true)]
        [InlineData(typeof(IntDigit), typeof(long?), true)]
        [InlineData(typeof(int), typeof(string), false)]
        [InlineData(typeof(object), typeof(string), false)]
        [InlineData(typeof(DateTime), typeof(DateTime?), false)]
        [InlineData(typeof(bool), typeof(bool), false)]
        public void CompatibleForAddOp(Type t1, Type t2, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.AreCompatibleForOperation(t1, t2, CodeBinaryOperatorType.Add);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(typeof(int?), typeof(int), true)]
        [InlineData(typeof(int), typeof(int?), true)]
        [InlineData(typeof(int), typeof(short?), true)]
        [InlineData(typeof(short), typeof(sbyte), true)]
        [InlineData(typeof(sbyte), typeof(short), true)]
        [InlineData(typeof(int), typeof(short), true)]
        [InlineData(typeof(short), typeof(int), true)]
        [InlineData(typeof(int?), typeof(short), true)]
        [InlineData(typeof(short), typeof(int?), true)]
        [InlineData(typeof(int?), typeof(short?), true)]
        [InlineData(typeof(short?), typeof(int?), true)]
        [InlineData(typeof(CustString), typeof(string), true)]
        [InlineData(typeof(string), typeof(CustString), true)]
        [InlineData(typeof(DoubleDigit), typeof(double), true)]
        [InlineData(typeof(double), typeof(DoubleDigit), true)]
        [InlineData(typeof(CustString?), typeof(CustString), true)]
        [InlineData(typeof(IntDigit?), typeof(short), true)]
        [InlineData(typeof(IntDigit), typeof(long), true)]
        [InlineData(typeof(IntDigit), typeof(long?), true)]
        [InlineData(typeof(int), typeof(string), false)]
        [InlineData(typeof(DateTime), typeof(DateTime?), true)]
        [InlineData(typeof(bool), typeof(bool), true)]
        public void CompatibleForValueEqualityOp(Type t1, Type t2, bool expectedResult)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.AreCompatibleForOperation(t1, t2, CodeBinaryOperatorType.ValueEquality);

            //assert
            Assert.Equal(expectedResult, result);
        }
    }
}

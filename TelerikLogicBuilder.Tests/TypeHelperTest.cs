using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using LogicBuilder.Forms.Parameters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TelerikLogicBuilder.Tests.AttributeSamples;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class TypeHelperTest
    {
        public TypeHelperTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData(typeof(string), "String")]
        [InlineData(typeof(int?), "Nullable`1[Int32]")]
        [InlineData(typeof(List<string>), "List`1[String]")]
        [InlineData(typeof(List<int?>), "List`1[Nullable`1[Int32]]")]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
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
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void TryParseReturnsExpectedResult(string toParse, Type type, object expectedParsedResult, bool expectedSuccess)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            var result = helper.TryParse(toParse, type, out object parsedResult);

            //assert
            Assert.Equal(expectedSuccess, result);
            Assert.Equal(expectedParsedResult, parsedResult);
        }

        [Theory]
        [InlineData("1", typeof(List<int?>))]
        [InlineData("20.12", typeof(object))]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void TryParseThrowsExceptionForInvalidType(string toParse, Type type)
        {
            //arrange
            ITypeHelper helper = serviceProvider.GetRequiredService<ITypeHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => helper.TryParse(toParse, type, out object parsedResult));
        }

        private void Initialize()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }
    }
}

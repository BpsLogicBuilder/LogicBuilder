using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TelerikLogicBuilder.Tests.Constants;
using Xunit;

namespace TelerikLogicBuilder.Tests
{
    public class EnumHelperTest
    {
        public EnumHelperTest()
        {
            Initialize();
        }

        #region Fields
        private IServiceProvider serviceProvider;
        #endregion Fields

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetVisibleEnumTextReturnsCorrectStringForDefaultCulture()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var visibleText = enumHelper.GetVisibleEnumText(ParameterType.String);

            //assert
            Assert.Equal("String", visibleText);
        }

        [Fact]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        public void GetVisibleEnumTextReturnsCorrectStringForSatelliteCulture()
        {
            //arrange
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var visibleText = enumHelper.GetVisibleEnumText(ParameterType.String);

            //assert
            Assert.Equal("StringFR", visibleText);
        }

        [Theory]
        [InlineData(typeof(List<string>), ListType.GenericList)]
        [InlineData(typeof(IList<string>), ListType.IGenericList)]
        [InlineData(typeof(Collection<string>), ListType.GenericCollection)]
        [InlineData(typeof(ICollection<string>), ListType.IGenericCollection)]
        [InlineData(typeof(IEnumerable<string>), ListType.IGenericEnumerable)]
        [InlineData(typeof(string[]), ListType.Array)]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetListTypeReturnsTheExpectedEnumType(Type type, ListType expectedListType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var listType = enumHelper.GetListType(type);

            //assert
            Assert.Equal(expectedListType, listType);
        }

        [Fact]
        public void GetListTypeThrowsCriticalExceptionForInvalidType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetListType(typeof(string)));
        }

        [Theory]
        [InlineData(typeof(bool), ParameterType.Boolean)]
        [InlineData(typeof(DateTimeOffset), ParameterType.DateTimeOffset)]
        [InlineData(typeof(DateTime), ParameterType.DateTime)]
        [InlineData(typeof(Date), ParameterType.Date)]
        [InlineData(typeof(TimeSpan), ParameterType.TimeSpan)]
        [InlineData(typeof(TimeOfDay), ParameterType.TimeOfDay)]
        [InlineData(typeof(Guid), ParameterType.Guid)]
        [InlineData(typeof(decimal), ParameterType.Decimal)]
        [InlineData(typeof(byte), ParameterType.Byte)]
        [InlineData(typeof(short), ParameterType.Short)]
        [InlineData(typeof(int), ParameterType.Integer)]
        [InlineData(typeof(long), ParameterType.Long)]
        [InlineData(typeof(float), ParameterType.Float)]
        [InlineData(typeof(double), ParameterType.Double)]
        [InlineData(typeof(char), ParameterType.Char)]
        [InlineData(typeof(sbyte), ParameterType.SByte)]
        [InlineData(typeof(ushort), ParameterType.UShort)]
        [InlineData(typeof(uint), ParameterType.UInteger)]
        [InlineData(typeof(ulong), ParameterType.ULong)]
        [InlineData(typeof(string), ParameterType.String)]
        [InlineData(typeof(bool?), ParameterType.NullableBoolean)]
        [InlineData(typeof(DateTimeOffset?), ParameterType.NullableDateTimeOffset)]
        [InlineData(typeof(DateTime?), ParameterType.NullableDateTime)]
        [InlineData(typeof(Date?), ParameterType.NullableDate)]
        [InlineData(typeof(TimeSpan?), ParameterType.NullableTimeSpan)]
        [InlineData(typeof(TimeOfDay?), ParameterType.NullableTimeOfDay)]
        [InlineData(typeof(Guid?), ParameterType.NullableGuid)]
        [InlineData(typeof(decimal?), ParameterType.NullableDecimal)]
        [InlineData(typeof(byte?), ParameterType.NullableByte)]
        [InlineData(typeof(short?), ParameterType.NullableShort)]
        [InlineData(typeof(int?), ParameterType.NullableInteger)]
        [InlineData(typeof(long?), ParameterType.NullableLong)]
        [InlineData(typeof(float?), ParameterType.NullableFloat)]
        [InlineData(typeof(double?), ParameterType.NullableDouble)]
        [InlineData(typeof(char?), ParameterType.NullableChar)]
        [InlineData(typeof(sbyte?), ParameterType.NullableSByte)]
        [InlineData(typeof(ushort?), ParameterType.NullableUShort)]
        [InlineData(typeof(uint?), ParameterType.NullableUInteger)]
        [InlineData(typeof(ulong?), ParameterType.NullableULong)]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetParameterTypeReturnsTheExpectedEnumType(Type type, ParameterType expectedParameterType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var listType = enumHelper.GetParameterType(type);

            //assert
            Assert.Equal(expectedParameterType, listType);
        }

        [Theory]
        [InlineData(ListType.GenericList, "String", "Generic List Of String")]
        [InlineData(ListType.GenericCollection, "String", "Generic Collection Of String")]
        [InlineData(ListType.IGenericList, "String", "Generic List Interface Of String")]
        [InlineData(ListType.IGenericCollection, "String", "Generic Collection Interface Of String")]
        [InlineData(ListType.IGenericEnumerable, "String", "Generic Enumerable Interface Of String")]
        [InlineData(ListType.Array, "String", "Array Of String")]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetTypeDescriptionReturnsTheExpectedString(ListType listType, string elementType, string expectedResult)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var description = enumHelper.GetTypeDescription(listType, elementType);

            //assert
            Assert.Equal(expectedResult, description);
        }

        private void Initialize()
        {
            serviceProvider = new ServiceCollection()
                .AddSingleton<IExceptionHelper, ExceptionHelper>()
                .AddSingleton<IEnumHelper, ABIS.LogicBuilder.FlowBuilder.Services.EnumHelper>()
                .AddSingleton<IStringHelper, StringHelper>()
                .BuildServiceProvider();
        }
    }
}

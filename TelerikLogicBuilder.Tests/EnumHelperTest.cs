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
            var visibleText = enumHelper.GetVisibleEnumText(LiteralParameterType.String);

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
            var visibleText = enumHelper.GetVisibleEnumText(LiteralParameterType.String);

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
        [InlineData(typeof(bool), LiteralParameterType.Boolean)]
        [InlineData(typeof(DateTimeOffset), LiteralParameterType.DateTimeOffset)]
        [InlineData(typeof(DateTime), LiteralParameterType.DateTime)]
        [InlineData(typeof(Date), LiteralParameterType.Date)]
        [InlineData(typeof(TimeSpan), LiteralParameterType.TimeSpan)]
        [InlineData(typeof(TimeOfDay), LiteralParameterType.TimeOfDay)]
        [InlineData(typeof(Guid), LiteralParameterType.Guid)]
        [InlineData(typeof(decimal), LiteralParameterType.Decimal)]
        [InlineData(typeof(byte), LiteralParameterType.Byte)]
        [InlineData(typeof(short), LiteralParameterType.Short)]
        [InlineData(typeof(int), LiteralParameterType.Integer)]
        [InlineData(typeof(long), LiteralParameterType.Long)]
        [InlineData(typeof(float), LiteralParameterType.Float)]
        [InlineData(typeof(double), LiteralParameterType.Double)]
        [InlineData(typeof(char), LiteralParameterType.Char)]
        [InlineData(typeof(sbyte), LiteralParameterType.SByte)]
        [InlineData(typeof(ushort), LiteralParameterType.UShort)]
        [InlineData(typeof(uint), LiteralParameterType.UInteger)]
        [InlineData(typeof(ulong), LiteralParameterType.ULong)]
        [InlineData(typeof(string), LiteralParameterType.String)]
        [InlineData(typeof(bool?), LiteralParameterType.NullableBoolean)]
        [InlineData(typeof(DateTimeOffset?), LiteralParameterType.NullableDateTimeOffset)]
        [InlineData(typeof(DateTime?), LiteralParameterType.NullableDateTime)]
        [InlineData(typeof(Date?), LiteralParameterType.NullableDate)]
        [InlineData(typeof(TimeSpan?), LiteralParameterType.NullableTimeSpan)]
        [InlineData(typeof(TimeOfDay?), LiteralParameterType.NullableTimeOfDay)]
        [InlineData(typeof(Guid?), LiteralParameterType.NullableGuid)]
        [InlineData(typeof(decimal?), LiteralParameterType.NullableDecimal)]
        [InlineData(typeof(byte?), LiteralParameterType.NullableByte)]
        [InlineData(typeof(short?), LiteralParameterType.NullableShort)]
        [InlineData(typeof(int?), LiteralParameterType.NullableInteger)]
        [InlineData(typeof(long?), LiteralParameterType.NullableLong)]
        [InlineData(typeof(float?), LiteralParameterType.NullableFloat)]
        [InlineData(typeof(double?), LiteralParameterType.NullableDouble)]
        [InlineData(typeof(char?), LiteralParameterType.NullableChar)]
        [InlineData(typeof(sbyte?), LiteralParameterType.NullableSByte)]
        [InlineData(typeof(ushort?), LiteralParameterType.NullableUShort)]
        [InlineData(typeof(uint?), LiteralParameterType.NullableUInteger)]
        [InlineData(typeof(ulong?), LiteralParameterType.NullableULong)]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetParameterTypeReturnsTheExpectedEnumType(Type type, LiteralParameterType expectedParameterType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var listType = enumHelper.GetLiteralParameterType(type);

            //assert
            Assert.Equal(expectedParameterType, listType);
        }

        [Fact]
        public void GetParameterTypeThrowsCriticalExceptionForInvalidType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetLiteralParameterType(typeof(void)));
        }

        [Theory]
        [InlineData(typeof(bool), LiteralVariableType.Boolean)]
        [InlineData(typeof(DateTimeOffset), LiteralVariableType.DateTimeOffset)]
        [InlineData(typeof(DateTime), LiteralVariableType.DateTime)]
        [InlineData(typeof(Date), LiteralVariableType.Date)]
        [InlineData(typeof(TimeSpan), LiteralVariableType.TimeSpan)]
        [InlineData(typeof(TimeOfDay), LiteralVariableType.TimeOfDay)]
        [InlineData(typeof(Guid), LiteralVariableType.Guid)]
        [InlineData(typeof(decimal), LiteralVariableType.Decimal)]
        [InlineData(typeof(byte), LiteralVariableType.Byte)]
        [InlineData(typeof(short), LiteralVariableType.Short)]
        [InlineData(typeof(int), LiteralVariableType.Integer)]
        [InlineData(typeof(long), LiteralVariableType.Long)]
        [InlineData(typeof(float), LiteralVariableType.Float)]
        [InlineData(typeof(double), LiteralVariableType.Double)]
        [InlineData(typeof(char), LiteralVariableType.Char)]
        [InlineData(typeof(sbyte), LiteralVariableType.SByte)]
        [InlineData(typeof(ushort), LiteralVariableType.UShort)]
        [InlineData(typeof(uint), LiteralVariableType.UInteger)]
        [InlineData(typeof(ulong), LiteralVariableType.ULong)]
        [InlineData(typeof(string), LiteralVariableType.String)]
        [InlineData(typeof(bool?), LiteralVariableType.NullableBoolean)]
        [InlineData(typeof(DateTimeOffset?), LiteralVariableType.NullableDateTimeOffset)]
        [InlineData(typeof(DateTime?), LiteralVariableType.NullableDateTime)]
        [InlineData(typeof(Date?), LiteralVariableType.NullableDate)]
        [InlineData(typeof(TimeSpan?), LiteralVariableType.NullableTimeSpan)]
        [InlineData(typeof(TimeOfDay?), LiteralVariableType.NullableTimeOfDay)]
        [InlineData(typeof(Guid?), LiteralVariableType.NullableGuid)]
        [InlineData(typeof(decimal?), LiteralVariableType.NullableDecimal)]
        [InlineData(typeof(byte?), LiteralVariableType.NullableByte)]
        [InlineData(typeof(short?), LiteralVariableType.NullableShort)]
        [InlineData(typeof(int?), LiteralVariableType.NullableInteger)]
        [InlineData(typeof(long?), LiteralVariableType.NullableLong)]
        [InlineData(typeof(float?), LiteralVariableType.NullableFloat)]
        [InlineData(typeof(double?), LiteralVariableType.NullableDouble)]
        [InlineData(typeof(char?), LiteralVariableType.NullableChar)]
        [InlineData(typeof(sbyte?), LiteralVariableType.NullableSByte)]
        [InlineData(typeof(ushort?), LiteralVariableType.NullableUShort)]
        [InlineData(typeof(uint?), LiteralVariableType.NullableUInteger)]
        [InlineData(typeof(ulong?), LiteralVariableType.NullableULong)]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetVariableTypeReturnsTheExpectedEnumType(Type type, LiteralVariableType expectedVariableType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var variableType = enumHelper.GetLiteralVariableType(type);

            //assert
            Assert.Equal(expectedVariableType, variableType);
        }

        [Fact]
        public void GetVariableTypeThrowsCriticalExceptionForInvalidType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetLiteralVariableType(typeof(void)));
        }

        [Theory]
        [InlineData(typeof(void), LiteralFunctionReturnType.Void)]
        [InlineData(typeof(bool), LiteralFunctionReturnType.Boolean)]
        [InlineData(typeof(DateTimeOffset), LiteralFunctionReturnType.DateTimeOffset)]
        [InlineData(typeof(DateTime), LiteralFunctionReturnType.DateTime)]
        [InlineData(typeof(Date), LiteralFunctionReturnType.Date)]
        [InlineData(typeof(TimeSpan), LiteralFunctionReturnType.TimeSpan)]
        [InlineData(typeof(TimeOfDay), LiteralFunctionReturnType.TimeOfDay)]
        [InlineData(typeof(Guid), LiteralFunctionReturnType.Guid)]
        [InlineData(typeof(decimal), LiteralFunctionReturnType.Decimal)]
        [InlineData(typeof(byte), LiteralFunctionReturnType.Byte)]
        [InlineData(typeof(short), LiteralFunctionReturnType.Short)]
        [InlineData(typeof(int), LiteralFunctionReturnType.Integer)]
        [InlineData(typeof(long), LiteralFunctionReturnType.Long)]
        [InlineData(typeof(float), LiteralFunctionReturnType.Float)]
        [InlineData(typeof(double), LiteralFunctionReturnType.Double)]
        [InlineData(typeof(char), LiteralFunctionReturnType.Char)]
        [InlineData(typeof(sbyte), LiteralFunctionReturnType.SByte)]
        [InlineData(typeof(ushort), LiteralFunctionReturnType.UShort)]
        [InlineData(typeof(uint), LiteralFunctionReturnType.UInteger)]
        [InlineData(typeof(ulong), LiteralFunctionReturnType.ULong)]
        [InlineData(typeof(string), LiteralFunctionReturnType.String)]
        [InlineData(typeof(bool?), LiteralFunctionReturnType.NullableBoolean)]
        [InlineData(typeof(DateTimeOffset?), LiteralFunctionReturnType.NullableDateTimeOffset)]
        [InlineData(typeof(DateTime?), LiteralFunctionReturnType.NullableDateTime)]
        [InlineData(typeof(Date?), LiteralFunctionReturnType.NullableDate)]
        [InlineData(typeof(TimeSpan?), LiteralFunctionReturnType.NullableTimeSpan)]
        [InlineData(typeof(TimeOfDay?), LiteralFunctionReturnType.NullableTimeOfDay)]
        [InlineData(typeof(Guid?), LiteralFunctionReturnType.NullableGuid)]
        [InlineData(typeof(decimal?), LiteralFunctionReturnType.NullableDecimal)]
        [InlineData(typeof(byte?), LiteralFunctionReturnType.NullableByte)]
        [InlineData(typeof(short?), LiteralFunctionReturnType.NullableShort)]
        [InlineData(typeof(int?), LiteralFunctionReturnType.NullableInteger)]
        [InlineData(typeof(long?), LiteralFunctionReturnType.NullableLong)]
        [InlineData(typeof(float?), LiteralFunctionReturnType.NullableFloat)]
        [InlineData(typeof(double?), LiteralFunctionReturnType.NullableDouble)]
        [InlineData(typeof(char?), LiteralFunctionReturnType.NullableChar)]
        [InlineData(typeof(sbyte?), LiteralFunctionReturnType.NullableSByte)]
        [InlineData(typeof(ushort?), LiteralFunctionReturnType.NullableUShort)]
        [InlineData(typeof(uint?), LiteralFunctionReturnType.NullableUInteger)]
        [InlineData(typeof(ulong?), LiteralFunctionReturnType.NullableULong)]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetFunctionReturnTypeReturnsTheExpectedEnumType(Type type, LiteralFunctionReturnType expectedFunctionReturnType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var functionReturnType = enumHelper.GetLiteralFunctionReturnType(type);

            //assert
            Assert.Equal(expectedFunctionReturnType, functionReturnType);
        }

        [Fact]
        public void GetFunctionReturnTypeThrowsCriticalExceptionForInvalidType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetLiteralFunctionReturnType(typeof(object)));
        }

        [Theory]
        [InlineData(typeof(void), LiteralType.Void)]
        [InlineData(typeof(bool), LiteralType.Boolean)]
        [InlineData(typeof(DateTimeOffset), LiteralType.DateTimeOffset)]
        [InlineData(typeof(DateTime), LiteralType.DateTime)]
        [InlineData(typeof(Date), LiteralType.Date)]
        [InlineData(typeof(TimeSpan), LiteralType.TimeSpan)]
        [InlineData(typeof(TimeOfDay), LiteralType.TimeOfDay)]
        [InlineData(typeof(Guid), LiteralType.Guid)]
        [InlineData(typeof(decimal), LiteralType.Decimal)]
        [InlineData(typeof(byte), LiteralType.Byte)]
        [InlineData(typeof(short), LiteralType.Short)]
        [InlineData(typeof(int), LiteralType.Integer)]
        [InlineData(typeof(long), LiteralType.Long)]
        [InlineData(typeof(float), LiteralType.Float)]
        [InlineData(typeof(double), LiteralType.Double)]
        [InlineData(typeof(char), LiteralType.Char)]
        [InlineData(typeof(sbyte), LiteralType.SByte)]
        [InlineData(typeof(ushort), LiteralType.UShort)]
        [InlineData(typeof(uint), LiteralType.UInteger)]
        [InlineData(typeof(ulong), LiteralType.ULong)]
        [InlineData(typeof(string), LiteralType.String)]
        [InlineData(typeof(bool?), LiteralType.NullableBoolean)]
        [InlineData(typeof(DateTimeOffset?), LiteralType.NullableDateTimeOffset)]
        [InlineData(typeof(DateTime?), LiteralType.NullableDateTime)]
        [InlineData(typeof(Date?), LiteralType.NullableDate)]
        [InlineData(typeof(TimeSpan?), LiteralType.NullableTimeSpan)]
        [InlineData(typeof(TimeOfDay?), LiteralType.NullableTimeOfDay)]
        [InlineData(typeof(Guid?), LiteralType.NullableGuid)]
        [InlineData(typeof(decimal?), LiteralType.NullableDecimal)]
        [InlineData(typeof(byte?), LiteralType.NullableByte)]
        [InlineData(typeof(short?), LiteralType.NullableShort)]
        [InlineData(typeof(int?), LiteralType.NullableInteger)]
        [InlineData(typeof(long?), LiteralType.NullableLong)]
        [InlineData(typeof(float?), LiteralType.NullableFloat)]
        [InlineData(typeof(double?), LiteralType.NullableDouble)]
        [InlineData(typeof(char?), LiteralType.NullableChar)]
        [InlineData(typeof(sbyte?), LiteralType.NullableSByte)]
        [InlineData(typeof(ushort?), LiteralType.NullableUShort)]
        [InlineData(typeof(uint?), LiteralType.NullableUInteger)]
        [InlineData(typeof(ulong?), LiteralType.NullableULong)]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetLiteralTypeReturnsTheExpectedEnumType(Type type, LiteralType expectedLiteralType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var literalType = enumHelper.GetLiteralType(type);

            //assert
            Assert.Equal(expectedLiteralType, literalType);
        }

        [Theory]
        [InlineData(LiteralFunctionReturnType.Void, typeof(void))]
        [InlineData(LiteralFunctionReturnType.Boolean, typeof(bool))]
        [InlineData(LiteralFunctionReturnType.DateTimeOffset, typeof(DateTimeOffset))]
        [InlineData(LiteralFunctionReturnType.DateTime, typeof(DateTime))]
        [InlineData(LiteralFunctionReturnType.Date, typeof(Date))]
        [InlineData(LiteralFunctionReturnType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralFunctionReturnType.TimeOfDay, typeof(TimeOfDay))]
        [InlineData(LiteralFunctionReturnType.Guid, typeof(Guid))]
        [InlineData(LiteralFunctionReturnType.Decimal, typeof(decimal))]
        [InlineData(LiteralFunctionReturnType.Byte, typeof(byte))]
        [InlineData(LiteralFunctionReturnType.Short, typeof(short))]
        [InlineData(LiteralFunctionReturnType.Integer, typeof(int))]
        [InlineData(LiteralFunctionReturnType.Long, typeof(long))]
        [InlineData(LiteralFunctionReturnType.Float, typeof(float))]
        [InlineData(LiteralFunctionReturnType.Double, typeof(double))]
        [InlineData(LiteralFunctionReturnType.Char, typeof(char))]
        [InlineData(LiteralFunctionReturnType.SByte, typeof(sbyte))]
        [InlineData(LiteralFunctionReturnType.UShort, typeof(ushort))]
        [InlineData(LiteralFunctionReturnType.UInteger, typeof(uint))]
        [InlineData(LiteralFunctionReturnType.ULong, typeof(ulong))]
        [InlineData(LiteralFunctionReturnType.String, typeof(string))]
        [InlineData(LiteralFunctionReturnType.NullableBoolean, typeof(bool?))]
        [InlineData(LiteralFunctionReturnType.NullableDateTimeOffset, typeof(DateTimeOffset?))]
        [InlineData(LiteralFunctionReturnType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralFunctionReturnType.NullableDate, typeof(Date?))]
        [InlineData(LiteralFunctionReturnType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralFunctionReturnType.NullableTimeOfDay, typeof(TimeOfDay?))]
        [InlineData(LiteralFunctionReturnType.NullableGuid, typeof(Guid?))]
        [InlineData(LiteralFunctionReturnType.NullableDecimal, typeof(decimal?))]
        [InlineData(LiteralFunctionReturnType.NullableByte, typeof(byte?))]
        [InlineData(LiteralFunctionReturnType.NullableShort, typeof(short?))]
        [InlineData(LiteralFunctionReturnType.NullableInteger, typeof(int?))]
        [InlineData(LiteralFunctionReturnType.NullableLong, typeof(long?))]
        [InlineData(LiteralFunctionReturnType.NullableFloat, typeof(float?))]
        [InlineData(LiteralFunctionReturnType.NullableDouble, typeof(double?))]
        [InlineData(LiteralFunctionReturnType.NullableChar, typeof(char?))]
        [InlineData(LiteralFunctionReturnType.NullableSByte, typeof(sbyte?))]
        [InlineData(LiteralFunctionReturnType.NullableUShort, typeof(ushort?))]
        [InlineData(LiteralFunctionReturnType.NullableUInteger, typeof(uint?))]
        [InlineData(LiteralFunctionReturnType.NullableULong, typeof(ulong?))]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetSystemTypeFromLiteralFunctionReturnTypeReturnsTheExpectedType(LiteralFunctionReturnType literalType, Type expectedType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetSystemType(literalType);

            //assert
            Assert.Equal(expectedType, result);
        }

        [Theory]
        [InlineData(LiteralParameterType.Any, typeof(string))]
        [InlineData(LiteralParameterType.Boolean, typeof(bool))]
        [InlineData(LiteralParameterType.DateTimeOffset, typeof(DateTimeOffset))]
        [InlineData(LiteralParameterType.DateTime, typeof(DateTime))]
        [InlineData(LiteralParameterType.Date, typeof(Date))]
        [InlineData(LiteralParameterType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralParameterType.TimeOfDay, typeof(TimeOfDay))]
        [InlineData(LiteralParameterType.Guid, typeof(Guid))]
        [InlineData(LiteralParameterType.Decimal, typeof(decimal))]
        [InlineData(LiteralParameterType.Byte, typeof(byte))]
        [InlineData(LiteralParameterType.Short, typeof(short))]
        [InlineData(LiteralParameterType.Integer, typeof(int))]
        [InlineData(LiteralParameterType.Long, typeof(long))]
        [InlineData(LiteralParameterType.Float, typeof(float))]
        [InlineData(LiteralParameterType.Double, typeof(double))]
        [InlineData(LiteralParameterType.Char, typeof(char))]
        [InlineData(LiteralParameterType.SByte, typeof(sbyte))]
        [InlineData(LiteralParameterType.UShort, typeof(ushort))]
        [InlineData(LiteralParameterType.UInteger, typeof(uint))]
        [InlineData(LiteralParameterType.ULong, typeof(ulong))]
        [InlineData(LiteralParameterType.String, typeof(string))]
        [InlineData(LiteralParameterType.NullableBoolean, typeof(bool?))]
        [InlineData(LiteralParameterType.NullableDateTimeOffset, typeof(DateTimeOffset?))]
        [InlineData(LiteralParameterType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralParameterType.NullableDate, typeof(Date?))]
        [InlineData(LiteralParameterType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralParameterType.NullableTimeOfDay, typeof(TimeOfDay?))]
        [InlineData(LiteralParameterType.NullableGuid, typeof(Guid?))]
        [InlineData(LiteralParameterType.NullableDecimal, typeof(decimal?))]
        [InlineData(LiteralParameterType.NullableByte, typeof(byte?))]
        [InlineData(LiteralParameterType.NullableShort, typeof(short?))]
        [InlineData(LiteralParameterType.NullableInteger, typeof(int?))]
        [InlineData(LiteralParameterType.NullableLong, typeof(long?))]
        [InlineData(LiteralParameterType.NullableFloat, typeof(float?))]
        [InlineData(LiteralParameterType.NullableDouble, typeof(double?))]
        [InlineData(LiteralParameterType.NullableChar, typeof(char?))]
        [InlineData(LiteralParameterType.NullableSByte, typeof(sbyte?))]
        [InlineData(LiteralParameterType.NullableUShort, typeof(ushort?))]
        [InlineData(LiteralParameterType.NullableUInteger, typeof(uint?))]
        [InlineData(LiteralParameterType.NullableULong, typeof(ulong?))]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetSystemTypeFromLiteralParameterTypeReturnsTheExpectedType(LiteralParameterType literalType, Type expectedType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetSystemType(literalType);

            //assert
            Assert.Equal(expectedType, result);
        }

        [Theory]
        [InlineData(LiteralType.Void, typeof(void))]
        [InlineData(LiteralType.Boolean, typeof(bool))]
        [InlineData(LiteralType.DateTimeOffset, typeof(DateTimeOffset))]
        [InlineData(LiteralType.DateTime, typeof(DateTime))]
        [InlineData(LiteralType.Date, typeof(Date))]
        [InlineData(LiteralType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralType.TimeOfDay, typeof(TimeOfDay))]
        [InlineData(LiteralType.Guid, typeof(Guid))]
        [InlineData(LiteralType.Decimal, typeof(decimal))]
        [InlineData(LiteralType.Byte, typeof(byte))]
        [InlineData(LiteralType.Short, typeof(short))]
        [InlineData(LiteralType.Integer, typeof(int))]
        [InlineData(LiteralType.Long, typeof(long))]
        [InlineData(LiteralType.Float, typeof(float))]
        [InlineData(LiteralType.Double, typeof(double))]
        [InlineData(LiteralType.Char, typeof(char))]
        [InlineData(LiteralType.SByte, typeof(sbyte))]
        [InlineData(LiteralType.UShort, typeof(ushort))]
        [InlineData(LiteralType.UInteger, typeof(uint))]
        [InlineData(LiteralType.ULong, typeof(ulong))]
        [InlineData(LiteralType.String, typeof(string))]
        [InlineData(LiteralType.NullableBoolean, typeof(bool?))]
        [InlineData(LiteralType.NullableDateTimeOffset, typeof(DateTimeOffset?))]
        [InlineData(LiteralType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralType.NullableDate, typeof(Date?))]
        [InlineData(LiteralType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralType.NullableTimeOfDay, typeof(TimeOfDay?))]
        [InlineData(LiteralType.NullableGuid, typeof(Guid?))]
        [InlineData(LiteralType.NullableDecimal, typeof(decimal?))]
        [InlineData(LiteralType.NullableByte, typeof(byte?))]
        [InlineData(LiteralType.NullableShort, typeof(short?))]
        [InlineData(LiteralType.NullableInteger, typeof(int?))]
        [InlineData(LiteralType.NullableLong, typeof(long?))]
        [InlineData(LiteralType.NullableFloat, typeof(float?))]
        [InlineData(LiteralType.NullableDouble, typeof(double?))]
        [InlineData(LiteralType.NullableChar, typeof(char?))]
        [InlineData(LiteralType.NullableSByte, typeof(sbyte?))]
        [InlineData(LiteralType.NullableUShort, typeof(ushort?))]
        [InlineData(LiteralType.NullableUInteger, typeof(uint?))]
        [InlineData(LiteralType.NullableULong, typeof(ulong?))]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetSystemTypeFromLiteralTypeReturnsTheExpectedType(LiteralType literalType, Type expectedType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetSystemType(literalType);

            //assert
            Assert.Equal(expectedType, result);
        }

        [Theory]
        [InlineData(LiteralVariableType.Boolean, typeof(bool))]
        [InlineData(LiteralVariableType.DateTimeOffset, typeof(DateTimeOffset))]
        [InlineData(LiteralVariableType.DateTime, typeof(DateTime))]
        [InlineData(LiteralVariableType.Date, typeof(Date))]
        [InlineData(LiteralVariableType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralVariableType.TimeOfDay, typeof(TimeOfDay))]
        [InlineData(LiteralVariableType.Guid, typeof(Guid))]
        [InlineData(LiteralVariableType.Decimal, typeof(decimal))]
        [InlineData(LiteralVariableType.Byte, typeof(byte))]
        [InlineData(LiteralVariableType.Short, typeof(short))]
        [InlineData(LiteralVariableType.Integer, typeof(int))]
        [InlineData(LiteralVariableType.Long, typeof(long))]
        [InlineData(LiteralVariableType.Float, typeof(float))]
        [InlineData(LiteralVariableType.Double, typeof(double))]
        [InlineData(LiteralVariableType.Char, typeof(char))]
        [InlineData(LiteralVariableType.SByte, typeof(sbyte))]
        [InlineData(LiteralVariableType.UShort, typeof(ushort))]
        [InlineData(LiteralVariableType.UInteger, typeof(uint))]
        [InlineData(LiteralVariableType.ULong, typeof(ulong))]
        [InlineData(LiteralVariableType.String, typeof(string))]
        [InlineData(LiteralVariableType.NullableBoolean, typeof(bool?))]
        [InlineData(LiteralVariableType.NullableDateTimeOffset, typeof(DateTimeOffset?))]
        [InlineData(LiteralVariableType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralVariableType.NullableDate, typeof(Date?))]
        [InlineData(LiteralVariableType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralVariableType.NullableTimeOfDay, typeof(TimeOfDay?))]
        [InlineData(LiteralVariableType.NullableGuid, typeof(Guid?))]
        [InlineData(LiteralVariableType.NullableDecimal, typeof(decimal?))]
        [InlineData(LiteralVariableType.NullableByte, typeof(byte?))]
        [InlineData(LiteralVariableType.NullableShort, typeof(short?))]
        [InlineData(LiteralVariableType.NullableInteger, typeof(int?))]
        [InlineData(LiteralVariableType.NullableLong, typeof(long?))]
        [InlineData(LiteralVariableType.NullableFloat, typeof(float?))]
        [InlineData(LiteralVariableType.NullableDouble, typeof(double?))]
        [InlineData(LiteralVariableType.NullableChar, typeof(char?))]
        [InlineData(LiteralVariableType.NullableSByte, typeof(sbyte?))]
        [InlineData(LiteralVariableType.NullableUShort, typeof(ushort?))]
        [InlineData(LiteralVariableType.NullableUInteger, typeof(uint?))]
        [InlineData(LiteralVariableType.NullableULong, typeof(ulong?))]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void GetSystemTypeFromLiteralVariableTypeReturnsTheExpectedType(LiteralVariableType literalType, Type expectedType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetSystemType(literalType);

            //assert
            Assert.Equal(expectedType, result);
        }

        [Fact]
        public void GetLiteralTypeThrowsCriticalExceptionForInvalidType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetLiteralType(typeof(object)));
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

        [Fact]
        public void ParseEnumTextThrowsCriticalExceptionForInvalidType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.ParseEnumText<string>("xyz"));
        }

        [Theory]
        [InlineData("IGenericEnumerable", ListType.IGenericEnumerable)]
        [InlineData("Array", ListType.Array)]
        [Trait(TraitTypes.TestCategory, TestCategories.UnitTest)]
        internal void ParseEnumTextReturnsTheExpectedEnumType(string listType, ListType expectedResult)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.ParseEnumText<ListType>(listType);

            //assert
            Assert.Equal(expectedResult, result);
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

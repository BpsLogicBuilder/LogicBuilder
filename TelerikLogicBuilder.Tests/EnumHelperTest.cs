using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Xunit;
using FlowBuilder = ABIS.LogicBuilder.FlowBuilder;

namespace TelerikLogicBuilder.Tests
{
    public class EnumHelperTest
    {
        public EnumHelperTest()
        {
            serviceProvider = FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
        }

        #region Fields
        private readonly IServiceProvider serviceProvider;
        #endregion Fields

        [Theory]
        [InlineData(typeof(ParameterCategory))]
        [InlineData(typeof(GenericConfigCategory))]
        [InlineData(typeof(ListParameterInputStyle))]
        [InlineData(typeof(ListType))]
        [InlineData(typeof(ListVariableInputStyle))]
        [InlineData(typeof(LiteralFunctionReturnType))]
        [InlineData(typeof(LiteralListElementType))]
        [InlineData(typeof(LiteralParameterInputStyle))]
        [InlineData(typeof(LiteralParameterType))]
        [InlineData(typeof(LiteralType))]
        [InlineData(typeof(LiteralVariableInputStyle))]
        [InlineData(typeof(ReferenceCategories))]
        public void GetVisibleEnumText(Type enumType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            MethodInfo mInfo = typeof(IEnumHelper)
                .GetMethod(nameof(IEnumHelper.GetVisibleEnumText))!
                .MakeGenericMethod(new Type[] { enumType })!;

            //act assert
            foreach (var enumValue in Enum.GetValues(enumType))
            {
                object? result = mInfo.Invoke(enumHelper, new object[] { enumValue });
                Assert.NotNull(result);
                Assert.True(result is string);
                Assert.False(string.IsNullOrEmpty((string)result!));
            }
        }

        [Fact]
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
        [InlineData(typeof(bool), LiteralListElementType.Boolean)]
        [InlineData(typeof(DateTimeOffset), LiteralListElementType.DateTimeOffset)]
        [InlineData(typeof(DateOnly), LiteralListElementType.DateOnly)]
        [InlineData(typeof(DateTime), LiteralListElementType.DateTime)]
        [InlineData(typeof(Date), LiteralListElementType.Date)]
        [InlineData(typeof(TimeSpan), LiteralListElementType.TimeSpan)]
        [InlineData(typeof(TimeOnly), LiteralListElementType.TimeOnly)]
        [InlineData(typeof(TimeOfDay), LiteralListElementType.TimeOfDay)]
        [InlineData(typeof(Guid), LiteralListElementType.Guid)]
        [InlineData(typeof(decimal), LiteralListElementType.Decimal)]
        [InlineData(typeof(byte), LiteralListElementType.Byte)]
        [InlineData(typeof(short), LiteralListElementType.Short)]
        [InlineData(typeof(int), LiteralListElementType.Integer)]
        [InlineData(typeof(long), LiteralListElementType.Long)]
        [InlineData(typeof(float), LiteralListElementType.Float)]
        [InlineData(typeof(double), LiteralListElementType.Double)]
        [InlineData(typeof(char), LiteralListElementType.Char)]
        [InlineData(typeof(sbyte), LiteralListElementType.SByte)]
        [InlineData(typeof(ushort), LiteralListElementType.UShort)]
        [InlineData(typeof(uint), LiteralListElementType.UInteger)]
        [InlineData(typeof(ulong), LiteralListElementType.ULong)]
        [InlineData(typeof(string), LiteralListElementType.String)]
        [InlineData(typeof(bool?), LiteralListElementType.NullableBoolean)]
        [InlineData(typeof(DateTimeOffset?), LiteralListElementType.NullableDateTimeOffset)]
        [InlineData(typeof(DateOnly?), LiteralListElementType.NullableDateOnly)]
        [InlineData(typeof(DateTime?), LiteralListElementType.NullableDateTime)]
        [InlineData(typeof(Date?), LiteralListElementType.NullableDate)]
        [InlineData(typeof(TimeSpan?), LiteralListElementType.NullableTimeSpan)]
        [InlineData(typeof(TimeOnly?), LiteralListElementType.NullableTimeOnly)]
        [InlineData(typeof(TimeOfDay?), LiteralListElementType.NullableTimeOfDay)]
        [InlineData(typeof(Guid?), LiteralListElementType.NullableGuid)]
        [InlineData(typeof(decimal?), LiteralListElementType.NullableDecimal)]
        [InlineData(typeof(byte?), LiteralListElementType.NullableByte)]
        [InlineData(typeof(short?), LiteralListElementType.NullableShort)]
        [InlineData(typeof(int?), LiteralListElementType.NullableInteger)]
        [InlineData(typeof(long?), LiteralListElementType.NullableLong)]
        [InlineData(typeof(float?), LiteralListElementType.NullableFloat)]
        [InlineData(typeof(double?), LiteralListElementType.NullableDouble)]
        [InlineData(typeof(char?), LiteralListElementType.NullableChar)]
        [InlineData(typeof(sbyte?), LiteralListElementType.NullableSByte)]
        [InlineData(typeof(ushort?), LiteralListElementType.NullableUShort)]
        [InlineData(typeof(uint?), LiteralListElementType.NullableUInteger)]
        [InlineData(typeof(ulong?), LiteralListElementType.NullableULong)]
        internal void GetLiteralListElementTypeReturnsTheExpectedEnumType(Type type, LiteralListElementType expectedParameterType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var listType = enumHelper.GetLiteralListElementType(type);

            //assert
            Assert.Equal(expectedParameterType, listType);
        }

        [Fact]
        public void GetLiteralListElementTypeThrowsCriticalExceptionForInvalidType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetLiteralListElementType(typeof(void)));
        }

        [Theory]
        [InlineData(typeof(bool), LiteralParameterType.Boolean)]
        [InlineData(typeof(DateTimeOffset), LiteralParameterType.DateTimeOffset)]
        [InlineData(typeof(DateOnly), LiteralParameterType.DateOnly)]
        [InlineData(typeof(DateTime), LiteralParameterType.DateTime)]
        [InlineData(typeof(Date), LiteralParameterType.Date)]
        [InlineData(typeof(TimeSpan), LiteralParameterType.TimeSpan)]
        [InlineData(typeof(TimeOnly), LiteralParameterType.TimeOnly)]
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
        [InlineData(typeof(DateOnly?), LiteralParameterType.NullableDateOnly)]
        [InlineData(typeof(DateTime?), LiteralParameterType.NullableDateTime)]
        [InlineData(typeof(Date?), LiteralParameterType.NullableDate)]
        [InlineData(typeof(TimeSpan?), LiteralParameterType.NullableTimeSpan)]
        [InlineData(typeof(TimeOnly?), LiteralParameterType.NullableTimeOnly)]
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
        [InlineData(typeof(DateOnly), LiteralVariableType.DateOnly)]
        [InlineData(typeof(DateTime), LiteralVariableType.DateTime)]
        [InlineData(typeof(Date), LiteralVariableType.Date)]
        [InlineData(typeof(TimeSpan), LiteralVariableType.TimeSpan)]
        [InlineData(typeof(TimeOnly), LiteralVariableType.TimeOnly)]
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
        [InlineData(typeof(DateOnly?), LiteralVariableType.NullableDateOnly)]
        [InlineData(typeof(DateTime?), LiteralVariableType.NullableDateTime)]
        [InlineData(typeof(Date?), LiteralVariableType.NullableDate)]
        [InlineData(typeof(TimeSpan?), LiteralVariableType.NullableTimeSpan)]
        [InlineData(typeof(TimeOnly?), LiteralVariableType.NullableTimeOnly)]
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
        [InlineData(typeof(DateOnly), LiteralFunctionReturnType.DateOnly)]
        [InlineData(typeof(DateTime), LiteralFunctionReturnType.DateTime)]
        [InlineData(typeof(Date), LiteralFunctionReturnType.Date)]
        [InlineData(typeof(TimeSpan), LiteralFunctionReturnType.TimeSpan)]
        [InlineData(typeof(TimeOnly), LiteralFunctionReturnType.TimeOnly)]
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
        [InlineData(typeof(DateOnly?), LiteralFunctionReturnType.NullableDateOnly)]
        [InlineData(typeof(DateTime?), LiteralFunctionReturnType.NullableDateTime)]
        [InlineData(typeof(Date?), LiteralFunctionReturnType.NullableDate)]
        [InlineData(typeof(TimeSpan?), LiteralFunctionReturnType.NullableTimeSpan)]
        [InlineData(typeof(TimeOnly?), LiteralFunctionReturnType.NullableTimeOnly)]
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
        [InlineData(typeof(DateOnly), LiteralType.DateOnly)]
        [InlineData(typeof(DateTime), LiteralType.DateTime)]
        [InlineData(typeof(Date), LiteralType.Date)]
        [InlineData(typeof(TimeSpan), LiteralType.TimeSpan)]
        [InlineData(typeof(TimeOnly), LiteralType.TimeOnly)]
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
        [InlineData(typeof(DateOnly?), LiteralType.NullableDateOnly)]
        [InlineData(typeof(DateTime?), LiteralType.NullableDateTime)]
        [InlineData(typeof(Date?), LiteralType.NullableDate)]
        [InlineData(typeof(TimeSpan?), LiteralType.NullableTimeSpan)]
        [InlineData(typeof(TimeOnly?), LiteralType.NullableTimeOnly)]
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
        [InlineData(ListType.Array, typeof(string), typeof(string[]))]
        [InlineData(ListType.GenericCollection, typeof(string), typeof(Collection<string>))]
        [InlineData(ListType.GenericList, typeof(string), typeof(List<string>))]
        [InlineData(ListType.IGenericCollection, typeof(string), typeof(ICollection<string>))]
        [InlineData(ListType.IGenericEnumerable, typeof(string), typeof(IEnumerable<string>))]
        [InlineData(ListType.IGenericList, typeof(string), typeof(IList<string>))]
        internal void GetSystemTypeGivenListAndElementTypeWorks(ListType listType, Type elementType, Type expectedResult)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetSystemType(listType, elementType);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        internal void GetSystemTypeGivenListAndElementTypeThrowsForUndefinedEnum()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetSystemType((ListType)(-1), typeof(string)));
        }

        [Theory]
        [InlineData(LiteralFunctionReturnType.Void, typeof(void))]
        [InlineData(LiteralFunctionReturnType.Boolean, typeof(bool))]
        [InlineData(LiteralFunctionReturnType.DateTimeOffset, typeof(DateTimeOffset))]
        [InlineData(LiteralFunctionReturnType.DateOnly, typeof(DateOnly))]
        [InlineData(LiteralFunctionReturnType.DateTime, typeof(DateTime))]
        [InlineData(LiteralFunctionReturnType.Date, typeof(Date))]
        [InlineData(LiteralFunctionReturnType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralFunctionReturnType.TimeOnly, typeof(TimeOnly))]
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
        [InlineData(LiteralFunctionReturnType.NullableDateOnly, typeof(DateOnly?))]
        [InlineData(LiteralFunctionReturnType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralFunctionReturnType.NullableDate, typeof(Date?))]
        [InlineData(LiteralFunctionReturnType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralFunctionReturnType.NullableTimeOnly, typeof(TimeOnly?))]
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
        [InlineData(LiteralListElementType.Any, typeof(string))]
        [InlineData(LiteralListElementType.Boolean, typeof(bool))]
        [InlineData(LiteralListElementType.DateTimeOffset, typeof(DateTimeOffset))]
        [InlineData(LiteralListElementType.DateOnly, typeof(DateOnly))]
        [InlineData(LiteralListElementType.DateTime, typeof(DateTime))]
        [InlineData(LiteralListElementType.Date, typeof(Date))]
        [InlineData(LiteralListElementType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralListElementType.TimeOnly, typeof(TimeOnly))]
        [InlineData(LiteralListElementType.TimeOfDay, typeof(TimeOfDay))]
        [InlineData(LiteralListElementType.Guid, typeof(Guid))]
        [InlineData(LiteralListElementType.Decimal, typeof(decimal))]
        [InlineData(LiteralListElementType.Byte, typeof(byte))]
        [InlineData(LiteralListElementType.Short, typeof(short))]
        [InlineData(LiteralListElementType.Integer, typeof(int))]
        [InlineData(LiteralListElementType.Long, typeof(long))]
        [InlineData(LiteralListElementType.Float, typeof(float))]
        [InlineData(LiteralListElementType.Double, typeof(double))]
        [InlineData(LiteralListElementType.Char, typeof(char))]
        [InlineData(LiteralListElementType.SByte, typeof(sbyte))]
        [InlineData(LiteralListElementType.UShort, typeof(ushort))]
        [InlineData(LiteralListElementType.UInteger, typeof(uint))]
        [InlineData(LiteralListElementType.ULong, typeof(ulong))]
        [InlineData(LiteralListElementType.String, typeof(string))]
        [InlineData(LiteralListElementType.NullableBoolean, typeof(bool?))]
        [InlineData(LiteralListElementType.NullableDateTimeOffset, typeof(DateTimeOffset?))]
        [InlineData(LiteralListElementType.NullableDateOnly, typeof(DateOnly?))]
        [InlineData(LiteralListElementType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralListElementType.NullableDate, typeof(Date?))]
        [InlineData(LiteralListElementType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralListElementType.NullableTimeOnly, typeof(TimeOnly?))]
        [InlineData(LiteralListElementType.NullableTimeOfDay, typeof(TimeOfDay?))]
        [InlineData(LiteralListElementType.NullableGuid, typeof(Guid?))]
        [InlineData(LiteralListElementType.NullableDecimal, typeof(decimal?))]
        [InlineData(LiteralListElementType.NullableByte, typeof(byte?))]
        [InlineData(LiteralListElementType.NullableShort, typeof(short?))]
        [InlineData(LiteralListElementType.NullableInteger, typeof(int?))]
        [InlineData(LiteralListElementType.NullableLong, typeof(long?))]
        [InlineData(LiteralListElementType.NullableFloat, typeof(float?))]
        [InlineData(LiteralListElementType.NullableDouble, typeof(double?))]
        [InlineData(LiteralListElementType.NullableChar, typeof(char?))]
        [InlineData(LiteralListElementType.NullableSByte, typeof(sbyte?))]
        [InlineData(LiteralListElementType.NullableUShort, typeof(ushort?))]
        [InlineData(LiteralListElementType.NullableUInteger, typeof(uint?))]
        [InlineData(LiteralListElementType.NullableULong, typeof(ulong?))]
        internal void GetSystemTypeFromLiteralListElementTypeReturnsTheExpectedType(LiteralListElementType literalType, Type expectedType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetSystemType(literalType);

            //assert
            Assert.Equal(expectedType, result);
        }

        [Fact]
        internal void GetSystemTypeFromLiteralListElementTypeThrowsForUndefinedEnum()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetSystemType((LiteralListElementType)(-1)));
        }

        [Theory]
        [InlineData(LiteralParameterType.Any, typeof(string))]
        [InlineData(LiteralParameterType.Boolean, typeof(bool))]
        [InlineData(LiteralParameterType.DateTimeOffset, typeof(DateTimeOffset))]
        [InlineData(LiteralParameterType.DateOnly, typeof(DateOnly))]
        [InlineData(LiteralParameterType.DateTime, typeof(DateTime))]
        [InlineData(LiteralParameterType.Date, typeof(Date))]
        [InlineData(LiteralParameterType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralParameterType.TimeOnly, typeof(TimeOnly))]
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
        [InlineData(LiteralParameterType.NullableDateOnly, typeof(DateOnly?))]
        [InlineData(LiteralParameterType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralParameterType.NullableDate, typeof(Date?))]
        [InlineData(LiteralParameterType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralParameterType.NullableTimeOnly, typeof(TimeOnly?))]
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
        internal void GetSystemTypeFromLiteralParameterTypeReturnsTheExpectedType(LiteralParameterType literalType, Type expectedType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetSystemType(literalType);

            //assert
            Assert.Equal(expectedType, result);
        }

        [Fact]
        internal void GetSystemTypeFromLiteralParameterTypeThrowsForUndefinedEnum()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //assert
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetSystemType((LiteralParameterType)(-1)));
        }

        [Theory]
        [InlineData(LiteralType.Void, typeof(void))]
        [InlineData(LiteralType.Boolean, typeof(bool))]
        [InlineData(LiteralType.DateTimeOffset, typeof(DateTimeOffset))]
        [InlineData(LiteralType.DateOnly, typeof(DateOnly))]
        [InlineData(LiteralType.DateTime, typeof(DateTime))]
        [InlineData(LiteralType.Date, typeof(Date))]
        [InlineData(LiteralType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralType.TimeOnly, typeof(TimeOnly))]
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
        [InlineData(LiteralType.NullableDateOnly, typeof(DateOnly?))]
        [InlineData(LiteralType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralType.NullableDate, typeof(Date?))]
        [InlineData(LiteralType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralType.NullableTimeOnly, typeof(TimeOnly?))]
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
        [InlineData(LiteralVariableType.DateOnly, typeof(DateOnly))]
        [InlineData(LiteralVariableType.DateTime, typeof(DateTime))]
        [InlineData(LiteralVariableType.Date, typeof(Date))]
        [InlineData(LiteralVariableType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralVariableType.TimeOnly, typeof(TimeOnly))]
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
        [InlineData(LiteralVariableType.NullableDateOnly, typeof(DateOnly?))]
        [InlineData(LiteralVariableType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralVariableType.NullableDate, typeof(Date?))]
        [InlineData(LiteralVariableType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralVariableType.NullableTimeOnly, typeof(TimeOnly?))]
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
        internal void GetSystemTypeFromLiteralVariableTypeReturnsTheExpectedType(LiteralVariableType literalType, Type expectedType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetSystemType(literalType);

            //assert
            Assert.Equal(expectedType, result);
        }

        [Theory]
        [InlineData(LiteralVariableType.Boolean, false)]
        [InlineData(LiteralVariableType.DateTimeOffset, false)]
        [InlineData(LiteralVariableType.DateOnly, false)]
        [InlineData(LiteralVariableType.DateTime, false)]
        [InlineData(LiteralVariableType.Date, false)]
        [InlineData(LiteralVariableType.TimeSpan, false)]
        [InlineData(LiteralVariableType.TimeOnly, false)]
        [InlineData(LiteralVariableType.TimeOfDay, false)]
        [InlineData(LiteralVariableType.Guid, false)]
        [InlineData(LiteralVariableType.Decimal, false)]
        [InlineData(LiteralVariableType.Byte, true)]
        [InlineData(LiteralVariableType.Short, true)]
        [InlineData(LiteralVariableType.Integer, true)]
        [InlineData(LiteralVariableType.Long, false)]
        [InlineData(LiteralVariableType.Float, false)]
        [InlineData(LiteralVariableType.Double, false)]
        [InlineData(LiteralVariableType.Char, true)]
        [InlineData(LiteralVariableType.SByte, true)]
        [InlineData(LiteralVariableType.UShort, true)]
        [InlineData(LiteralVariableType.UInteger, false)]
        [InlineData(LiteralVariableType.ULong, false)]
        [InlineData(LiteralVariableType.String, false)]
        [InlineData(LiteralVariableType.NullableBoolean, false)]
        [InlineData(LiteralVariableType.NullableDateTimeOffset, false)]
        [InlineData(LiteralVariableType.NullableDateOnly, false)]
        [InlineData(LiteralVariableType.NullableDateTime, false)]
        [InlineData(LiteralVariableType.NullableDate, false)]
        [InlineData(LiteralVariableType.NullableTimeSpan, false)]
        [InlineData(LiteralVariableType.NullableTimeOnly, false)]
        [InlineData(LiteralVariableType.NullableTimeOfDay, false)]
        [InlineData(LiteralVariableType.NullableGuid, false)]
        [InlineData(LiteralVariableType.NullableDecimal, false)]
        [InlineData(LiteralVariableType.NullableByte, false)]
        [InlineData(LiteralVariableType.NullableShort, false)]
        [InlineData(LiteralVariableType.NullableInteger, false)]
        [InlineData(LiteralVariableType.NullableLong, false)]
        [InlineData(LiteralVariableType.NullableFloat, false)]
        [InlineData(LiteralVariableType.NullableDouble, false)]
        [InlineData(LiteralVariableType.NullableChar, false)]
        [InlineData(LiteralVariableType.NullableSByte, false)]
        [InlineData(LiteralVariableType.NullableUShort, false)]
        [InlineData(LiteralVariableType.NullableUInteger, false)]
        [InlineData(LiteralVariableType.NullableULong, false)]
        internal void CanBeIntegerReturnsTheExpectedBoolean(LiteralVariableType literalType, bool expectedResult)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.CanBeInteger(literalType);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ConvertEnumListToStringListWorksWithDefinedValues()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            List<LiteralType> enumList = new() {  LiteralType.Decimal, LiteralType.Integer, LiteralType.String };

            //act
            var enumNames = enumHelper.ConvertEnumListToStringList(enumList);

            //assert
            Assert.False(enumNames.Contains("Decimal"));
            Assert.False(enumNames.Contains("Integer"));
            Assert.False(enumNames.Contains("String"));
            Assert.True(enumNames.Contains("Boolean"));
            Assert.True(enumNames.Contains("DateTime"));
            Assert.True(enumNames.Contains("DateTimeOffset"));
            Assert.True(enumNames.Contains("DateOnly"));
        }

        [Fact]
        public void ConvertEnumListToStringListThrowsCriticalExceptionForInvalidGenericType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.ConvertEnumListToStringList<object>(new List<object>()));
        }

        [Fact]
        public void ConvertToEnumListWorksWithDefinedValues()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            List<string> enumNames = new() { "Decimal", "Integer", "String" };

            //act
            var enumList = enumHelper.ConvertToEnumList<LiteralType>(enumNames);

            //assert
            Assert.True(enumList.SequenceEqual(new List<LiteralType> { LiteralType.Decimal, LiteralType.Integer, LiteralType.String }));
        }

        [Fact]
        public void ConvertToEnumListThrowsCriticalExceptionForInvalidGenericType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.ConvertToEnumList<object>(new List<string>()));
        }

        [Fact]
        public void ConvertToEnumListThrowsArgumentExceptionForUndefinedEnumValues()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            List<string> enumNames = new() { "XYZ" };

            //act
            Assert.Throws<ArgumentException>(() => enumHelper.ConvertToEnumList<LiteralType>(enumNames));
        }

        [Fact]
        public void ConvertVisibleDropDownValuesToEnumNamesWorksWithDefinedValues()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            List<string> enumNames = new() { "property", "FIELD", "ArrayItem" };

            //act
            var enumList = enumHelper.ConvertVisibleDropDownValuesToEnumNames<ValidIndirectReference>(enumNames).ToList();

            //assert
            Assert.True(enumList.SequenceEqual(new List<string> { "Property", "Field", "ArrayIndexer" }));
        }

        [Fact]
        public void ConvertVisibleDropDownValuesToEnumNamesWorksWithDefinedValuesForSatelliteCulture()
        {
            //arrange
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();
            List<string> enumNames = new() { "propertyfr", "FIELDfr", "ArrayItemfr" };

            //act
            var enumList = enumHelper.ConvertVisibleDropDownValuesToEnumNames<ValidIndirectReference>(enumNames).ToList();

            //assert
            Assert.True(enumList.SequenceEqual(new List<string> { "Property", "Field", "ArrayIndexer" }));
        }

        [Fact]
        public void ConvertVisibleDropDownValuesToEnumNamesThrowsCriticalExceptionForInvalidGenericType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.ConvertVisibleDropDownValuesToEnumNames<object>(new List<string>()));
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
        internal void GetTypeDescriptionReturnsTheExpectedString(ListType listType, string elementType, string expectedResult)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var description = enumHelper.GetTypeDescription(listType, elementType);

            //assert
            Assert.Equal(expectedResult, description);
        }

        [Theory]
        [InlineData("Add", true)]
        [InlineData("Subtract", true)]
        [InlineData("Multiply", true)]
        [InlineData("Divide", true)]
        [InlineData("Modulus", true)]
        [InlineData("IdentityInequality", true)]
        [InlineData("IdentityEquality", true)]
        [InlineData("ValueEquality", true)]
        [InlineData("BitwiseOr", true)]
        [InlineData("BitwiseAnd", true)]
        [InlineData("BooleanOr", true)]
        [InlineData("BooleanAnd", true)]
        [InlineData("LessThan", true)]
        [InlineData("LessThanOrEqual", true)]
        [InlineData("GreaterThan", true)]
        [InlineData("GreaterThanOrEqual", true)]
        [InlineData("Assign", false)]
        [InlineData("UndefinedEnumString", false)]
        internal void IsValidCodeBinaryOperatorReturnsTheExpectedResult(string stringToParse, bool expectedResult)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.IsValidCodeBinaryOperator(stringToParse);

            //assert
            Assert.Equal(expectedResult, result);
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
        internal void ParseEnumTextReturnsTheExpectedEnumType(string listType, ListType expectedResult)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.ParseEnumText<ListType>(listType);

            //assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(XmlDataConstants.LITERALPARAMETERELEMENT, ParameterCategory.Literal)]
        [InlineData(XmlDataConstants.OBJECTPARAMETERELEMENT, ParameterCategory.Object)]
        [InlineData(XmlDataConstants.GENERICPARAMETERELEMENT, ParameterCategory.Generic)]
        [InlineData(XmlDataConstants.LITERALLISTPARAMETERELEMENT, ParameterCategory.LiteralList)]
        [InlineData(XmlDataConstants.OBJECTLISTPARAMETERELEMENT, ParameterCategory.ObjectList)]
        [InlineData(XmlDataConstants.GENERICLISTPARAMETERELEMENT, ParameterCategory.GenericList)]
        internal void GetParameterCategoryReturnsTheExpectedCategory(string elementName, ParameterCategory expectedCategory)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var category = enumHelper.GetParameterCategory(elementName);

            //assert
            Assert.Equal(expectedCategory, category);
        }

        [Fact]
        public void GetParameterCategoryThrowsCriticalExceptionForInvalidElementName()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetParameterCategory("xyz"));
        }

        [Theory]
        [InlineData(XmlDataConstants.CONSTRUCTORELEMENT, ObjectCategory.Constructor)]
        [InlineData(XmlDataConstants.FUNCTIONELEMENT, ObjectCategory.Function)]
        [InlineData(XmlDataConstants.VARIABLEELEMENT, ObjectCategory.Variable)]
        [InlineData(XmlDataConstants.LITERALLISTELEMENT, ObjectCategory.LiteralList)]
        [InlineData(XmlDataConstants.OBJECTLISTELEMENT, ObjectCategory.ObjectList)]
        internal void GetObjectCategoryReturnsTheExpectedCategory(string elementName, ObjectCategory expectedCategory)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var category = enumHelper.GetObjectCategory(elementName);

            //assert
            Assert.Equal(expectedCategory, category);
        }

        [Fact]
        public void GetObjectCategoryThrowsCriticalExceptionForInvalidElementName()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetObjectCategory("xyz"));
        }

        [Theory]
        [InlineData(XmlDataConstants.LITERALELEMENT, ReturnTypeCategory.Literal)]
        [InlineData(XmlDataConstants.OBJECTELEMENT, ReturnTypeCategory.Object)]
        [InlineData(XmlDataConstants.GENERICELEMENT, ReturnTypeCategory.Generic)]
        [InlineData(XmlDataConstants.LITERALLISTELEMENT, ReturnTypeCategory.LiteralList)]
        [InlineData(XmlDataConstants.OBJECTLISTELEMENT, ReturnTypeCategory.ObjectList)]
        [InlineData(XmlDataConstants.GENERICLISTELEMENT, ReturnTypeCategory.GenericList)]
        internal void GetReturnTypeCategoryReturnsTheExpectedCategory(string elementName, ReturnTypeCategory expectedCategory)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var category = enumHelper.GetReturnTypeCategory(elementName);

            //assert
            Assert.Equal(expectedCategory, category);
        }

        [Fact]
        public void GetReturnTypeCategoryThrowsCriticalExceptionForInvalidElementName()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetReturnTypeCategory("xyz"));
        }

        [Theory]
        [InlineData(XmlDataConstants.LITERALVARIABLEELEMENT, VariableTypeCategory.Literal)]
        [InlineData(XmlDataConstants.OBJECTVARIABLEELEMENT, VariableTypeCategory.Object)]
        [InlineData(XmlDataConstants.LITERALLISTVARIABLEELEMENT, VariableTypeCategory.LiteralList)]
        [InlineData(XmlDataConstants.OBJECTLISTVARIABLEELEMENT, VariableTypeCategory.ObjectList)]
        internal void GetVariableTypeCategoryReturnsTheExpectedCategory(string elementName, VariableTypeCategory expectedCategory)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var category = enumHelper.GetVariableTypeCategory(elementName);

            //assert
            Assert.Equal(expectedCategory, category);
        }

        [Fact]
        public void GetVariableTypeCategoryThrowsCriticalExceptionForInvalidElementName()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetVariableTypeCategory("xyz"));
        }

        [Fact]
        public void ToValidIndirectReferenceDictionaryReturnsCorrectDictionaryForDefaultCulture()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            IDictionary<string, ValidIndirectReference> dictionary = enumHelper.ToValidIndirectReferenceDictionary();

            //assert
            Assert.Equal("field", dictionary.Keys.First());
        }

        [Fact]
        public void ToValidIndirectReferenceDictionaryReturnsCorrectDictionaryForSatelliteCulture()
        {
            //arrange
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            IDictionary<string, ValidIndirectReference> dictionary = enumHelper.ToValidIndirectReferenceDictionary();

            //assert
            Assert.Equal("fieldfr", dictionary.Keys.First());
        }

        [Fact]
        public void ToVisibleDropdownValuesReturnsCorrectDictionaryForDefaultCulture()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            IList<string> result = enumHelper.ToVisibleDropdownValues(new string[] { "Property", "Field", "ArrayIndexer" });

            //assert
            Assert.True(result.SequenceEqual(new List<string> { "Property", "Field", "ArrayItem" }));
        }

        [Fact]
        public void ToVisibleDropdownValuesReturnsCorrectDictionaryForSatelliteCulture()
        {
            //arrange
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            IList<string> result = enumHelper.ToVisibleDropdownValues(new string[] { "Property", "Field", "ArrayIndexer" });

            //assert
            Assert.True(result.SequenceEqual(new List<string> { "PropertyFR", "FieldFR", "ArrayItemFR" }));
        }

        [Fact]
        public void GetValidIndirectReferencesListReturnsExpectedStringForDefaultCulture()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            string inderectReferences = enumHelper.GetValidIndirectReferencesList();
            HashSet<string> inderectReferencesList = new
            (
                inderectReferences.Split
                (
                    Environment.NewLine.ToCharArray(), 
                    StringSplitOptions.RemoveEmptyEntries
                )
            );

            //assert
            Assert.Contains("StringKeyValue", inderectReferencesList);
            Assert.Contains("IntKeyValue", inderectReferencesList);
            Assert.Contains("Field", inderectReferencesList);
            Assert.Contains("Property", inderectReferencesList);
        }

        [Fact]
        public void GetValidIndirectReferencesListReturnsExpectedStringForSatelliteCulture()
        {
            //arrange
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            string inderectReferences = enumHelper.GetValidIndirectReferencesList();
            HashSet<string> inderectReferencesList = new
            (
                inderectReferences.Split
                (
                    Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries
                )
            );

            //assert
            Assert.Contains("StringKeyValueFR", inderectReferencesList);
            Assert.Contains("IntKeyValueFR", inderectReferencesList);
            Assert.Contains("FieldFR", inderectReferencesList);
            Assert.Contains("PropertyFR", inderectReferencesList);
        }

        [Fact]
        public void GetValidCategoriesListReturnsExpectedStringForDefaultCulture()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            string referenceCategories = enumHelper.GetValidCategoriesList();
            HashSet<string> referenceCategorieList = new
            (
                referenceCategories.Split
                (
                    Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries
                )
            );

            //assert
            Assert.Contains("Instance Reference", referenceCategorieList);
            Assert.Contains("Static Reference", referenceCategorieList);
            Assert.Contains("Type", referenceCategorieList);
            Assert.Contains("This", referenceCategorieList);
            Assert.Contains("None", referenceCategorieList);
        }

        [Fact]
        public void GetValidCategoriesListReturnsExpectedStringForSatelliteCulture()
        {
            //arrange
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            string referenceCategories = enumHelper.GetValidCategoriesList();
            HashSet<string> referenceCategorieList = new
            (
                referenceCategories.Split
                (
                    Environment.NewLine.ToCharArray(),
                    StringSplitOptions.RemoveEmptyEntries
                )
            );

            //assert
            Assert.Contains("Instance ReferenceFR", referenceCategorieList);
            Assert.Contains("Static ReferenceFR", referenceCategorieList);
            Assert.Contains("TypeFR", referenceCategorieList);
            Assert.Contains("ThisFR", referenceCategorieList);
            Assert.Contains("NoneFR", referenceCategorieList);
        }

        [Fact]
        public void BuildValidReferenceDefinitionReturnsCorrectStringForDefaultCulture()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            string result = enumHelper.BuildValidReferenceDefinition("Field.Property.IntKeyValue");

            //assert
            Assert.Equal("Field.Property.IntegerKeyIndexer", result);
        }

        [Fact]
        public void BuildValidReferenceDefinitionReturnsCorrectStringForSatelliteCulture()
        {
            //arrange
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr");
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            string result = enumHelper.BuildValidReferenceDefinition("FieldFR.PropertyFR.IntKeyValueFR");

            //assert
            Assert.Equal("Field.Property.IntegerKeyIndexer", result);
        }

        [Theory]
        [InlineData(XmlDataConstants.LITERALPARAMETERELEMENT, GenericConfigCategory.Literal)]
        [InlineData(XmlDataConstants.OBJECTPARAMETERELEMENT, GenericConfigCategory.Object)]
        [InlineData(XmlDataConstants.LITERALLISTPARAMETERELEMENT, GenericConfigCategory.LiteralList)]
        [InlineData(XmlDataConstants.OBJECTLISTPARAMETERELEMENT, GenericConfigCategory.ObjectList)]
        internal void GetGenericConfigCategoryReturnsTheExpectedCategory(string elementName, GenericConfigCategory expectedCategory)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var category = enumHelper.GetGenericConfigCategory(elementName);

            //assert
            Assert.Equal(expectedCategory, category);
        }

        [Fact]
        public void GetGenericConfigCategoryThrowsCriticalExceptionForInvalidElementName()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetGenericConfigCategory("xyz"));
        }

        [Theory]
        [InlineData(typeof(bool), ValidIndirectReference.BooleanKeyIndexer)]
        [InlineData(typeof(DateTimeOffset), ValidIndirectReference.DateTimeOffsetKeyIndexer)]
        [InlineData(typeof(DateOnly), ValidIndirectReference.DateOnlyKeyIndexer)]
        [InlineData(typeof(DateTime), ValidIndirectReference.DateTimeKeyIndexer)]
        [InlineData(typeof(Date), ValidIndirectReference.DateKeyIndexer)]
        [InlineData(typeof(TimeSpan), ValidIndirectReference.TimeSpanKeyIndexer)]
        [InlineData(typeof(TimeOnly), ValidIndirectReference.TimeOnlyKeyIndexer)]
        [InlineData(typeof(TimeOfDay), ValidIndirectReference.TimeOfDayKeyIndexer)]
        [InlineData(typeof(Guid), ValidIndirectReference.GuidKeyIndexer)]
        [InlineData(typeof(decimal), ValidIndirectReference.DecimalKeyIndexer)]
        [InlineData(typeof(byte), ValidIndirectReference.ByteKeyIndexer)]
        [InlineData(typeof(short), ValidIndirectReference.ShortKeyIndexer)]
        [InlineData(typeof(int), ValidIndirectReference.IntegerKeyIndexer)]
        [InlineData(typeof(long), ValidIndirectReference.LongKeyIndexer)]
        [InlineData(typeof(float), ValidIndirectReference.FloatKeyIndexer)]
        [InlineData(typeof(double), ValidIndirectReference.DoubleKeyIndexer)]
        [InlineData(typeof(char), ValidIndirectReference.CharKeyIndexer)]
        [InlineData(typeof(sbyte), ValidIndirectReference.SByteKeyIndexer)]
        [InlineData(typeof(ushort), ValidIndirectReference.UShortKeyIndexer)]
        [InlineData(typeof(uint), ValidIndirectReference.UIntegerKeyIndexer)]
        [InlineData(typeof(ulong), ValidIndirectReference.ULongKeyIndexer)]
        [InlineData(typeof(string), ValidIndirectReference.StringKeyIndexer)]
        internal void GetIndexReferenceDefinitionReturnsTheExpectedEnumType(Type type, ValidIndirectReference expectedEnumType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var literalType = enumHelper.GetIndexReferenceDefinition(type);

            //assert
            Assert.Equal(expectedEnumType, literalType);
        }

        [Fact]
        public void GetIndexReferenceDefinitionThrowsCriticalExceptionForInvalidType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetIndexReferenceDefinition(typeof(void)));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{C0115382-3B85-4FCD-AF3A-15F9991D65E3}"),
                exception.Message
            );
        }

        [Theory]
        [InlineData(typeof(bool), VariableCategory.BooleanKeyIndexer)]
        [InlineData(typeof(DateTimeOffset), VariableCategory.DateTimeOffsetKeyIndexer)]
        [InlineData(typeof(DateOnly), VariableCategory.DateOnlyKeyIndexer)]
        [InlineData(typeof(DateTime), VariableCategory.DateTimeKeyIndexer)]
        [InlineData(typeof(Date), VariableCategory.DateKeyIndexer)]
        [InlineData(typeof(TimeSpan), VariableCategory.TimeSpanKeyIndexer)]
        [InlineData(typeof(TimeOnly), VariableCategory.TimeOnlyKeyIndexer)]
        [InlineData(typeof(TimeOfDay), VariableCategory.TimeOfDayKeyIndexer)]
        [InlineData(typeof(Guid), VariableCategory.GuidKeyIndexer)]
        [InlineData(typeof(decimal), VariableCategory.DecimalKeyIndexer)]
        [InlineData(typeof(byte), VariableCategory.ByteKeyIndexer)]
        [InlineData(typeof(short), VariableCategory.ShortKeyIndexer)]
        [InlineData(typeof(int), VariableCategory.IntegerKeyIndexer)]
        [InlineData(typeof(long), VariableCategory.LongKeyIndexer)]
        [InlineData(typeof(float), VariableCategory.FloatKeyIndexer)]
        [InlineData(typeof(double), VariableCategory.DoubleKeyIndexer)]
        [InlineData(typeof(char), VariableCategory.CharKeyIndexer)]
        [InlineData(typeof(sbyte), VariableCategory.SByteKeyIndexer)]
        [InlineData(typeof(ushort), VariableCategory.UShortKeyIndexer)]
        [InlineData(typeof(uint), VariableCategory.UIntegerKeyIndexer)]
        [InlineData(typeof(ulong), VariableCategory.ULongKeyIndexer)]
        [InlineData(typeof(string), VariableCategory.StringKeyIndexer)]
        internal void GetIndexVariableCategoryReturnsTheExpectedEnumType(Type type, VariableCategory expectedEnumType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var literalType = enumHelper.GetIndexVariableCategory(type);

            //assert
            Assert.Equal(expectedEnumType, literalType);
        }

        [Fact]
        public void GetIndexVariableCategoryThrowsCriticalExceptionForInvalidType()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetIndexVariableCategory(typeof(void)));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{DEDB5B31-31D0-4BB3-9D50-EA29A908D948}"),
                exception.Message
            );
        }

        [Theory]
        [InlineData(VariableCategory.Field, ValidIndirectReference.Field)]
        [InlineData(VariableCategory.Property, ValidIndirectReference.Property)]
        [InlineData(VariableCategory.ArrayIndexer, ValidIndirectReference.ArrayIndexer)]
        [InlineData(VariableCategory.VariableKeyIndexer, ValidIndirectReference.VariableKeyIndexer)]
        [InlineData(VariableCategory.BooleanKeyIndexer, ValidIndirectReference.BooleanKeyIndexer)]
        [InlineData(VariableCategory.VariableArrayIndexer, ValidIndirectReference.VariableArrayIndexer)]
        [InlineData(VariableCategory.DateTimeOffsetKeyIndexer, ValidIndirectReference.DateTimeOffsetKeyIndexer)]
        [InlineData(VariableCategory.DateOnlyKeyIndexer, ValidIndirectReference.DateOnlyKeyIndexer)]
        [InlineData(VariableCategory.DateTimeKeyIndexer, ValidIndirectReference.DateTimeKeyIndexer)]
        [InlineData(VariableCategory.DateKeyIndexer, ValidIndirectReference.DateKeyIndexer)]
        [InlineData(VariableCategory.TimeSpanKeyIndexer, ValidIndirectReference.TimeSpanKeyIndexer)]
        [InlineData(VariableCategory.TimeOnlyKeyIndexer, ValidIndirectReference.TimeOnlyKeyIndexer)]
        [InlineData(VariableCategory.TimeOfDayKeyIndexer, ValidIndirectReference.TimeOfDayKeyIndexer)]
        [InlineData(VariableCategory.GuidKeyIndexer, ValidIndirectReference.GuidKeyIndexer)]
        [InlineData(VariableCategory.DecimalKeyIndexer, ValidIndirectReference.DecimalKeyIndexer)]
        [InlineData(VariableCategory.ByteKeyIndexer, ValidIndirectReference.ByteKeyIndexer)]
        [InlineData(VariableCategory.ShortKeyIndexer, ValidIndirectReference.ShortKeyIndexer)]
        [InlineData(VariableCategory.IntegerKeyIndexer, ValidIndirectReference.IntegerKeyIndexer)]
        [InlineData(VariableCategory.LongKeyIndexer, ValidIndirectReference.LongKeyIndexer)]
        [InlineData(VariableCategory.FloatKeyIndexer, ValidIndirectReference.FloatKeyIndexer)]
        [InlineData(VariableCategory.DoubleKeyIndexer, ValidIndirectReference.DoubleKeyIndexer)]
        [InlineData(VariableCategory.CharKeyIndexer, ValidIndirectReference.CharKeyIndexer)]
        [InlineData(VariableCategory.SByteKeyIndexer, ValidIndirectReference.SByteKeyIndexer)]
        [InlineData(VariableCategory.UShortKeyIndexer, ValidIndirectReference.UShortKeyIndexer)]
        [InlineData(VariableCategory.UIntegerKeyIndexer, ValidIndirectReference.UIntegerKeyIndexer)]
        [InlineData(VariableCategory.ULongKeyIndexer, ValidIndirectReference.ULongKeyIndexer)]
        [InlineData(VariableCategory.StringKeyIndexer, ValidIndirectReference.StringKeyIndexer)]
        internal void GetValidIndirectReferenceReturnsTheExpectedEnumType(VariableCategory variableCategory, ValidIndirectReference expectedEnumType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetValidIndirectReference(variableCategory);

            //assert
            Assert.Equal(expectedEnumType, result);
        }

        [Fact]
        public void GetValidIndirectReferenceThrowsCriticalExceptionForInvalidVariableCategory()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetValidIndirectReference((VariableCategory)(-1)));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{48F6C59B-69D1-4658-B87B-6B2921538A8E}"),
                exception.Message
            );
        }

        [Theory]
        [InlineData(ValidIndirectReference.Field, VariableCategory.Field)]
        [InlineData(ValidIndirectReference.Property, VariableCategory.Property)]
        [InlineData(ValidIndirectReference.ArrayIndexer, VariableCategory.ArrayIndexer)]
        [InlineData(ValidIndirectReference.VariableKeyIndexer, VariableCategory.VariableKeyIndexer)]
        [InlineData(ValidIndirectReference.BooleanKeyIndexer, VariableCategory.BooleanKeyIndexer)]
        [InlineData(ValidIndirectReference.VariableArrayIndexer, VariableCategory.VariableArrayIndexer)]
        [InlineData(ValidIndirectReference.DateTimeOffsetKeyIndexer, VariableCategory.DateTimeOffsetKeyIndexer)]
        [InlineData(ValidIndirectReference.DateOnlyKeyIndexer, VariableCategory.DateOnlyKeyIndexer)]
        [InlineData(ValidIndirectReference.DateTimeKeyIndexer, VariableCategory.DateTimeKeyIndexer)]
        [InlineData(ValidIndirectReference.DateKeyIndexer, VariableCategory.DateKeyIndexer)]
        [InlineData(ValidIndirectReference.TimeSpanKeyIndexer, VariableCategory.TimeSpanKeyIndexer)]
        [InlineData(ValidIndirectReference.TimeOnlyKeyIndexer, VariableCategory.TimeOnlyKeyIndexer)]
        [InlineData(ValidIndirectReference.TimeOfDayKeyIndexer, VariableCategory.TimeOfDayKeyIndexer)]
        [InlineData(ValidIndirectReference.GuidKeyIndexer, VariableCategory.GuidKeyIndexer)]
        [InlineData(ValidIndirectReference.DecimalKeyIndexer, VariableCategory.DecimalKeyIndexer)]
        [InlineData(ValidIndirectReference.ByteKeyIndexer, VariableCategory.ByteKeyIndexer)]
        [InlineData(ValidIndirectReference.ShortKeyIndexer, VariableCategory.ShortKeyIndexer)]
        [InlineData(ValidIndirectReference.IntegerKeyIndexer, VariableCategory.IntegerKeyIndexer)]
        [InlineData(ValidIndirectReference.LongKeyIndexer, VariableCategory.LongKeyIndexer)]
        [InlineData(ValidIndirectReference.FloatKeyIndexer, VariableCategory.FloatKeyIndexer)]
        [InlineData(ValidIndirectReference.DoubleKeyIndexer, VariableCategory.DoubleKeyIndexer)]
        [InlineData(ValidIndirectReference.CharKeyIndexer, VariableCategory.CharKeyIndexer)]
        [InlineData(ValidIndirectReference.SByteKeyIndexer, VariableCategory.SByteKeyIndexer)]
        [InlineData(ValidIndirectReference.UShortKeyIndexer, VariableCategory.UShortKeyIndexer)]
        [InlineData(ValidIndirectReference.UIntegerKeyIndexer, VariableCategory.UIntegerKeyIndexer)]
        [InlineData(ValidIndirectReference.ULongKeyIndexer, VariableCategory.ULongKeyIndexer)]
        [InlineData(ValidIndirectReference.StringKeyIndexer, VariableCategory.StringKeyIndexer)]
        internal void GetVariableCategoryReturnsTheExpectedEnumType(ValidIndirectReference validIndirectReference, VariableCategory expectedEnumType)
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var result = enumHelper.GetVariableCategory(validIndirectReference);

            //assert
            Assert.Equal(expectedEnumType, result);
        }

        [Fact]
        public void GetVariableCategoryThrowsCriticalExceptionForInvalidValidIndirectReference()
        {
            //arrange
            IEnumHelper enumHelper = serviceProvider.GetRequiredService<IEnumHelper>();

            //act
            var exception = Assert.Throws<CriticalLogicBuilderException>(() => enumHelper.GetVariableCategory((ValidIndirectReference)(-1)));
            Assert.Equal
            (
                string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{A23350F6-7EC0-402A-823C-DF83E79A161D}"),
                exception.Message
            );
        }
    }
}

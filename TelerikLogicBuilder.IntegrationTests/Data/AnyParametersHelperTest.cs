using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Xml;
using TelerikLogicBuilder.IntegrationTests.Constants;
using Xunit;
namespace TelerikLogicBuilder.IntegrationTests.Data
{
    public class AnyParametersHelperTest : IClassFixture<AnyParametersHelperFixture>
    {
        private readonly AnyParametersHelperFixture _fixture;

        public AnyParametersHelperTest(AnyParametersHelperFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void CanCreateAnyParametersHelper()
        {
            //act
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();

            //assert
            Assert.NotNull(helper);
        }

        [Theory]
        [InlineData(LiteralVariableType.Boolean, LiteralFunctionReturnType.Boolean, typeof(bool))]
        [InlineData(LiteralVariableType.Byte, LiteralFunctionReturnType.Byte, typeof(byte))]
        [InlineData(LiteralVariableType.Char, LiteralFunctionReturnType.Char, typeof(char))]
        [InlineData(LiteralVariableType.Date, LiteralFunctionReturnType.Date, typeof(Date))]
        [InlineData(LiteralVariableType.DateTime, LiteralFunctionReturnType.DateTime, typeof(DateTime))]
        [InlineData(LiteralVariableType.DateTimeOffset, LiteralFunctionReturnType.DateTimeOffset, typeof(DateTimeOffset))]
        [InlineData(LiteralVariableType.Decimal, LiteralFunctionReturnType.Decimal, typeof(decimal))]
        [InlineData(LiteralVariableType.Double, LiteralFunctionReturnType.Double, typeof(double))]
        [InlineData(LiteralVariableType.Float, LiteralFunctionReturnType.Float, typeof(float))]
        [InlineData(LiteralVariableType.Guid, LiteralFunctionReturnType.Guid, typeof(Guid))]
        [InlineData(LiteralVariableType.Integer, LiteralFunctionReturnType.Integer, typeof(int))]
        [InlineData(LiteralVariableType.Long, LiteralFunctionReturnType.Long, typeof(long))]
        [InlineData(LiteralVariableType.SByte, LiteralFunctionReturnType.SByte, typeof(sbyte))]
        [InlineData(LiteralVariableType.Short, LiteralFunctionReturnType.Short, typeof(short))]
        [InlineData(LiteralVariableType.String, LiteralFunctionReturnType.String, typeof(string))]
        [InlineData(LiteralVariableType.TimeOfDay, LiteralFunctionReturnType.TimeOfDay, typeof(TimeOfDay))]
        [InlineData(LiteralVariableType.TimeSpan, LiteralFunctionReturnType.TimeSpan, typeof(TimeSpan))]
        [InlineData(LiteralVariableType.UInteger, LiteralFunctionReturnType.UInteger, typeof(uint))]
        [InlineData(LiteralVariableType.ULong, LiteralFunctionReturnType.ULong, typeof(ulong))]
        [InlineData(LiteralVariableType.UShort, LiteralFunctionReturnType.UShort, typeof(ushort))]
        [InlineData(LiteralVariableType.NullableBoolean, LiteralFunctionReturnType.NullableBoolean, typeof(bool?))]
        [InlineData(LiteralVariableType.NullableByte, LiteralFunctionReturnType.NullableByte, typeof(byte?))]
        [InlineData(LiteralVariableType.NullableChar, LiteralFunctionReturnType.NullableChar, typeof(char?))]
        [InlineData(LiteralVariableType.NullableDate, LiteralFunctionReturnType.NullableDate, typeof(Date?))]
        [InlineData(LiteralVariableType.NullableDateTime, LiteralFunctionReturnType.NullableDateTime, typeof(DateTime?))]
        [InlineData(LiteralVariableType.NullableDateTimeOffset, LiteralFunctionReturnType.NullableDateTimeOffset, typeof(DateTimeOffset?))]
        [InlineData(LiteralVariableType.NullableDecimal, LiteralFunctionReturnType.NullableDecimal, typeof(decimal?))]
        [InlineData(LiteralVariableType.NullableDouble, LiteralFunctionReturnType.NullableDouble, typeof(double?))]
        [InlineData(LiteralVariableType.NullableFloat, LiteralFunctionReturnType.NullableFloat, typeof(float?))]
        [InlineData(LiteralVariableType.NullableGuid, LiteralFunctionReturnType.NullableGuid, typeof(Guid?))]
        [InlineData(LiteralVariableType.NullableInteger, LiteralFunctionReturnType.NullableInteger, typeof(int?))]
        [InlineData(LiteralVariableType.NullableLong, LiteralFunctionReturnType.NullableLong, typeof(long?))]
        [InlineData(LiteralVariableType.NullableSByte, LiteralFunctionReturnType.NullableSByte, typeof(sbyte?))]
        [InlineData(LiteralVariableType.NullableShort, LiteralFunctionReturnType.NullableShort, typeof(short?))]
        [InlineData(LiteralVariableType.NullableTimeOfDay, LiteralFunctionReturnType.NullableTimeOfDay, typeof(TimeOfDay?))]
        [InlineData(LiteralVariableType.NullableTimeSpan, LiteralFunctionReturnType.NullableTimeSpan, typeof(TimeSpan?))]
        [InlineData(LiteralVariableType.NullableUInteger, LiteralFunctionReturnType.NullableUInteger, typeof(uint?))]
        [InlineData(LiteralVariableType.NullableULong, LiteralFunctionReturnType.NullableULong, typeof(ulong?))]
        [InlineData(LiteralVariableType.NullableUShort, LiteralFunctionReturnType.NullableUShort, typeof(ushort?))]
        internal void ReturnsTheExpectedTypeWithBothItemsElementsOfTheSameType(LiteralVariableType literalVariableType, LiteralFunctionReturnType functionReturnType, Type expectedLiteralType)
        {
            //arrange
            string variableName = $"{Enum.GetName(typeof(LiteralVariableType), literalVariableType)}Item";
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1""><variable name=""{variableName}"" visibleText=""visibleText"" /></literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2""><function name=""StaticMethodReturnsGenericType"" visibleText=""StaticMethodReturnsGenericType"">
                                                          <genericArguments>
                                                            <literalParameter genericArgumentName=""A"">
                                                              <literalType>{Enum.GetName(typeof(LiteralFunctionReturnType), functionReturnType)}</literalType>
                                                              <control>SingleLineTextBox</control>
                                                              <useForEquality>true</useForEquality>
                                                              <useForHashCode>false</useForHashCode>
                                                              <useForToString>true</useForToString>
                                                              <propertySource />
                                                              <propertySourceParameter />
                                                              <defaultValue />
                                                              <domain />
                                                            </literalParameter>
                                                          </genericArguments>
                                                          <parameters />
                                                        </function></literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(expectedLiteralType, pair.ParameterOneType);
            Assert.Equal(expectedLiteralType, pair.ParameterTwoType);
        }

        [Theory]
        [InlineData(LiteralVariableType.Boolean, "true", typeof(bool), typeof(bool))]
        [InlineData(LiteralVariableType.Boolean, "zzz", typeof(bool), typeof(string))]
        [InlineData(LiteralVariableType.Byte, "255", typeof(byte), typeof(byte))]
        [InlineData(LiteralVariableType.Byte, "32767", typeof(byte), typeof(short))]
        [InlineData(LiteralVariableType.Byte, "65535", typeof(byte), typeof(ushort))]
        [InlineData(LiteralVariableType.Byte, "2147483647", typeof(byte), typeof(int))]
        [InlineData(LiteralVariableType.Byte, "4294967295", typeof(byte), typeof(uint))]
        [InlineData(LiteralVariableType.Byte, "9223372036854775807", typeof(byte), typeof(long))]
        [InlineData(LiteralVariableType.Byte, "18446744073709551615", typeof(byte), typeof(ulong))]
        [InlineData(LiteralVariableType.Byte, "1.7976931348623157E+308", typeof(byte), typeof(double))]
        [InlineData(LiteralVariableType.Byte, "zzz", typeof(byte), typeof(string))]
        [InlineData(LiteralVariableType.Char, "\u2713", typeof(char), typeof(char))]
        [InlineData(LiteralVariableType.Char, "zzz", typeof(char), typeof(string))]
        [InlineData(LiteralVariableType.Date, "2020-10-10", typeof(Date), typeof(Date))]
        [InlineData(LiteralVariableType.Date, "zzz", typeof(Date), typeof(string))]
        [InlineData(LiteralVariableType.DateTime, "2020-10-10 13:13:13", typeof(DateTime), typeof(DateTime))]
        [InlineData(LiteralVariableType.DateTime, "zzz", typeof(DateTime), typeof(string))]
        [InlineData(LiteralVariableType.DateTimeOffset, "2020-10-10 13:13:13 +01:00", typeof(DateTimeOffset), typeof(DateTimeOffset))]
        [InlineData(LiteralVariableType.DateTimeOffset, "zzz", typeof(DateTimeOffset), typeof(string))]
        [InlineData(LiteralVariableType.Decimal, "79228162514264337593543950335", typeof(decimal), typeof(decimal))]
        [InlineData(LiteralVariableType.Decimal, "zzz", typeof(decimal), typeof(string))]
        [InlineData(LiteralVariableType.Double, "1.7976931348623157E+308", typeof(double), typeof(double))]
        [InlineData(LiteralVariableType.Double, "zzz", typeof(double), typeof(string))]
        [InlineData(LiteralVariableType.Float, "3.40282347E+38", typeof(float), typeof(float))]
        [InlineData(LiteralVariableType.Float, "zzz", typeof(float), typeof(string))]
        [InlineData(LiteralVariableType.Guid, "{2D64191A-C055-4E41-BF86-3781D775FA97}", typeof(Guid), typeof(Guid))]
        [InlineData(LiteralVariableType.Guid, "zzz", typeof(Guid), typeof(string))]
        [InlineData(LiteralVariableType.Integer, "2147483647", typeof(int), typeof(int))]
        [InlineData(LiteralVariableType.Integer, "9223372036854775807", typeof(int), typeof(long))]
        [InlineData(LiteralVariableType.Integer, "1.7976931348623157E+308", typeof(int), typeof(double))]
        [InlineData(LiteralVariableType.Integer, "zzz", typeof(int), typeof(string))]
        [InlineData(LiteralVariableType.Long, "9223372036854775807", typeof(long), typeof(long))]
        [InlineData(LiteralVariableType.Long, "1.7976931348623157E+308", typeof(long), typeof(double))]
        [InlineData(LiteralVariableType.Long, "zzz", typeof(long), typeof(string))]
        [InlineData(LiteralVariableType.SByte, "127", typeof(sbyte), typeof(sbyte))]
        [InlineData(LiteralVariableType.SByte, "32767", typeof(sbyte), typeof(short))]
        [InlineData(LiteralVariableType.SByte, "2147483647", typeof(sbyte), typeof(int))]
        [InlineData(LiteralVariableType.SByte, "9223372036854775807", typeof(sbyte), typeof(long))]
        [InlineData(LiteralVariableType.SByte, "1.7976931348623157E+308", typeof(sbyte), typeof(double))]
        [InlineData(LiteralVariableType.SByte, "zzz", typeof(sbyte), typeof(string))]
        [InlineData(LiteralVariableType.Short, "32767", typeof(short), typeof(short))]
        [InlineData(LiteralVariableType.Short, "2147483647", typeof(short), typeof(int))]
        [InlineData(LiteralVariableType.Short, "9223372036854775807", typeof(short), typeof(long))]
        [InlineData(LiteralVariableType.Short, "1.7976931348623157E+308", typeof(short), typeof(double))]
        [InlineData(LiteralVariableType.Short, "zzz", typeof(short), typeof(string))]
        [InlineData(LiteralVariableType.String, "zzz", typeof(string), typeof(string))]
        [InlineData(LiteralVariableType.TimeOfDay, "13:13:13", typeof(TimeOfDay), typeof(TimeOfDay))]
        [InlineData(LiteralVariableType.TimeOfDay, "zzz", typeof(TimeOfDay), typeof(string))]
        [InlineData(LiteralVariableType.TimeSpan, "13:13:13", typeof(TimeSpan), typeof(TimeSpan))]
        [InlineData(LiteralVariableType.TimeSpan, "zzz", typeof(TimeSpan), typeof(string))]
        [InlineData(LiteralVariableType.UInteger, "4294967295", typeof(uint), typeof(uint))]
        [InlineData(LiteralVariableType.UInteger, "9223372036854775807", typeof(uint), typeof(long))]
        [InlineData(LiteralVariableType.UInteger, "18446744073709551615", typeof(uint), typeof(ulong))]
        [InlineData(LiteralVariableType.UInteger, "1.7976931348623157E+308", typeof(uint), typeof(double))]
        [InlineData(LiteralVariableType.UInteger, "zzz", typeof(uint), typeof(string))]
        [InlineData(LiteralVariableType.ULong, "18446744073709551615", typeof(ulong), typeof(ulong))]
        [InlineData(LiteralVariableType.ULong, "1.7976931348623157E+308", typeof(ulong), typeof(double))]
        [InlineData(LiteralVariableType.ULong, "zzz", typeof(ulong), typeof(string))]
        [InlineData(LiteralVariableType.UShort, "65535", typeof(ushort), typeof(ushort))]
        [InlineData(LiteralVariableType.UShort, "2147483647", typeof(ushort), typeof(int))]
        [InlineData(LiteralVariableType.UShort, "4294967295", typeof(ushort), typeof(uint))]
        [InlineData(LiteralVariableType.UShort, "9223372036854775807", typeof(ushort), typeof(long))]
        [InlineData(LiteralVariableType.UShort, "18446744073709551615", typeof(ushort), typeof(ulong))]
        [InlineData(LiteralVariableType.UShort, "1.7976931348623157E+308", typeof(ushort), typeof(double))]
        [InlineData(LiteralVariableType.UShort, "zzz", typeof(ushort), typeof(string))]
        [InlineData(LiteralVariableType.NullableBoolean, "true", typeof(bool?), typeof(bool))]
        [InlineData(LiteralVariableType.NullableBoolean, "zzz", typeof(bool?), typeof(string))]
        [InlineData(LiteralVariableType.NullableByte, "255", typeof(byte?), typeof(byte))]
        [InlineData(LiteralVariableType.NullableByte, "32767", typeof(byte?), typeof(short))]
        [InlineData(LiteralVariableType.NullableByte, "65535", typeof(byte?), typeof(ushort))]
        [InlineData(LiteralVariableType.NullableByte, "2147483647", typeof(byte?), typeof(int))]
        [InlineData(LiteralVariableType.NullableByte, "4294967295", typeof(byte?), typeof(uint))]
        [InlineData(LiteralVariableType.NullableByte, "9223372036854775807", typeof(byte?), typeof(long))]
        [InlineData(LiteralVariableType.NullableByte, "18446744073709551615", typeof(byte?), typeof(ulong))]
        [InlineData(LiteralVariableType.NullableByte, "1.7976931348623157E+308", typeof(byte?), typeof(double))]
        [InlineData(LiteralVariableType.NullableByte, "zzz", typeof(byte?), typeof(string))]
        [InlineData(LiteralVariableType.NullableChar, "\u2713", typeof(char?), typeof(char))]
        [InlineData(LiteralVariableType.NullableChar, "zzz", typeof(char?), typeof(string))]
        [InlineData(LiteralVariableType.NullableDate, "2020-10-10", typeof(Date?), typeof(Date))]
        [InlineData(LiteralVariableType.NullableDate, "zzz", typeof(Date?), typeof(string))]
        [InlineData(LiteralVariableType.NullableDateTime, "2020-10-10 13:13:13", typeof(DateTime?), typeof(DateTime))]
        [InlineData(LiteralVariableType.NullableDateTime, "zzz", typeof(DateTime?), typeof(string))]
        [InlineData(LiteralVariableType.NullableDateTimeOffset, "2020-10-10 13:13:13 +01:00", typeof(DateTimeOffset?), typeof(DateTimeOffset))]
        [InlineData(LiteralVariableType.NullableDateTimeOffset, "zzz", typeof(DateTimeOffset?), typeof(string))]
        [InlineData(LiteralVariableType.NullableDecimal, "79228162514264337593543950335", typeof(decimal?), typeof(decimal))]
        [InlineData(LiteralVariableType.NullableDecimal, "zzz", typeof(decimal?), typeof(string))]
        [InlineData(LiteralVariableType.NullableDouble, "1.7976931348623157E+308", typeof(double?), typeof(double))]
        [InlineData(LiteralVariableType.NullableDouble, "zzz", typeof(double?), typeof(string))]
        [InlineData(LiteralVariableType.NullableFloat, "3.40282347E+38", typeof(float?), typeof(float))]
        [InlineData(LiteralVariableType.NullableFloat, "zzz", typeof(float?), typeof(string))]
        [InlineData(LiteralVariableType.NullableGuid, "{2D64191A-C055-4E41-BF86-3781D775FA97}", typeof(Guid?), typeof(Guid))]
        [InlineData(LiteralVariableType.NullableGuid, "zzz", typeof(Guid?), typeof(string))]
        [InlineData(LiteralVariableType.NullableInteger, "2147483647", typeof(int?), typeof(int))]
        [InlineData(LiteralVariableType.NullableInteger, "9223372036854775807", typeof(int?), typeof(long))]
        [InlineData(LiteralVariableType.NullableInteger, "1.7976931348623157E+308", typeof(int?), typeof(double))]
        [InlineData(LiteralVariableType.NullableInteger, "zzz", typeof(int?), typeof(string))]
        [InlineData(LiteralVariableType.NullableLong, "9223372036854775807", typeof(long?), typeof(long))]
        [InlineData(LiteralVariableType.NullableLong, "1.7976931348623157E+308", typeof(long?), typeof(double))]
        [InlineData(LiteralVariableType.NullableLong, "zzz", typeof(long?), typeof(string))]
        [InlineData(LiteralVariableType.NullableSByte, "127", typeof(sbyte?), typeof(sbyte))]
        [InlineData(LiteralVariableType.NullableSByte, "32767", typeof(sbyte?), typeof(short))]
        [InlineData(LiteralVariableType.NullableSByte, "2147483647", typeof(sbyte?), typeof(int))]
        [InlineData(LiteralVariableType.NullableSByte, "9223372036854775807", typeof(sbyte?), typeof(long))]
        [InlineData(LiteralVariableType.NullableSByte, "1.7976931348623157E+308", typeof(sbyte?), typeof(double))]
        [InlineData(LiteralVariableType.NullableSByte, "zzz", typeof(sbyte?), typeof(string))]
        [InlineData(LiteralVariableType.NullableShort, "32767", typeof(short?), typeof(short))]
        [InlineData(LiteralVariableType.NullableShort, "2147483647", typeof(short?), typeof(int))]
        [InlineData(LiteralVariableType.NullableShort, "9223372036854775807", typeof(short?), typeof(long))]
        [InlineData(LiteralVariableType.NullableShort, "1.7976931348623157E+308", typeof(short?), typeof(double))]
        [InlineData(LiteralVariableType.NullableShort, "zzz", typeof(short?), typeof(string))]
        [InlineData(LiteralVariableType.NullableTimeOfDay, "13:13:13", typeof(TimeOfDay?), typeof(TimeOfDay))]
        [InlineData(LiteralVariableType.NullableTimeOfDay, "zzz", typeof(TimeOfDay?), typeof(string))]
        [InlineData(LiteralVariableType.NullableTimeSpan, "13:13:13", typeof(TimeSpan?), typeof(TimeSpan))]
        [InlineData(LiteralVariableType.NullableTimeSpan, "zzz", typeof(TimeSpan?), typeof(string))]
        [InlineData(LiteralVariableType.NullableUInteger, "4294967295", typeof(uint?), typeof(uint))]
        [InlineData(LiteralVariableType.NullableUInteger, "9223372036854775807", typeof(uint?), typeof(long))]
        [InlineData(LiteralVariableType.NullableUInteger, "18446744073709551615", typeof(uint?), typeof(ulong))]
        [InlineData(LiteralVariableType.NullableUInteger, "1.7976931348623157E+308", typeof(uint?), typeof(double))]
        [InlineData(LiteralVariableType.NullableUInteger, "zzz", typeof(uint?), typeof(string))]
        [InlineData(LiteralVariableType.NullableULong, "18446744073709551615", typeof(ulong?), typeof(ulong))]
        [InlineData(LiteralVariableType.NullableULong, "1.7976931348623157E+308", typeof(ulong?), typeof(double))]
        [InlineData(LiteralVariableType.NullableULong, "zzz", typeof(ulong?), typeof(string))]
        [InlineData(LiteralVariableType.NullableUShort, "65535", typeof(ushort?), typeof(ushort))]
        [InlineData(LiteralVariableType.NullableUShort, "2147483647", typeof(ushort?), typeof(int))]
        [InlineData(LiteralVariableType.NullableUShort, "4294967295", typeof(ushort?), typeof(uint))]
        [InlineData(LiteralVariableType.NullableUShort, "9223372036854775807", typeof(ushort?), typeof(long))]
        [InlineData(LiteralVariableType.NullableUShort, "18446744073709551615", typeof(ushort?), typeof(ulong))]
        [InlineData(LiteralVariableType.NullableUShort, "1.7976931348623157E+308", typeof(ushort?), typeof(double))]
        [InlineData(LiteralVariableType.NullableUShort, "zzz", typeof(ushort?), typeof(string))]
        internal void ReturnsTheExpectedTypeWithItemOneElementAndItemTwoText(LiteralVariableType elementOneVariableType, string elementTwoText, Type expectedElementOneType, Type expectedElementTwoType)
        {
            //arrange
            string elementOneVariableName = $"{Enum.GetName(typeof(LiteralVariableType), elementOneVariableType)}Item";
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1""><variable name=""{elementOneVariableName}"" visibleText=""visibleText"" /></literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2"">{elementTwoText}</literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(expectedElementOneType, pair.ParameterOneType);
            Assert.Equal(expectedElementTwoType, pair.ParameterTwoType);
        }

        [Theory]
        [InlineData("true", LiteralVariableType.Boolean, typeof(bool), typeof(bool))]
        [InlineData("zzz", LiteralVariableType.Boolean, typeof(string), typeof(bool))]
        [InlineData("255", LiteralVariableType.Byte, typeof(byte), typeof(byte))]
        [InlineData("32767", LiteralVariableType.Byte, typeof(short), typeof(byte))]
        [InlineData("65535", LiteralVariableType.Byte, typeof(ushort), typeof(byte))]
        [InlineData("2147483647", LiteralVariableType.Byte, typeof(int), typeof(byte))]
        [InlineData("4294967295", LiteralVariableType.Byte, typeof(uint), typeof(byte))]
        [InlineData("9223372036854775807", LiteralVariableType.Byte, typeof(long), typeof(byte))]
        [InlineData("18446744073709551615", LiteralVariableType.Byte, typeof(ulong), typeof(byte))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.Byte, typeof(double), typeof(byte))]
        [InlineData("zzz", LiteralVariableType.Byte, typeof(string), typeof(byte))]
        [InlineData("\u2713", LiteralVariableType.Char, typeof(char), typeof(char))]
        [InlineData("zzz", LiteralVariableType.Char, typeof(string), typeof(char))]
        [InlineData("2020-10-10", LiteralVariableType.Date, typeof(Date), typeof(Date))]
        [InlineData("zzz", LiteralVariableType.Date, typeof(string), typeof(Date))]
        [InlineData("2020-10-10 13:13:13", LiteralVariableType.DateTime, typeof(DateTime), typeof(DateTime))]
        [InlineData("zzz", LiteralVariableType.DateTime, typeof(string), typeof(DateTime))]
        [InlineData("2020-10-10 13:13:13 +01:00", LiteralVariableType.DateTimeOffset, typeof(DateTimeOffset), typeof(DateTimeOffset))]
        [InlineData("zzz", LiteralVariableType.DateTimeOffset, typeof(string), typeof(DateTimeOffset))]
        [InlineData("79228162514264337593543950335", LiteralVariableType.Decimal, typeof(decimal), typeof(decimal))]
        [InlineData("zzz", LiteralVariableType.Decimal, typeof(string), typeof(decimal))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.Double, typeof(double), typeof(double))]
        [InlineData("zzz", LiteralVariableType.Double, typeof(string), typeof(double))]
        [InlineData("3.40282347E+38", LiteralVariableType.Float, typeof(float), typeof(float))]
        [InlineData("zzz", LiteralVariableType.Float, typeof(string), typeof(float))]
        [InlineData("{2D64191A-C055-4E41-BF86-3781D775FA97}", LiteralVariableType.Guid, typeof(Guid), typeof(Guid))]
        [InlineData("zzz", LiteralVariableType.Guid, typeof(string), typeof(Guid))]
        [InlineData("2147483647", LiteralVariableType.Integer, typeof(int), typeof(int))]
        [InlineData("9223372036854775807", LiteralVariableType.Integer, typeof(long), typeof(int))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.Integer, typeof(double), typeof(int))]
        [InlineData("zzz", LiteralVariableType.Integer, typeof(string), typeof(int))]
        [InlineData("9223372036854775807", LiteralVariableType.Long, typeof(long), typeof(long))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.Long, typeof(double), typeof(long))]
        [InlineData("zzz", LiteralVariableType.Long, typeof(string), typeof(long))]
        [InlineData("127", LiteralVariableType.SByte, typeof(sbyte), typeof(sbyte))]
        [InlineData("32767", LiteralVariableType.SByte, typeof(short), typeof(sbyte))]
        [InlineData("2147483647", LiteralVariableType.SByte, typeof(int), typeof(sbyte))]
        [InlineData("9223372036854775807", LiteralVariableType.SByte, typeof(long), typeof(sbyte))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.SByte, typeof(double), typeof(sbyte))]
        [InlineData("zzz", LiteralVariableType.SByte, typeof(string), typeof(sbyte))]
        [InlineData("32767", LiteralVariableType.Short, typeof(short), typeof(short))]
        [InlineData("2147483647", LiteralVariableType.Short, typeof(int), typeof(short))]
        [InlineData("9223372036854775807", LiteralVariableType.Short, typeof(long), typeof(short))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.Short, typeof(double), typeof(short))]
        [InlineData("zzz", LiteralVariableType.Short, typeof(string), typeof(short))]
        [InlineData("zzz", LiteralVariableType.String, typeof(string), typeof(string))]
        [InlineData("13:13:13", LiteralVariableType.TimeOfDay, typeof(TimeOfDay), typeof(TimeOfDay))]
        [InlineData("zzz", LiteralVariableType.TimeOfDay, typeof(string), typeof(TimeOfDay))]
        [InlineData("13:13:13", LiteralVariableType.TimeSpan, typeof(TimeSpan), typeof(TimeSpan))]
        [InlineData("zzz", LiteralVariableType.TimeSpan, typeof(string), typeof(TimeSpan))]
        [InlineData("4294967295", LiteralVariableType.UInteger, typeof(uint), typeof(uint))]
        [InlineData("9223372036854775807", LiteralVariableType.UInteger, typeof(long), typeof(uint))]
        [InlineData("18446744073709551615", LiteralVariableType.UInteger, typeof(ulong), typeof(uint))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.UInteger, typeof(double), typeof(uint))]
        [InlineData("zzz", LiteralVariableType.UInteger, typeof(string), typeof(uint))]
        [InlineData("18446744073709551615", LiteralVariableType.ULong, typeof(ulong), typeof(ulong))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.ULong, typeof(double), typeof(ulong))]
        [InlineData("zzz", LiteralVariableType.ULong, typeof(string), typeof(ulong))]
        [InlineData("65535", LiteralVariableType.UShort, typeof(ushort), typeof(ushort))]
        [InlineData("2147483647", LiteralVariableType.UShort, typeof(int), typeof(ushort))]
        [InlineData("4294967295", LiteralVariableType.UShort, typeof(uint), typeof(ushort))]
        [InlineData("9223372036854775807", LiteralVariableType.UShort, typeof(long), typeof(ushort))]
        [InlineData("18446744073709551615", LiteralVariableType.UShort, typeof(ulong), typeof(ushort))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.UShort, typeof(double), typeof(ushort))]
        [InlineData("zzz", LiteralVariableType.UShort, typeof(string), typeof(ushort))]
        [InlineData("true", LiteralVariableType.NullableBoolean, typeof(bool), typeof(bool?))]
        [InlineData("zzz", LiteralVariableType.NullableBoolean, typeof(string), typeof(bool?))]
        [InlineData("255", LiteralVariableType.NullableByte, typeof(byte), typeof(byte?))]
        [InlineData("32767", LiteralVariableType.NullableByte, typeof(short), typeof(byte?))]
        [InlineData("65535", LiteralVariableType.NullableByte, typeof(ushort), typeof(byte?))]
        [InlineData("2147483647", LiteralVariableType.NullableByte, typeof(int), typeof(byte?))]
        [InlineData("4294967295", LiteralVariableType.NullableByte, typeof(uint), typeof(byte?))]
        [InlineData("9223372036854775807", LiteralVariableType.NullableByte, typeof(long), typeof(byte?))]
        [InlineData("18446744073709551615", LiteralVariableType.NullableByte, typeof(ulong), typeof(byte?))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.NullableByte, typeof(double), typeof(byte?))]
        [InlineData("zzz", LiteralVariableType.NullableByte, typeof(string), typeof(byte?))]
        [InlineData("\u2713", LiteralVariableType.NullableChar, typeof(char), typeof(char?))]
        [InlineData("zzz", LiteralVariableType.NullableChar, typeof(string), typeof(char?))]
        [InlineData("2020-10-10", LiteralVariableType.NullableDate, typeof(Date), typeof(Date?))]
        [InlineData("zzz", LiteralVariableType.NullableDate, typeof(string), typeof(Date?))]
        [InlineData("2020-10-10 13:13:13", LiteralVariableType.NullableDateTime, typeof(DateTime), typeof(DateTime?))]
        [InlineData("zzz", LiteralVariableType.NullableDateTime, typeof(string), typeof(DateTime?))]
        [InlineData("2020-10-10 13:13:13 +01:00", LiteralVariableType.NullableDateTimeOffset, typeof(DateTimeOffset), typeof(DateTimeOffset?))]
        [InlineData("zzz", LiteralVariableType.NullableDateTimeOffset, typeof(string), typeof(DateTimeOffset?))]
        [InlineData("79228162514264337593543950335", LiteralVariableType.NullableDecimal, typeof(decimal), typeof(decimal?))]
        [InlineData("zzz", LiteralVariableType.NullableDecimal, typeof(string), typeof(decimal?))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.NullableDouble, typeof(double), typeof(double?))]
        [InlineData("zzz", LiteralVariableType.NullableDouble, typeof(string), typeof(double?))]
        [InlineData("3.40282347E+38", LiteralVariableType.NullableFloat, typeof(float), typeof(float?))]
        [InlineData("zzz", LiteralVariableType.NullableFloat, typeof(string), typeof(float?))]
        [InlineData("{2D64191A-C055-4E41-BF86-3781D775FA97}", LiteralVariableType.NullableGuid, typeof(Guid), typeof(Guid?))]
        [InlineData("zzz", LiteralVariableType.NullableGuid, typeof(string), typeof(Guid?))]
        [InlineData("2147483647", LiteralVariableType.NullableInteger, typeof(int), typeof(int?))]
        [InlineData("9223372036854775807", LiteralVariableType.NullableInteger, typeof(long), typeof(int?))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.NullableInteger, typeof(double), typeof(int?))]
        [InlineData("zzz", LiteralVariableType.NullableInteger, typeof(string), typeof(int?))]
        [InlineData("9223372036854775807", LiteralVariableType.NullableLong, typeof(long), typeof(long?))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.NullableLong, typeof(double), typeof(long?))]
        [InlineData("zzz", LiteralVariableType.NullableLong, typeof(string), typeof(long?))]
        [InlineData("127", LiteralVariableType.NullableSByte, typeof(sbyte), typeof(sbyte?))]
        [InlineData("32767", LiteralVariableType.NullableSByte, typeof(short), typeof(sbyte?))]
        [InlineData("2147483647", LiteralVariableType.NullableSByte, typeof(int), typeof(sbyte?))]
        [InlineData("9223372036854775807", LiteralVariableType.NullableSByte, typeof(long), typeof(sbyte?))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.NullableSByte, typeof(double), typeof(sbyte?))]
        [InlineData("zzz", LiteralVariableType.NullableSByte, typeof(string), typeof(sbyte?))]
        [InlineData("32767", LiteralVariableType.NullableShort, typeof(short), typeof(short?))]
        [InlineData("2147483647", LiteralVariableType.NullableShort, typeof(int), typeof(short?))]
        [InlineData("9223372036854775807", LiteralVariableType.NullableShort, typeof(long), typeof(short?))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.NullableShort, typeof(double), typeof(short?))]
        [InlineData("zzz", LiteralVariableType.NullableShort, typeof(string), typeof(short?))]
        [InlineData("13:13:13", LiteralVariableType.NullableTimeOfDay, typeof(TimeOfDay), typeof(TimeOfDay?))]
        [InlineData("zzz", LiteralVariableType.NullableTimeOfDay, typeof(string), typeof(TimeOfDay?))]
        [InlineData("13:13:13", LiteralVariableType.NullableTimeSpan, typeof(TimeSpan), typeof(TimeSpan?))]
        [InlineData("zzz", LiteralVariableType.NullableTimeSpan, typeof(string), typeof(TimeSpan?))]
        [InlineData("4294967295", LiteralVariableType.NullableUInteger, typeof(uint), typeof(uint?))]
        [InlineData("9223372036854775807", LiteralVariableType.NullableUInteger, typeof(long), typeof(uint?))]
        [InlineData("18446744073709551615", LiteralVariableType.NullableUInteger, typeof(ulong), typeof(uint?))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.NullableUInteger, typeof(double), typeof(uint?))]
        [InlineData("zzz", LiteralVariableType.NullableUInteger, typeof(string), typeof(uint?))]
        [InlineData("18446744073709551615", LiteralVariableType.NullableULong, typeof(ulong), typeof(ulong?))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.NullableULong, typeof(double), typeof(ulong?))]
        [InlineData("zzz", LiteralVariableType.NullableULong, typeof(string), typeof(ulong?))]
        [InlineData("65535", LiteralVariableType.NullableUShort, typeof(ushort), typeof(ushort?))]
        [InlineData("2147483647", LiteralVariableType.NullableUShort, typeof(int), typeof(ushort?))]
        [InlineData("4294967295", LiteralVariableType.NullableUShort, typeof(uint), typeof(ushort?))]
        [InlineData("9223372036854775807", LiteralVariableType.NullableUShort, typeof(long), typeof(ushort?))]
        [InlineData("18446744073709551615", LiteralVariableType.NullableUShort, typeof(ulong), typeof(ushort?))]
        [InlineData("1.7976931348623157E+308", LiteralVariableType.NullableUShort, typeof(double), typeof(ushort?))]
        [InlineData("zzz", LiteralVariableType.NullableUShort, typeof(string), typeof(ushort?))]
        internal void ReturnsTheExpectedTypeWithItemOneTextAndItemTwoElement(string elementOneText, LiteralVariableType elementTwoVariableType, Type expectedElementOneType, Type expectedElementTwoType)
        {
            //arrange
            string elementTwoVariableName = $"{Enum.GetName(typeof(LiteralVariableType), elementTwoVariableType)}Item";
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1"">{elementOneText}</literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2""><variable name=""{elementTwoVariableName}"" visibleText=""visibleText"" /></literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(expectedElementOneType, pair.ParameterOneType);
            Assert.Equal(expectedElementTwoType, pair.ParameterTwoType);
        }

        [Theory]
        [InlineData(LiteralVariableType.Boolean, typeof(bool), typeof(bool?))]
        [InlineData(LiteralVariableType.Byte, typeof(byte), typeof(byte?))]
        [InlineData(LiteralVariableType.Char, typeof(char), typeof(char?))]
        [InlineData(LiteralVariableType.Date, typeof(Date), typeof(Date?))]
        [InlineData(LiteralVariableType.DateTime, typeof(DateTime), typeof(DateTime?))]
        [InlineData(LiteralVariableType.DateTimeOffset, typeof(DateTimeOffset), typeof(DateTimeOffset?))]
        [InlineData(LiteralVariableType.Decimal, typeof(decimal), typeof(decimal?))]
        [InlineData(LiteralVariableType.Double, typeof(double), typeof(double?))]
        [InlineData(LiteralVariableType.Float, typeof(float), typeof(float?))]
        [InlineData(LiteralVariableType.Guid, typeof(Guid), typeof(Guid?))]
        [InlineData(LiteralVariableType.Integer, typeof(int), typeof(int?))]
        [InlineData(LiteralVariableType.Long, typeof(long), typeof(long?))]
        [InlineData(LiteralVariableType.SByte, typeof(sbyte), typeof(sbyte?))]
        [InlineData(LiteralVariableType.Short, typeof(short), typeof(short?))]
        [InlineData(LiteralVariableType.String, typeof(string), typeof(string))]
        [InlineData(LiteralVariableType.TimeOfDay, typeof(TimeOfDay), typeof(TimeOfDay?))]
        [InlineData(LiteralVariableType.TimeSpan, typeof(TimeSpan), typeof(TimeSpan?))]
        [InlineData(LiteralVariableType.UInteger, typeof(uint), typeof(uint?))]
        [InlineData(LiteralVariableType.ULong, typeof(ulong), typeof(ulong?))]
        [InlineData(LiteralVariableType.UShort, typeof(ushort), typeof(ushort?))]
        [InlineData(LiteralVariableType.NullableBoolean, typeof(bool?), typeof(bool?))]
        [InlineData(LiteralVariableType.NullableByte, typeof(byte?), typeof(byte?))]
        [InlineData(LiteralVariableType.NullableChar, typeof(char?), typeof(char?))]
        [InlineData(LiteralVariableType.NullableDate, typeof(Date?), typeof(Date?))]
        [InlineData(LiteralVariableType.NullableDateTime, typeof(DateTime?), typeof(DateTime?))]
        [InlineData(LiteralVariableType.NullableDateTimeOffset, typeof(DateTimeOffset?), typeof(DateTimeOffset?))]
        [InlineData(LiteralVariableType.NullableDecimal, typeof(decimal?), typeof(decimal?))]
        [InlineData(LiteralVariableType.NullableDouble, typeof(double?), typeof(double?))]
        [InlineData(LiteralVariableType.NullableFloat, typeof(float?), typeof(float?))]
        [InlineData(LiteralVariableType.NullableGuid, typeof(Guid?), typeof(Guid?))]
        [InlineData(LiteralVariableType.NullableInteger, typeof(int?), typeof(int?))]
        [InlineData(LiteralVariableType.NullableLong, typeof(long?), typeof(long?))]
        [InlineData(LiteralVariableType.NullableSByte, typeof(sbyte?), typeof(sbyte?))]
        [InlineData(LiteralVariableType.NullableShort, typeof(short?), typeof(short?))]
        [InlineData(LiteralVariableType.NullableTimeOfDay, typeof(TimeOfDay?), typeof(TimeOfDay?))]
        [InlineData(LiteralVariableType.NullableTimeSpan, typeof(TimeSpan?), typeof(TimeSpan?))]
        [InlineData(LiteralVariableType.NullableUInteger, typeof(uint?), typeof(uint?))]
        [InlineData(LiteralVariableType.NullableULong, typeof(ulong?), typeof(ulong?))]
        [InlineData(LiteralVariableType.NullableUShort, typeof(ushort?), typeof(ushort?))]
        internal void ReturnsTheExpectedTypeWithItemOneElementAndItemTwoNoChildNode(LiteralVariableType elementOneVariableType, Type expectedElementOneType, Type expectedElementTwoType)
        {
            //arrange
            string elementOneVariableName = $"{Enum.GetName(typeof(LiteralVariableType), elementOneVariableType)}Item";
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1""><variable name=""{elementOneVariableName}"" visibleText=""visibleText"" /></literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2""></literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(expectedElementOneType, pair.ParameterOneType);
            Assert.Equal(expectedElementTwoType, pair.ParameterTwoType);
        }

        [Theory]
        [InlineData(LiteralVariableType.Boolean, typeof(bool?), typeof(bool))]
        [InlineData(LiteralVariableType.Byte, typeof(byte?), typeof(byte))]
        [InlineData(LiteralVariableType.Char, typeof(char?), typeof(char))]
        [InlineData(LiteralVariableType.Date, typeof(Date?), typeof(Date))]
        [InlineData(LiteralVariableType.DateTime, typeof(DateTime?), typeof(DateTime))]
        [InlineData(LiteralVariableType.DateTimeOffset, typeof(DateTimeOffset?), typeof(DateTimeOffset))]
        [InlineData(LiteralVariableType.Decimal, typeof(decimal?), typeof(decimal))]
        [InlineData(LiteralVariableType.Double, typeof(double?), typeof(double))]
        [InlineData(LiteralVariableType.Float, typeof(float?), typeof(float))]
        [InlineData(LiteralVariableType.Guid, typeof(Guid?), typeof(Guid))]
        [InlineData(LiteralVariableType.Integer, typeof(int?), typeof(int))]
        [InlineData(LiteralVariableType.Long, typeof(long?), typeof(long))]
        [InlineData(LiteralVariableType.SByte, typeof(sbyte?), typeof(sbyte))]
        [InlineData(LiteralVariableType.Short, typeof(short?), typeof(short))]
        [InlineData(LiteralVariableType.String, typeof(string), typeof(string))]
        [InlineData(LiteralVariableType.TimeOfDay, typeof(TimeOfDay?), typeof(TimeOfDay))]
        [InlineData(LiteralVariableType.TimeSpan, typeof(TimeSpan?), typeof(TimeSpan))]
        [InlineData(LiteralVariableType.UInteger, typeof(uint?), typeof(uint))]
        [InlineData(LiteralVariableType.ULong, typeof(ulong?), typeof(ulong))]
        [InlineData(LiteralVariableType.UShort, typeof(ushort?), typeof(ushort))]
        [InlineData(LiteralVariableType.NullableBoolean, typeof(bool?), typeof(bool?))]
        [InlineData(LiteralVariableType.NullableByte, typeof(byte?), typeof(byte?))]
        [InlineData(LiteralVariableType.NullableChar, typeof(char?), typeof(char?))]
        [InlineData(LiteralVariableType.NullableDate, typeof(Date?), typeof(Date?))]
        [InlineData(LiteralVariableType.NullableDateTime, typeof(DateTime?), typeof(DateTime?))]
        [InlineData(LiteralVariableType.NullableDateTimeOffset, typeof(DateTimeOffset?), typeof(DateTimeOffset?))]
        [InlineData(LiteralVariableType.NullableDecimal, typeof(decimal?), typeof(decimal?))]
        [InlineData(LiteralVariableType.NullableDouble, typeof(double?), typeof(double?))]
        [InlineData(LiteralVariableType.NullableFloat, typeof(float?), typeof(float?))]
        [InlineData(LiteralVariableType.NullableGuid, typeof(Guid?), typeof(Guid?))]
        [InlineData(LiteralVariableType.NullableInteger, typeof(int?), typeof(int?))]
        [InlineData(LiteralVariableType.NullableLong, typeof(long?), typeof(long?))]
        [InlineData(LiteralVariableType.NullableSByte, typeof(sbyte?), typeof(sbyte?))]
        [InlineData(LiteralVariableType.NullableShort, typeof(short?), typeof(short?))]
        [InlineData(LiteralVariableType.NullableTimeOfDay, typeof(TimeOfDay?), typeof(TimeOfDay?))]
        [InlineData(LiteralVariableType.NullableTimeSpan, typeof(TimeSpan?), typeof(TimeSpan?))]
        [InlineData(LiteralVariableType.NullableUInteger, typeof(uint?), typeof(uint?))]
        [InlineData(LiteralVariableType.NullableULong, typeof(ulong?), typeof(ulong?))]
        [InlineData(LiteralVariableType.NullableUShort, typeof(ushort?), typeof(ushort?))]
        internal void ReturnsTheExpectedTypeWithItemOneNoChildNodeAndItemTwoElement(LiteralVariableType elementTwoVariableType, Type expectedElementOneType, Type expectedElementTwoType)
        {
            //arrange
            string elementTwoVariableName = $"{Enum.GetName(typeof(LiteralVariableType), elementTwoVariableType)}Item";
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1""></literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2""><variable name=""{elementTwoVariableName}"" visibleText=""visibleText"" /></literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(expectedElementOneType, pair.ParameterOneType);
            Assert.Equal(expectedElementTwoType, pair.ParameterTwoType);
        }

        [Theory]
        [InlineData(LiteralVariableType.Integer, LiteralFunctionReturnType.Short, typeof(int), typeof(short))]
        [InlineData(LiteralVariableType.DateTimeOffset, LiteralFunctionReturnType.String, typeof(DateTimeOffset), typeof(string))]
        [InlineData(LiteralVariableType.NullableInteger, LiteralFunctionReturnType.TimeSpan, typeof(int?), typeof(TimeSpan))]
        internal void ReturnsTheExpectedTypeWithBothItemsElementsOfTheDifferentTypes(LiteralVariableType elementOneVariableType, LiteralFunctionReturnType functionReturnType, Type expectedItemOneType, Type expectedItemTwoType)
        {
            //arrange
            string elementOneVariableName = $"{Enum.GetName(typeof(LiteralVariableType), elementOneVariableType)}Item";
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1""><variable name=""{elementOneVariableName}"" visibleText=""visibleText"" /></literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2""><function name=""StaticMethodReturnsGenericType"" visibleText=""StaticMethodReturnsGenericType"">
                                                          <genericArguments>
                                                            <literalParameter genericArgumentName=""A"">
                                                              <literalType>{Enum.GetName(typeof(LiteralFunctionReturnType), functionReturnType)}</literalType>
                                                              <control>SingleLineTextBox</control>
                                                              <useForEquality>true</useForEquality>
                                                              <useForHashCode>false</useForHashCode>
                                                              <useForToString>true</useForToString>
                                                              <propertySource />
                                                              <propertySourceParameter />
                                                              <defaultValue />
                                                              <domain />
                                                            </literalParameter>
                                                          </genericArguments>
                                                          <parameters />
                                                        </function></literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(expectedItemOneType, pair.ParameterOneType);
            Assert.Equal(expectedItemTwoType, pair.ParameterTwoType);
        }

        [Theory]
        [InlineData("true", "false", typeof(bool))]
        [InlineData("1", "255", typeof(byte))]
        [InlineData("\u2713", "\u2714", typeof(char))]
        [InlineData("2012-11-11T12:00:00.00Z", "2012-11-11T12:00:00.00Z", typeof(DateTime))]
        [InlineData("1.7976931348623157E+308", "1.7976931348623157E+308", typeof(double))]
        [InlineData("{2D64191A-C055-4E41-BF86-3781D775FA97}", "{2D64191A-C055-4E41-BF86-3781D775FA97}", typeof(Guid))]
        [InlineData("2147483647", "2147483647", typeof(int))]
        [InlineData("9223372036854775807", "9223372036854775807", typeof(long))]
        [InlineData("-127", "127", typeof(sbyte))]
        [InlineData("-32768", "32767", typeof(short))]
        [InlineData("13:13:13", "13:13:13", typeof(TimeSpan))]
        [InlineData("1", "4294967295", typeof(uint))]
        [InlineData("1", "18446744073709551615", typeof(ulong))]
        [InlineData("65535", "65535", typeof(ushort))]
        [InlineData("-32768", "3dfsdfsd", typeof(string))]
        public void ReturnsTheExpectedTypeWithBothItemsText(string elementOneText, string elementTwoText, Type expectedLiteralType)
        {
            //arrange
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1"">{elementOneText}</literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2"">{elementTwoText}</literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(expectedLiteralType, pair.ParameterOneType);
            Assert.Equal(expectedLiteralType, pair.ParameterTwoType);
        }

        [Theory]
        [InlineData("", "false", typeof(bool?))]
        [InlineData("", "255", typeof(byte?))]
        [InlineData("", "\u2714", typeof(char?))]
        [InlineData("", "2012-11-11T12:00:00.00Z", typeof(DateTime?))]
        [InlineData("", "1.7976931348623157E+308", typeof(double?))]
        [InlineData("", "{2D64191A-C055-4E41-BF86-3781D775FA97}", typeof(Guid?))]
        [InlineData("", "2147483647", typeof(int?))]
        [InlineData("", "9223372036854775807", typeof(long?))]
        [InlineData("", "-127", typeof(sbyte?))]
        [InlineData("", "32767", typeof(short?))]
        [InlineData("", "13:13:13", typeof(TimeSpan?))]
        [InlineData("", "4294967295", typeof(uint?))]
        [InlineData("", "18446744073709551615", typeof(ulong?))]
        [InlineData("", "65535", typeof(ushort?))]
        [InlineData("", "3dfsdfsd", typeof(string))]
        [InlineData("true", "", typeof(bool?))]
        [InlineData("255", "", typeof(byte?))]
        [InlineData("\u2713", "", typeof(char?))]
        [InlineData("2012-11-11T12:00:00.00Z", "", typeof(DateTime?))]
        [InlineData("1.7976931348623157E+308", "", typeof(double?))]
        [InlineData("{2D64191A-C055-4E41-BF86-3781D775FA97}", "", typeof(Guid?))]
        [InlineData("2147483647", "", typeof(int?))]
        [InlineData("9223372036854775807", "", typeof(long?))]
        [InlineData("-127", "", typeof(sbyte?))]
        [InlineData("-32768", "", typeof(short?))]
        [InlineData("13:13:13", "", typeof(TimeSpan?))]
        [InlineData("4294967295", "", typeof(uint?))]
        [InlineData("18446744073709551615", "", typeof(ulong?))]
        [InlineData("65535", "", typeof(ushort?))]
        [InlineData("3dfsdfsd", "", typeof(string))]
        public void ReturnsTheExpectedTypeWithBothItemsTextOrNoChildNodes(string elementOneText, string elementTwoText, Type expectedLiteralType)
        {
            //arrange
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1"">{elementOneText}</literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2"">{elementTwoText}</literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(expectedLiteralType, pair.ParameterOneType);
            Assert.Equal(expectedLiteralType, pair.ParameterTwoType);
        }

        [Fact]
        public void ItemOneHasMultipleChildNodes()
        {
            //arrange
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1"">
                                                            <variable name=""IntegerItem"" visibleText=""visibleText"" />
                                                            <variable name=""StringItem"" visibleText=""visibleText"" />
                                                        </literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2""></literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(typeof(string), pair.ParameterOneType);
            Assert.Equal(typeof(string), pair.ParameterTwoType);
        }

        [Fact]
        public void ItemTwoHasMultipleChildNodes()
        {
            //arrange
            IAnyParametersHelper helper = _fixture.ServiceProvider.GetRequiredService<IAnyParametersHelper>();
            XmlElement xmlElementOne = GetXmlElement(@$"<literalParameter name=""p1""></literalParameter>");
            XmlElement xmlElementTwo = GetXmlElement(@$"<literalParameter name=""p2"">
                                                            <variable name=""IntegerItem"" visibleText=""visibleText"" />
                                                            <variable name=""StringItem"" visibleText=""visibleText"" />
                                                        </literalParameter>");

            //act
            AnyParameterPair pair = helper.GetTypes(xmlElementOne, xmlElementTwo, _fixture.ApplicationTypeInfoManager.GetApplicationTypeInfo(_fixture.ConfigurationService.GetSelectedApplication().Name));

            //assert
            Assert.Equal(typeof(string), pair.ParameterOneType);
            Assert.Equal(typeof(string), pair.ParameterTwoType);
        }

        private static XmlElement GetXmlElement(string xmlString)
            => GetXmlDocument(xmlString).DocumentElement!;

        private static XmlDocument GetXmlDocument(string xmlString)
        {
            XmlDocument xmlDocument = new();
            xmlDocument.LoadXml(xmlString);
            return xmlDocument;
        }
    }

    public class AnyParametersHelperFixture : IDisposable
    {
        public AnyParametersHelperFixture()
        {
            ServiceProvider = ABIS.LogicBuilder.FlowBuilder.Program.ServiceCollection.BuildServiceProvider();
            ConfigurationService = ServiceProvider.GetRequiredService<IConfigurationService>();
            GenericParametersHelper = ServiceProvider.GetRequiredService<IGenericParametersHelper>();
            ContextProvider = ServiceProvider.GetRequiredService<IContextProvider>();
            AssemblyLoadContextService = ServiceProvider.GetRequiredService<IAssemblyLoadContextManager>();
            LoadContextSponsor = ServiceProvider.GetRequiredService<ILoadContextSponsor>();
            TypeLoadHelper = ServiceProvider.GetRequiredService<ITypeLoadHelper>();
            ApplicationTypeInfoManager = ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>();

            ConfigurationService.ProjectProperties = new ProjectProperties
            (
                "Contoso",
                @"C:\ProjectPath",
                new Dictionary<string, Application>
                {
                    ["app01"] = new Application
                    (
                        "App01",
                        "App01",
                        "Contoso.Test.Flow.dll",
                        $@"{TestFolders.TestAssembliesFolder}\Contoso.Test.Flow\bin\Debug\netstandard2.0",
                        ABIS.LogicBuilder.FlowBuilder.Enums.RuntimeType.NetCore,
                        new List<string>(),
                        "Contoso.Test.Flow.FlowActivity",
                        "",
                        "",
                        new List<string>(),
                        "",
                        "",
                        "",
                        "",
                        new List<string>(),
                        new WebApiDeployment("", "", "", "", ContextProvider),
                        ContextProvider
                    )
                },
                new HashSet<string>(),
                ContextProvider
            );

            ConfigurationService.ConstructorList = new ConstructorList
            (
                new Dictionary<string, Constructor>
                {
                    ["TestResponseA"] = new Constructor
                    (
                        "TestResponseA",
                        "Contoso.Test.Business.Responses.TestResponseA",
                        new List<ParameterBase>
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
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        "",
                        ContextProvider
                    ),
                    ["TestResponseB"] = new Constructor
                    (
                        "TestResponseB",
                        "Contoso.Test.Business.Responses.TestResponseB",
                        new List<ParameterBase>
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
                                ContextProvider
                            ),
                            new LiteralParameter
                            (
                                "intProperty",
                                false,
                                "",
                                LiteralParameterType.Integer,
                                LiteralParameterInputStyle.SingleLineTextBox,
                                true,
                                false,
                                true,
                                "",
                                "",
                                "",
                                new List<string>(),
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        "",
                        ContextProvider
                    ),
                    ["GenericResponse"] = new Constructor
                    (
                        "GenericResponse",
                        "Contoso.Test.Business.Responses.GenericResponse`2",
                        new List<ParameterBase>
                        {
                            new GenericParameter
                            (
                                "aProperty",
                                false,
                                "",
                                "A",
                                ContextProvider
                            ),
                            new GenericParameter
                            (
                                "bProperty",
                                false,
                                "",
                                "B",
                                ContextProvider
                            )
                        },
                        new List<string> { "A", "B" },
                        "",
                        ContextProvider
                    ),
                    ["TypeNotFoundConstructor"] = new Constructor
                    (
                        "TypeNotFoundConstructor",
                        "Contoso.Test.Business.Responses.TypeNotFoundConstructor",
                        new List<ParameterBase>
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
                                ContextProvider
                            )
                        },
                        new List<string>(),
                        "",
                        ContextProvider
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.FunctionList = new FunctionList
            (
                new Dictionary<string, Function>
                {
                    ["GetString"] = new Function
                    (
                        "GetString",
                        "GetString",
                        FunctionCategories.Standard,
                        "",
                        "flowManager.CustomActions",
                        "Field.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string>(),
                        new LiteralReturnType(LiteralFunctionReturnType.String, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["StaticMethod"] = new Function
                    (
                        "StaticMethod",
                        "StaticMethod",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClass`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["StaticMethodTypeNotFound"] = new Function
                    (
                        "StaticMethodTypeNotFound",
                        "StaticMethodTypeNotFound",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticMethodTypeNotFound`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["StaticMethodGenericReturn"] = new Function
                    (
                        "StaticMethodGenericReturn",
                        "StaticMethodGenericReturn",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClass`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>
                        {
                            new GenericParameter
                            (
                                "aProperty",
                                false,
                                "",
                                "A",
                                ContextProvider
                            ),
                            new GenericParameter
                            (
                                "bProperty",
                                false,
                                "",
                                "B",
                                ContextProvider
                            )
                        },
                        new List<string> { "A", "B" },
                        new GenericReturnType("B", ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["StaticMethodReturnsGenericType"] = new Function
                    (
                        "StaticMethodReturnsGenericType",
                        "StaticMethodReturnsGenericType",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClassOneArgument`1",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A" },
                        new GenericReturnType("A", ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["StaticMethodWrongCategory"] = new Function
                    (
                        "StaticMethodWrongCategory",
                        "StaticMethodWrongCategory",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticGenericClass`2",
                        "",
                        "",
                        "",
                        ReferenceCategories.This,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    ),
                    ["StaticNonGenericMethod"] = new Function
                    (
                        "StaticNonGenericMethod",
                        "StaticNonGenericMethod",
                        FunctionCategories.Standard,
                        "Contoso.Test.Business.StaticNonGenericClass",
                        "",
                        "",
                        "",
                        ReferenceCategories.Type,
                        ParametersLayout.Sequential,
                        new List<ParameterBase>(),
                        new List<string> { "A", "B" },
                        new LiteralReturnType(LiteralFunctionReturnType.Boolean, ContextProvider),
                        "",
                        ContextProvider
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>()),
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            ConfigurationService.VariableList = new VariableList
            (
                new Dictionary<string, VariableBase>
                {
                    ["ObjectVariableNotFound"] = new ObjectVariable
                    (
                        "ObjectVariableNotFound",
                        "ObjectVariableNotFound",
                        VariableCategory.StringKeyIndexer,
                        "Contoso.Test.Business.Responses.TypeNotFound",
                        "",
                        "flowManager.FlowDataCache.Items",
                        "Field.Property.Property",
                        "",
                        ReferenceCategories.InstanceReference,
                        "",
                        "System.Object",
                        ContextProvider
                    )
                },
                new TreeFolder("root", new List<string>(), new List<TreeFolder>())
            );

            foreach (LiteralVariableType enumValue in Enum.GetValues<LiteralVariableType>())
            {
                string variableName = $"{Enum.GetName(typeof(LiteralVariableType), enumValue)}Item";
                ConfigurationService.VariableList.Variables.Add(variableName, GetLiteralVariable(variableName, enumValue));
            }

            LoadContextSponsor.LoadAssembiesIfNeeded();
        }

        LiteralVariable GetLiteralVariable(string name, LiteralVariableType literalVariableType)
            => new
            (
                name,
                name,
                VariableCategory.StringKeyIndexer,
                "",
                "",
                "flowManager.FlowDataCache.Items",
                "Field.Property.Property",
                "",
                ReferenceCategories.InstanceReference,
                "",
                literalVariableType,
                LiteralVariableInputStyle.SingleLineTextBox,
                "",
                "",
                new List<string>(),
                ContextProvider
            );

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            LoadContextSponsor.UnloadAssembliesOnCloseProject();
            Assert.Empty(AssemblyLoadContextService.GetAssemblyLoadContext().Assemblies);
        }

        internal IServiceProvider ServiceProvider;
        internal IConfigurationService ConfigurationService;
        internal IGenericParametersHelper GenericParametersHelper;
        internal IContextProvider ContextProvider;
        internal IAssemblyLoadContextManager AssemblyLoadContextService;
        internal ILoadContextSponsor LoadContextSponsor;
        internal ITypeLoadHelper TypeLoadHelper;
        internal IApplicationTypeInfoManager ApplicationTypeInfoManager;
    }
}

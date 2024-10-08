﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.OData.Edm;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class EnumHelper : IEnumHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IStringHelper _stringHelper;
        private readonly ITypeHelper _typeHelper;

        public EnumHelper(IExceptionHelper exceptionHelper, IStringHelper stringHelper, ITypeHelper typeHelper)
        {
            _exceptionHelper = exceptionHelper;
            _stringHelper = stringHelper;
            _typeHelper = typeHelper;
        }

        public string BuildValidReferenceDefinition(string referenceDefinition)
        {
            if (string.IsNullOrEmpty(referenceDefinition)) return string.Empty;

            IDictionary<string, ValidIndirectReference> definitionsDictionary = ToValidIndirectReferenceDictionary();
            return string.Join
            (
                MiscellaneousConstants.PERIODSTRING,
                _stringHelper.SplitWithQuoteQualifier(referenceDefinition.Trim(), MiscellaneousConstants.PERIODSTRING)
                    .Select(e => Enum.GetName(typeof(ValidIndirectReference), definitionsDictionary[e.ToLowerInvariant()]))
                    .ToArray()
            );
        }

        public bool CanBeInteger(LiteralVariableType variableType)
            => _typeHelper.AssignableFrom(typeof(int), GetSystemType(variableType));

        public IList<string> ConvertEnumListToStringList<T>(IList<T>? excludedItems = null)
        {
            if (!typeof(Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{4445DF20-47AB-434F-B43A-51D9BCC5FC1A}");

            HashSet<T> excludedItemsSet = excludedItems != null
                ? new HashSet<T>(excludedItems)
                : new HashSet<T>();

            return Enum.GetNames(typeof(T)).Aggregate
            (
                new List<string>(), 
                (list, next) =>
                {
                    if (!excludedItemsSet.Contains((T)Enum.Parse(typeof(T), next)))
                        list.Add(next);

                    return list;
                }
            ).ToList();
        }

        public IList<string> ConvertVisibleDropDownValuesToEnumNames<T>(ICollection<string> array)
        {
            if (!typeof(System.Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{61CE868D-1095-4E48-842F-E92BDD354109}");

            //Visible value is the key and the enum name is the value
            IDictionary<string, string> dropDownDictionary = Enum.GetNames(typeof(T))
                .ToDictionary
                (
                    n => Strings.ResourceManager.GetString(string.Concat(MiscellaneousConstants.ENUMDESCRIPTION, n))?.ToLowerInvariant() ?? throw _exceptionHelper.CriticalException("{505DAC49-2C40-4051-AC2E-76488AA75A85}"),
                    n => n
                );

            return array.Select
            (
                next => dropDownDictionary.TryGetValue(next.ToLowerInvariant(), out string? enumValue) ? enumValue : next
            )
            .ToArray();
        }

        public IList<T> ConvertToEnumList<T>(IEnumerable<string> enumNames)
        {
            if (!typeof(Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{2D89326E-DD55-4FC8-A2DD-35A200573038}");

            return enumNames.Select(item => (T)Enum.Parse(typeof(T), item)).ToList();
        }

        public ListType GetConcreteListType(ListType listType) 
            => listType switch
            {
                ListType.IGenericCollection => ListType.GenericCollection,
                ListType.IGenericEnumerable
                    or ListType.IGenericList => ListType.GenericList,
                ListType.Array
                    or ListType.GenericList
                    or ListType.GenericCollection => listType,
                _ => throw _exceptionHelper.CriticalException("{BB8AF4B0-0765-4060-9BDD-F1530FC18246}"),
            };

        public string GetEnumResourceString(string? enumName)
        {
            if (string.IsNullOrEmpty(enumName))
                throw _exceptionHelper.CriticalException("{87C725C2-BEB3-4BDC-AFDD-E750642957D9}");

            string? resourceValue = Strings.ResourceManager.GetString(string.Concat(MiscellaneousConstants.ENUMDESCRIPTION, enumName)) ?? throw _exceptionHelper.CriticalException("{5E5F427C-CA44-4D7F-99F1-18DE4680D629}");
            return resourceValue;
        }

        public GenericConfigCategory GetGenericConfigCategory(string elementName)
        {
            return elementName switch
            {
                XmlDataConstants.LITERALPARAMETERELEMENT => GenericConfigCategory.Literal,
                XmlDataConstants.OBJECTPARAMETERELEMENT => GenericConfigCategory.Object,
                XmlDataConstants.LITERALLISTPARAMETERELEMENT => GenericConfigCategory.LiteralList,
                XmlDataConstants.OBJECTLISTPARAMETERELEMENT => GenericConfigCategory.ObjectList,
                _ => throw _exceptionHelper.CriticalException("{2547A733-855E-423D-BAEA-B2D29F3E5A7D}"),
            };
        }

        static readonly Dictionary<Type, ValidIndirectReference> typeToValidIndirectReferenceTable = new()
        {
            [typeof(string)] = ValidIndirectReference.StringKeyIndexer,
            [typeof(int)] = ValidIndirectReference.IntegerKeyIndexer,
            [typeof(bool)] = ValidIndirectReference.BooleanKeyIndexer,
            [typeof(DateTime)] = ValidIndirectReference.DateTimeKeyIndexer,
            [typeof(DateTimeOffset)] = ValidIndirectReference.DateTimeOffsetKeyIndexer,
            [typeof(DateOnly)] = ValidIndirectReference.DateOnlyKeyIndexer,
            [typeof(Date)] = ValidIndirectReference.DateKeyIndexer,
            [typeof(TimeSpan)] = ValidIndirectReference.TimeSpanKeyIndexer,
            [typeof(TimeOnly)] = ValidIndirectReference.TimeOnlyKeyIndexer,
            [typeof(TimeOfDay)] = ValidIndirectReference.TimeOfDayKeyIndexer,
            [typeof(Guid)] = ValidIndirectReference.GuidKeyIndexer,
            [typeof(byte)] = ValidIndirectReference.ByteKeyIndexer,
            [typeof(short)] = ValidIndirectReference.ShortKeyIndexer,
            [typeof(long)] = ValidIndirectReference.LongKeyIndexer,
            [typeof(float)] = ValidIndirectReference.FloatKeyIndexer,
            [typeof(double)] = ValidIndirectReference.DoubleKeyIndexer,
            [typeof(decimal)] = ValidIndirectReference.DecimalKeyIndexer,
            [typeof(char)] = ValidIndirectReference.CharKeyIndexer,
            [typeof(sbyte)] = ValidIndirectReference.SByteKeyIndexer,
            [typeof(ushort)] = ValidIndirectReference.UShortKeyIndexer,
            [typeof(uint)] = ValidIndirectReference.UIntegerKeyIndexer,
            [typeof(ulong)] = ValidIndirectReference.ULongKeyIndexer
        };

        public ValidIndirectReference GetIndexReferenceDefinition(Type indexType)
        {
            if (typeToValidIndirectReferenceTable.TryGetValue(indexType, out var value))
                return value;
            else
                throw _exceptionHelper.CriticalException("{C0115382-3B85-4FCD-AF3A-15F9991D65E3}");
        }

        static readonly Dictionary<VariableCategory, Type> variableCategoryToTypeTable = new()
        {
            [VariableCategory.StringKeyIndexer] = typeof(string),
            [VariableCategory.IntegerKeyIndexer] = typeof(int),
            [VariableCategory.BooleanKeyIndexer] = typeof(bool),
            [VariableCategory.DateTimeKeyIndexer] = typeof(DateTime),
            [VariableCategory.DateTimeOffsetKeyIndexer] = typeof(DateTimeOffset),
            [VariableCategory.DateOnlyKeyIndexer] = typeof(DateOnly),
            [VariableCategory.DateKeyIndexer] = typeof(Date),
            [VariableCategory.TimeSpanKeyIndexer] = typeof(TimeSpan),
            [VariableCategory.TimeOnlyKeyIndexer] = typeof(TimeOnly),
            [VariableCategory.TimeOfDayKeyIndexer] = typeof(TimeOfDay),
            [VariableCategory.GuidKeyIndexer] = typeof(Guid),
            [VariableCategory.ByteKeyIndexer] = typeof(byte),
            [VariableCategory.ShortKeyIndexer] = typeof(short),
            [VariableCategory.LongKeyIndexer] = typeof(long),
            [VariableCategory.FloatKeyIndexer] = typeof(float),
            [VariableCategory.DoubleKeyIndexer] = typeof(double),
            [VariableCategory.DecimalKeyIndexer] = typeof(decimal),
            [VariableCategory.CharKeyIndexer] = typeof(char),
            [VariableCategory.SByteKeyIndexer] = typeof(sbyte),
            [VariableCategory.UShortKeyIndexer] = typeof(ushort),
            [VariableCategory.UIntegerKeyIndexer] = typeof(uint),
            [VariableCategory.ULongKeyIndexer] = typeof(ulong)
        };

        public Type GetVariableCategoryIndexType(VariableCategory variableCategory)
        {
            if (variableCategoryToTypeTable.TryGetValue(variableCategory, out var value))
                return value;
            else
                throw _exceptionHelper.CriticalException("{035AE1DA-4596-432B-8BFC-9DAB0DC94BCD}");
        }

        static readonly Dictionary<Type, VariableCategory> typeToVariableCategoryTable = new()
        {
            [typeof(string)] = VariableCategory.StringKeyIndexer,
            [typeof(int)] = VariableCategory.IntegerKeyIndexer,
            [typeof(bool)] = VariableCategory.BooleanKeyIndexer,
            [typeof(DateTime)] = VariableCategory.DateTimeKeyIndexer,
            [typeof(DateTimeOffset)] = VariableCategory.DateTimeOffsetKeyIndexer,
            [typeof(DateOnly)] = VariableCategory.DateOnlyKeyIndexer,
            [typeof(Date)] = VariableCategory.DateKeyIndexer,
            [typeof(TimeSpan)] = VariableCategory.TimeSpanKeyIndexer,
            [typeof(TimeOnly)] = VariableCategory.TimeOnlyKeyIndexer,
            [typeof(TimeOfDay)] = VariableCategory.TimeOfDayKeyIndexer,
            [typeof(Guid)] = VariableCategory.GuidKeyIndexer,
            [typeof(byte)] = VariableCategory.ByteKeyIndexer,
            [typeof(short)] = VariableCategory.ShortKeyIndexer,
            [typeof(long)] = VariableCategory.LongKeyIndexer,
            [typeof(float)] = VariableCategory.FloatKeyIndexer,
            [typeof(double)] = VariableCategory.DoubleKeyIndexer,
            [typeof(decimal)] = VariableCategory.DecimalKeyIndexer,
            [typeof(char)] = VariableCategory.CharKeyIndexer,
            [typeof(sbyte)] = VariableCategory.SByteKeyIndexer,
            [typeof(ushort)] = VariableCategory.UShortKeyIndexer,
            [typeof(uint)] = VariableCategory.UIntegerKeyIndexer,
            [typeof(ulong)] = VariableCategory.ULongKeyIndexer
        };

        public VariableCategory GetIndexVariableCategory(Type indexType)
        {
            if (typeToVariableCategoryTable.TryGetValue(indexType, out var value))
                return value;
            else
                throw _exceptionHelper.CriticalException("{DEDB5B31-31D0-4BB3-9D50-EA29A908D948}");
        }

        public ListType GetListType(Type memberType)
        {
            if (memberType.IsGenericType && memberType.GetGenericTypeDefinition().Equals(typeof(List<>)))
                return ListType.GenericList;
            else if (memberType.IsGenericType && memberType.GetGenericTypeDefinition().Equals(typeof(IList<>)))
                return ListType.IGenericList;
            else if (memberType.IsGenericType && memberType.GetGenericTypeDefinition().Equals(typeof(Collection<>)))
                return ListType.GenericCollection;
            else if (memberType.IsGenericType && memberType.GetGenericTypeDefinition().Equals(typeof(ICollection<>)))
                return ListType.IGenericCollection;
            else if (memberType.IsGenericType && memberType.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
                return ListType.IGenericEnumerable;
            else if (memberType.IsArray)
                return ListType.Array;

            throw _exceptionHelper.CriticalException("{D0C6A67C-18F4-468B-AD4F-C4EB0A707A2B}");
        }

        public LiteralFunctionReturnType GetLiteralFunctionReturnType(Type functionReturnType)
            => GetLiteralEnumType<LiteralFunctionReturnType>(functionReturnType);

        public LiteralListElementType GetLiteralListElementType(LiteralParameterType literalType)
        {
            string? literalTypeString = Enum.GetName(typeof(LiteralParameterType), literalType);
            if (!Enum.IsDefined(typeof(LiteralListElementType), literalTypeString ?? ""))
                throw _exceptionHelper.CriticalException("{D7EE5D3F-6594-46E0-B5EC-15202252F9D2}");

            return ParseEnumText<LiteralListElementType>(literalTypeString!);//not null if defined
        }

        public LiteralListElementType GetLiteralListElementType(LiteralVariableType literalType)
        {
            string? literalTypeString = Enum.GetName(typeof(LiteralVariableType), literalType);
            if (!Enum.IsDefined(typeof(LiteralListElementType), literalTypeString ?? ""))
                throw _exceptionHelper.CriticalException("{170A44A8-03E1-4504-B5C1-D1A05F84A69D}");

            return ParseEnumText<LiteralListElementType>(literalTypeString!);//not null if defined
        }

        public LiteralListElementType GetLiteralListElementType(Type literalType)
            => GetLiteralEnumType<LiteralListElementType>(literalType);

        public LiteralParameterType GetLiteralParameterType(LiteralListElementType literalType)
        {
            string? literalTypeString = Enum.GetName(typeof(LiteralListElementType), literalType);
            if (!Enum.IsDefined(typeof(LiteralParameterType), literalTypeString ?? ""))
                throw _exceptionHelper.CriticalException("{408A8237-B73B-4156-B435-055A03F86F4B}");

            return ParseEnumText<LiteralParameterType>(literalTypeString!);//not null if defined
        }

        public LiteralParameterType GetLiteralParameterType(Type parameterType) 
            => GetLiteralEnumType<LiteralParameterType>(parameterType);

        public LiteralType GetLiteralType(Type literalType)
            => GetLiteralEnumType<LiteralType>(literalType);

        public LiteralVariableType GetLiteralVariableType(LiteralListElementType literalType)
        {
            string? literalTypeString = Enum.GetName(typeof(LiteralListElementType), literalType);
            if (!Enum.IsDefined(typeof(LiteralVariableType), literalTypeString ?? ""))
                throw _exceptionHelper.CriticalException("{378118AF-037D-4B19-8C7D-6D265F5FD031}");

            return ParseEnumText<LiteralVariableType>(literalTypeString!);//not null if defined
        }

        public LiteralVariableType GetLiteralVariableType(Type variableType)
            => GetLiteralEnumType<LiteralVariableType>(variableType);

        public ObjectCategory GetObjectCategory(string elementName) 
            => elementName switch
            {
                XmlDataConstants.CONSTRUCTORELEMENT => ObjectCategory.Constructor,
                XmlDataConstants.FUNCTIONELEMENT => ObjectCategory.Function,
                XmlDataConstants.VARIABLEELEMENT => ObjectCategory.Variable,
                XmlDataConstants.LITERALLISTELEMENT => ObjectCategory.LiteralList,
                XmlDataConstants.OBJECTLISTELEMENT => ObjectCategory.ObjectList,
                _ => throw _exceptionHelper.CriticalException("{9DA384FE-1023-4C5A-89AA-3EB1DBC323DC}"),
            };

        public ParameterCategory GetParameterCategory(string elementName)
        {
            return elementName switch
            {
                XmlDataConstants.LITERALPARAMETERELEMENT => ParameterCategory.Literal,
                XmlDataConstants.OBJECTPARAMETERELEMENT => ParameterCategory.Object,
                XmlDataConstants.GENERICPARAMETERELEMENT => ParameterCategory.Generic,
                XmlDataConstants.LITERALLISTPARAMETERELEMENT => ParameterCategory.LiteralList,
                XmlDataConstants.OBJECTLISTPARAMETERELEMENT => ParameterCategory.ObjectList,
                XmlDataConstants.GENERICLISTPARAMETERELEMENT => ParameterCategory.GenericList,
                _ => throw _exceptionHelper.CriticalException("{3CC2927F-BFA1-4E9D-9D16-1DCD0603EE60}"),
            };
        }

        public ReturnTypeCategory GetReturnTypeCategory(string elementName) 
            => elementName switch
            {
                XmlDataConstants.LITERALELEMENT => ReturnTypeCategory.Literal,
                XmlDataConstants.OBJECTELEMENT => ReturnTypeCategory.Object,
                XmlDataConstants.GENERICELEMENT => ReturnTypeCategory.Generic,
                XmlDataConstants.LITERALLISTELEMENT => ReturnTypeCategory.LiteralList,
                XmlDataConstants.OBJECTLISTELEMENT => ReturnTypeCategory.ObjectList,
                XmlDataConstants.GENERICLISTELEMENT => ReturnTypeCategory.GenericList,
                _ => throw _exceptionHelper.CriticalException("{3A66A34C-D19F-4B76-9E26-3F95FFA09B36}"),
            };

        public Type GetSystemType(ListType listType, Type elementType) 
            => listType switch
            {
                ListType.Array => elementType.MakeArrayType(),
                ListType.GenericCollection => typeof(Collection<>).MakeGenericType(elementType),
                ListType.GenericList => typeof(List<>).MakeGenericType(elementType),
                ListType.IGenericCollection => typeof(ICollection<>).MakeGenericType(elementType),
                ListType.IGenericEnumerable => typeof(IEnumerable<>).MakeGenericType(elementType),
                ListType.IGenericList => typeof(IList<>).MakeGenericType(elementType),
                _ => throw _exceptionHelper.CriticalException("{32CE536A-47D0-412C-8558-78321BD89384}"),
            };

        public Type GetSystemType(LiteralFunctionReturnType functionReturnType) 
            => GetSystemTypeFromEnum(functionReturnType);

        public Type GetSystemType(LiteralListElementType literalType)
        {
            if (literalType == LiteralListElementType.Any)
                return typeof(string);

            return GetSystemTypeFromEnum(literalType);
        }

        public Type GetSystemType(LiteralParameterType parameterType)
        {
            if (parameterType == LiteralParameterType.Any)
                return typeof(string);

            return GetSystemTypeFromEnum(parameterType);
        }

        public Type GetSystemType(LiteralType literalType) 
            => GetSystemTypeFromEnum(literalType);

        public Type GetSystemType(LiteralVariableType variableType) 
            => GetSystemTypeFromEnum(variableType);

        public string GetTypeDescription(ListType listType, string elementType) 
            => string.Format
            (
                Strings.listDescriptionFormat, 
                GetVisibleEnumText(listType), 
                elementType
            );

        public string GetValidCategoriesList()
            => string.Join
            (
                Environment.NewLine,
                Enum.GetValues(typeof(ReferenceCategories))
                    .OfType<ReferenceCategories>()
                    .Select
                    (
                        e => GetVisibleEnumText(e)
                    )
            );

        public ValidIndirectReference GetValidIndirectReference(VariableCategory variableCategory)
        {
            string? literalTypeString = Enum.GetName(typeof(VariableCategory), variableCategory);
            if (!Enum.IsDefined(typeof(ValidIndirectReference), literalTypeString ?? ""))
                throw _exceptionHelper.CriticalException("{48F6C59B-69D1-4658-B87B-6B2921538A8E}");

            return ParseEnumText<ValidIndirectReference>(literalTypeString!);
        }

        public string GetValidIndirectReferencesList()
            => string.Join
            (
                Environment.NewLine,
                Enum.GetValues(typeof(ValidIndirectReference))
                    .OfType<ValidIndirectReference>()
                    .Select
                    (
                        e => GetVisibleEnumText(e)
                    )
            );

        public VariableCategory GetVariableCategory(ValidIndirectReference validIndirectReference)
        {
            string? literalTypeString = Enum.GetName(typeof(ValidIndirectReference), validIndirectReference);
            if (!Enum.IsDefined(typeof(VariableCategory), literalTypeString ?? ""))
                throw _exceptionHelper.CriticalException("{A23350F6-7EC0-402A-823C-DF83E79A161D}");

            return ParseEnumText<VariableCategory>(literalTypeString!);
        }

        public VariableTypeCategory GetVariableTypeCategory(string elementName) 
            => elementName switch
            {
                XmlDataConstants.LITERALVARIABLEELEMENT => VariableTypeCategory.Literal,
                XmlDataConstants.OBJECTVARIABLEELEMENT => VariableTypeCategory.Object,
                XmlDataConstants.LITERALLISTVARIABLEELEMENT => VariableTypeCategory.LiteralList,
                XmlDataConstants.OBJECTLISTVARIABLEELEMENT => VariableTypeCategory.ObjectList,
                _ => throw _exceptionHelper.CriticalException("{1727C7D3-FCAA-4A6F-BBD1-B4031824E2C6}"),
            };

        public string GetVisibleEnumText<T>(T enumType)
        {
            if (!typeof(Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{6BE2E8CC-C074-42AF-87C4-26AD076A8805}");

            if (enumType == null)
                throw _exceptionHelper.CriticalException("{D0C2AAEC-7605-427B-8EFD-EC717C551A09}");

            return GetEnumResourceString(Enum.GetName(typeof(T), enumType));
        }

        public bool IsValidCodeBinaryOperator(string item)
        {
            if (!Enum.IsDefined(typeof(CodeBinaryOperatorType), item))
                return false;

            return (CodeBinaryOperatorType)Enum.Parse(typeof(CodeBinaryOperatorType), item) != CodeBinaryOperatorType.Assign;
        }

        public T ParseEnumText<T>(string text)
        {
            if (!typeof(System.Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{D72B72F4-BA76-4CDE-943A-3F91202D6166}");

            return (T)Enum.Parse(typeof(T), text);
        }

        public IDictionary<string, ValidIndirectReference> ToValidIndirectReferenceDictionary()
            => Enum.GetNames(typeof(ValidIndirectReference))
                .ToDictionary
                (
                    name => GetEnumResourceString(name).ToLowerInvariant(),
                    name => (ValidIndirectReference)Enum.Parse(typeof(ValidIndirectReference), name)
                );

        public IList<string> ToVisibleDropdownValues(ICollection<string> array)
            => array.Select
            (
                next => _stringHelper.GetResoureString
                (
                    string.Concat(MiscellaneousConstants.ENUMDESCRIPTION, next),
                    true
                )
            )
            .ToArray();

        private Type GetSystemTypeFromEnum<T>(T enumType)
        {
            if (!typeof(Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{F7FA803F-43AD-429F-8C9C-50E92A38274F}");

            if (enumType == null)
                throw _exceptionHelper.CriticalException("{14B192D2-7E33-4E2F-A0DA-617CFB667797}");

            if (!Enum.IsDefined(typeof(T), enumType))
                throw _exceptionHelper.CriticalException("{6A3959B6-7D6F-44A7-AD46-FE18A8F470B5}");

            return GetSystemType
            (
                Enum.GetName
                (
                    typeof(T),
                    enumType
                )!/*Not null if Enum.IsDefined().*/
            );

            Type GetSystemType(string literalTypeString)
            {
                if (!Enum.IsDefined(typeof(LiteralType), literalTypeString))
                    throw _exceptionHelper.CriticalException("{3AF42B3B-1C9E-465B-A729-2C3FDEE5C033}");

                return LookupSystemType(ParseEnumText<LiteralType>(literalTypeString));
            }
        }

        private T GetLiteralEnumType<T>(Type enumType)
        {
            if (!typeof(System.Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{FD140CDA-D9C9-4E51-9AC6-22943D854030}");

            return GetLiteralTypeEnum
            (
                Enum.GetName
                (
                    typeof(LiteralType),
                    LookupLiteralType(enumType)
                )!/*LookupLiteralType() throws for an invalid enumType.*/
            );

            T GetLiteralTypeEnum(string literalTypeString)
            {
                if (!Enum.IsDefined(typeof(T), literalTypeString))
                    throw _exceptionHelper.CriticalException("{E17E3989-9339-43DD-B109-D3F7087D2C60}");

                return ParseEnumText<T>(literalTypeString);
            }
        }

        static readonly Dictionary<Type, LiteralType> typeToEnumTable = new()
        {
            [typeof(void)] = LiteralType.Void,
            [typeof(bool)] = LiteralType.Boolean,
            [typeof(DateTimeOffset)] = LiteralType.DateTimeOffset,
            [typeof(DateOnly)] = LiteralType.DateOnly,
            [typeof(DateTime)] = LiteralType.DateTime,
            [typeof(Date)] = LiteralType.Date,
            [typeof(TimeSpan)] = LiteralType.TimeSpan,
            [typeof(TimeOnly)] = LiteralType.TimeOnly,
            [typeof(TimeOfDay)] = LiteralType.TimeOfDay,
            [typeof(Guid)] = LiteralType.Guid,
            [typeof(decimal)] = LiteralType.Decimal,
            [typeof(byte)] = LiteralType.Byte,
            [typeof(short)] = LiteralType.Short,
            [typeof(int)] = LiteralType.Integer,
            [typeof(long)] = LiteralType.Long,
            [typeof(float)] = LiteralType.Float,
            [typeof(double)] = LiteralType.Double,
            [typeof(char)] = LiteralType.Char,
            [typeof(sbyte)] = LiteralType.SByte,
            [typeof(ushort)] = LiteralType.UShort,
            [typeof(uint)] = LiteralType.UInteger,
            [typeof(ulong)] = LiteralType.ULong,
            [typeof(string)] = LiteralType.String,
            [typeof(bool?)] = LiteralType.NullableBoolean,
            [typeof(DateTimeOffset?)] = LiteralType.NullableDateTimeOffset,
            [typeof(DateOnly?)] = LiteralType.NullableDateOnly,
            [typeof(DateTime?)] = LiteralType.NullableDateTime,
            [typeof(Date?)] = LiteralType.NullableDate,
            [typeof(TimeSpan?)] = LiteralType.NullableTimeSpan,
            [typeof(TimeOnly?)] = LiteralType.NullableTimeOnly,
            [typeof(TimeOfDay?)] = LiteralType.NullableTimeOfDay,
            [typeof(Guid?)] = LiteralType.NullableGuid,
            [typeof(decimal?)] = LiteralType.NullableDecimal,
            [typeof(byte?)] = LiteralType.NullableByte,
            [typeof(short?)] = LiteralType.NullableShort,
            [typeof(int?)] = LiteralType.NullableInteger,
            [typeof(long?)] = LiteralType.NullableLong,
            [typeof(float?)] = LiteralType.NullableFloat,
            [typeof(double?)] = LiteralType.NullableDouble,
            [typeof(char?)] = LiteralType.NullableChar,
            [typeof(sbyte?)] = LiteralType.NullableSByte,
            [typeof(ushort?)] = LiteralType.NullableUShort,
            [typeof(uint?)] = LiteralType.NullableUInteger,
            [typeof(ulong?)] = LiteralType.NullableULong
        };

        private LiteralType LookupLiteralType(Type literalType)
        {
            if (typeToEnumTable.TryGetValue(literalType, out var value))
                return value;
            else
                throw _exceptionHelper.CriticalException("{66DB8C24-091C-4475-914D-E9CA735526CC}");
        }

        static readonly Dictionary<LiteralType, Type> enumToTypeTable = new()
        {
            [LiteralType.Void] = typeof(void),
            [LiteralType.Boolean] = typeof(bool),
            [LiteralType.DateTimeOffset] = typeof(DateTimeOffset),
            [LiteralType.DateOnly] = typeof(DateOnly),
            [LiteralType.DateTime] = typeof(DateTime),
            [LiteralType.Date] = typeof(Date),
            [LiteralType.TimeSpan] = typeof(TimeSpan),
            [LiteralType.TimeOnly] = typeof(TimeOnly),
            [LiteralType.TimeOfDay] = typeof(TimeOfDay),
            [LiteralType.Guid] = typeof(Guid),
            [LiteralType.Decimal] = typeof(decimal),
            [LiteralType.Byte] = typeof(byte),
            [LiteralType.Short] = typeof(short),
            [LiteralType.Integer] = typeof(int),
            [LiteralType.Long] = typeof(long),
            [LiteralType.Float] = typeof(float),
            [LiteralType.Double] = typeof(double),
            [LiteralType.Char] = typeof(char),
            [LiteralType.SByte] = typeof(sbyte),
            [LiteralType.UShort] = typeof(ushort),
            [LiteralType.UInteger] = typeof(uint),
            [LiteralType.ULong] = typeof(ulong),
            [LiteralType.String] = typeof(string),
            [LiteralType.NullableBoolean] = typeof(bool?),
            [LiteralType.NullableDateTimeOffset] = typeof(DateTimeOffset?),
            [LiteralType.NullableDateOnly] = typeof(DateOnly?),
            [LiteralType.NullableDateTime] = typeof(DateTime?),
            [LiteralType.NullableDate] = typeof(Date?),
            [LiteralType.NullableTimeSpan] = typeof(TimeSpan?),
            [LiteralType.NullableTimeOnly] = typeof(TimeOnly?),
            [LiteralType.NullableTimeOfDay] = typeof(TimeOfDay?),
            [LiteralType.NullableGuid] = typeof(Guid?),
            [LiteralType.NullableDecimal] = typeof(decimal?),
            [LiteralType.NullableByte] = typeof(byte?),
            [LiteralType.NullableShort] = typeof(short?),
            [LiteralType.NullableInteger] = typeof(int?),
            [LiteralType.NullableLong] = typeof(long?),
            [LiteralType.NullableFloat] = typeof(float?),
            [LiteralType.NullableDouble] = typeof(double?),
            [LiteralType.NullableChar] = typeof(char?),
            [LiteralType.NullableSByte] = typeof(sbyte?),
            [LiteralType.NullableUShort] = typeof(ushort?),
            [LiteralType.NullableUInteger] = typeof(uint?),
            [LiteralType.NullableULong] = typeof(ulong?)
        };

        private Type LookupSystemType(LiteralType literalType)
        {
            if (enumToTypeTable.TryGetValue(literalType, out var value))
                return value;
            else
                throw _exceptionHelper.CriticalException("{C0115382-3B85-4FCD-AF3A-15F9991D65E3}");
        }
    }
}

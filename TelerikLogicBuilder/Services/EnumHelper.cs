using ABIS.LogicBuilder.FlowBuilder.Constants;
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

        public IList<T> ConvertToEnumList<T>(IEnumerable<string> enumNames)
        {
            if (!typeof(Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{2D89326E-DD55-4FC8-A2DD-35A200573038}");

            return enumNames.Select(item => (T)Enum.Parse(typeof(T), item)).ToList();
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

        public LiteralListElementType GetLiteralListElementType(Type literalType)
            => GetLiteralEnumType<LiteralListElementType>(literalType);

        public LiteralParameterType GetLiteralParameterType(Type parameterType) 
            => GetLiteralEnumType<LiteralParameterType>(parameterType);

        public LiteralType GetLiteralType(Type literalType)
            => GetLiteralEnumType<LiteralType>(literalType);

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

        private LiteralType LookupLiteralType(Type literalType)
        {
            Dictionary<Type, LiteralType> table = new()
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

            if (table.TryGetValue(literalType, out var value))
                return value;
            else
                throw _exceptionHelper.CriticalException("{66DB8C24-091C-4475-914D-E9CA735526CC}");
        }

        private Type LookupSystemType(LiteralType literalType)
        {
            Dictionary<LiteralType, Type> table = new()
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

            if (table.TryGetValue(literalType, out var value))
                return value;
            else
                throw _exceptionHelper.CriticalException("{C0115382-3B85-4FCD-AF3A-15F9991D65E3}");
        }

        public IDictionary<string, ValidIndirectReference> ToValidIndirectReferenceDictionary()
            => Enum.GetNames(typeof(ValidIndirectReference))
                .ToDictionary
                (
                    name => GetEnumResourceString(name).ToLowerInvariant(),
                    name => (ValidIndirectReference)Enum.Parse(typeof(ValidIndirectReference), name)
                );

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

        private string GetEnumResourceString(string? enumName)
        {
            if (string.IsNullOrEmpty(enumName))
                throw _exceptionHelper.CriticalException("{87C725C2-BEB3-4BDC-AFDD-E750642957D9}");

            string? resourceValue = Strings.ResourceManager.GetString(string.Concat(MiscellaneousConstants.ENUMDESCRIPTION, enumName));

            if (resourceValue == null)
                throw _exceptionHelper.CriticalException("{5E5F427C-CA44-4D7F-99F1-18DE4680D629}");

            return resourceValue;
        }
    }
}

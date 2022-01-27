using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class EnumHelper : IEnumHelper
    {
        private readonly IExceptionHelper _exceptionHelper;

        public EnumHelper(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
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

        public ParameterType GetParameterType(Type parameterType)
        {
            Dictionary<Type, ParameterType> table = new()
            {
                [typeof(bool)] = ParameterType.Boolean,
                [typeof(DateTimeOffset)] = ParameterType.DateTimeOffset,
                [typeof(DateTime)] = ParameterType.DateTime,
                [typeof(Date)] = ParameterType.Date,
                [typeof(TimeSpan)] = ParameterType.TimeSpan,
                [typeof(TimeOfDay)] = ParameterType.TimeOfDay,
                [typeof(Guid)] = ParameterType.Guid,
                [typeof(decimal)] = ParameterType.Decimal,
                [typeof(byte)] = ParameterType.Byte,
                [typeof(short)] = ParameterType.Short,
                [typeof(int)] = ParameterType.Integer,
                [typeof(long)] = ParameterType.Long,
                [typeof(float)] = ParameterType.Float,
                [typeof(double)] = ParameterType.Double,
                [typeof(char)] = ParameterType.Char,
                [typeof(sbyte)] = ParameterType.SByte,
                [typeof(ushort)] = ParameterType.UShort,
                [typeof(uint)] = ParameterType.UInteger,
                [typeof(ulong)] = ParameterType.ULong,
                [typeof(string)] = ParameterType.String,
                [typeof(bool?)] = ParameterType.NullableBoolean,
                [typeof(DateTimeOffset?)] = ParameterType.NullableDateTimeOffset,
                [typeof(DateTime?)] = ParameterType.NullableDateTime,
                [typeof(Date?)] = ParameterType.NullableDate,
                [typeof(TimeSpan?)] = ParameterType.NullableTimeSpan,
                [typeof(TimeOfDay?)] = ParameterType.NullableTimeOfDay,
                [typeof(Guid?)] = ParameterType.NullableGuid,
                [typeof(decimal?)] = ParameterType.NullableDecimal,
                [typeof(byte?)] = ParameterType.NullableByte,
                [typeof(short?)] = ParameterType.NullableShort,
                [typeof(int?)] = ParameterType.NullableInteger,
                [typeof(long?)] = ParameterType.NullableLong,
                [typeof(float?)] = ParameterType.NullableFloat,
                [typeof(double?)] = ParameterType.NullableDouble,
                [typeof(char?)] = ParameterType.NullableChar,
                [typeof(sbyte?)] = ParameterType.NullableSByte,
                [typeof(ushort?)] = ParameterType.NullableUShort,
                [typeof(uint?)] = ParameterType.NullableUInteger,
                [typeof(ulong?)] = ParameterType.NullableULong,
            };

            if (table.TryGetValue(parameterType, out var value))
                return value;
            else
                throw _exceptionHelper.CriticalException("{D732140E-3A93-4706-8925-CEB6AF0753C1}");
        }

        public string GetTypeDescription(ListType listType, string elementType) 
            => string.Format
            (
                Strings.listDescriptionFormat, 
                GetVisibleEnumText(listType), 
                elementType
            );

        public string GetVisibleEnumText<T>(T enumType)
        {
            if (!typeof(Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{6BE2E8CC-C074-42AF-87C4-26AD076A8805}");

            return Strings.ResourceManager.GetString(string.Concat(MiscellaneousConstants.ENUMDESCRIPTION, Enum.GetName(typeof(T), enumType)));
        }
    }
}

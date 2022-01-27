using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class TypeHelper : ITypeHelper
    {
        private readonly IExceptionHelper _exceptionHelper;

        public TypeHelper(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public string GetTypeDescription(Type type)
        {
            if (type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(List<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IList<>))
                || type.GetGenericTypeDefinition().Equals(typeof(Collection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(ICollection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>))))
                return string.Format(CultureInfo.InvariantCulture, MiscellaneousConstants.GENERICTYPEFORMAT, type.Name, GetTypeDescription(type.GetGenericArguments()[0]));
            else if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                return string.Format(CultureInfo.InvariantCulture, MiscellaneousConstants.GENERICTYPEFORMAT, type.Name, GetTypeDescription(Nullable.GetUnderlyingType(type)));
            else if (type.IsGenericType)
                return string.Format(CultureInfo.InvariantCulture, MiscellaneousConstants.GENERICTYPEFORMAT, type.Name, string.Join(MiscellaneousConstants.COMMA.ToString(), type.GetGenericArguments().Select(a => GetTypeDescription(a))));
            else
                return type.Name;
        }

        public Type GetUndelyingTypeForValidList(Type type)
        {
            if (type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(List<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IList<>))
                || type.GetGenericTypeDefinition().Equals(typeof(Collection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(ICollection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>))))
                return type.GetGenericArguments()[0];
            else if (type.IsArray)
                return type.GetElementType();
            else
                throw _exceptionHelper.CriticalException("{5ED4C326-21DA-4EB7-937A-2ED05DB47A0E}");
        }

        public bool IsLiteralType(Type type)
        {
            if (IsNullable(type))
                type = Nullable.GetUnderlyingType(type);

            return MiscellaneousConstants.Literals.Contains(type);
        }

        public bool IsNullable(Type type) 
            => type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));

        public bool IsValidList(Type type) 
            => (type.IsGenericType && (type.GetGenericTypeDefinition().Equals(typeof(List<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IList<>))
                || type.GetGenericTypeDefinition().Equals(typeof(Collection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(ICollection<>))
                || type.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>))))
                || (type.IsArray && type.GetArrayRank() == 1);

        public string ToId(Type type)
        {
            string typeName = ToId();

            if (typeName != null)
                return typeName;

            type = type.Assembly.GetTypes().FirstOrDefault(t => t?.Name == type.Name);//Search the assembly when type is not null
                                                                                      //but AssemblyQualifiedName and FullName are null
            if (type == null)
                return null;

            return ToId();

            string ToId()
                => type.IsGenericType && !type.IsGenericTypeDefinition
                           ? type.AssemblyQualifiedName
                           : type.FullName;
        }
    }
}

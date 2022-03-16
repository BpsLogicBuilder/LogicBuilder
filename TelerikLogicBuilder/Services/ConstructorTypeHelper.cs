using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ConstructorTypeHelper : IConstructorTypeHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public ConstructorTypeHelper(IConfigurationService configurationService, ITypeHelper typeHelper, ITypeLoadHelper typeLoadHelper)
        {
            _configurationService = configurationService;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
        }

        public IDictionary<string, Constructor> GetConstructors(Type constructorType, ApplicationTypeInfo application)
        {
            Dictionary<string, Constructor> objectDictionary = _configurationService.ConstructorList.Constructors.Aggregate
            (
                new Dictionary<string, Constructor>(), 
                (dic, next) =>
                {
                    if (!dic.ContainsKey(next.Value.TypeName))
                        dic.Add(next.Value.TypeName, next.Value);

                    return dic;
                }
            );

            if (constructorType.IsGenericType //if constructorType is a generic type with updated arguments (closed generic type)
                && !constructorType.IsGenericTypeDefinition) //return the constructor for the generic type definition if it exists
            {
                if (objectDictionary.TryGetValue(_typeHelper.ToId(constructorType.GetGenericTypeDefinition()), out Constructor? constructor))
                    return new Dictionary<string, Constructor> { [constructor.Name]=constructor };
                else
                    return new Dictionary<string, Constructor>();
            }

            return _configurationService.ConstructorList.Constructors.Where
            (
                constructor =>
                {
                    if (!_typeLoadHelper.TryGetSystemType(constructor.Value.TypeName, application, out Type? cType))
                        return false;

                    return _typeHelper.AssignableFrom(constructorType, cType);
                }
            ).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public IDictionary<string, Constructor> GetConstructors(string objectType, ApplicationTypeInfo application)
        {
            if (_typeLoadHelper.TryGetSystemType(objectType, application, out Type? constructorType))
                return GetConstructors(constructorType, application);

            return new Dictionary<string, Constructor>();
        }
    }
}

using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ConstructorTypeHelper : IConstructorTypeHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IGenericConfigManager _genericConfigManager;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public ConstructorTypeHelper(
            IConfigurationService configurationService,
            IGenericConfigManager genericConfigManager,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper)
        {
            _configurationService = configurationService;
            _genericConfigManager = genericConfigManager;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
        }

        public ClosedConstructor? GetConstructor(Type constructorType, ApplicationTypeInfo application)
            => GetConstructor(_typeHelper.ToId(constructorType), application);

        public ClosedConstructor? GetConstructor(string objectType, ApplicationTypeInfo application)
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

            if (objectDictionary.TryGetValue(objectType, out Constructor? constructor))
                return new ClosedConstructor(constructor, new List<GenericConfigBase>());

            if (!_typeLoadHelper.TryGetSystemType(objectType, application, out Type? constructorType))
                return null;

            if (constructorType.IsGenericType //if constructorType is a generic type with updated arguments (closed generic type)
                && !constructorType.IsGenericTypeDefinition//return the constructor for the generic type definition if it exists
                && objectDictionary.TryGetValue(_typeHelper.ToId(constructorType.GetGenericTypeDefinition()), out constructor)) 
            {
                IList<Type> argArguments = constructorType.GetGenericArguments();
                if (argArguments.Count == constructor.GenericArguments.Count)
                {
                    return new ClosedConstructor
                    (
                        constructor, 
                        _genericConfigManager.CreateGenericConfigs
                        (
                            constructor.GenericArguments, 
                            argArguments
                        )
                    );
                }
            }

            constructor = _configurationService.ConstructorList.Constructors
                .OrderBy(c => c.Key)
                .FirstOrDefault
                (
                    c =>
                    {
                        if (!_typeLoadHelper.TryGetSystemType(c.Value.TypeName, application, out Type? currentType))
                            return false;

                        return _typeHelper.AssignableFrom(constructorType, currentType);
                    }
                ).Value;

            return constructor == null 
                    ? null 
                    : new ClosedConstructor(constructor, new List<GenericConfigBase>());
        }

        public IDictionary<string, Constructor> GetConstructors(Type constructorType, ApplicationTypeInfo application)
        {
            Dictionary<string, Constructor> returnValue = new();

            if (constructorType.IsGenericType //if constructorType is a generic type with updated arguments (closed generic type)
                && !constructorType.IsGenericTypeDefinition) //add the constructor for the generic type definition if it exists
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

                if (objectDictionary.TryGetValue(_typeHelper.ToId(constructorType.GetGenericTypeDefinition()), out Constructor? constructor))
                    returnValue.Add(constructor.Name, constructor);
            }

            return _configurationService.ConstructorList.Constructors.Aggregate
            (
                returnValue, 
                (dictionary, next) =>
                {
                    if (_typeLoadHelper.TryGetSystemType(next.Value.TypeName, application, out Type? cType)
                        && _typeHelper.AssignableFrom(constructorType, cType))
                        dictionary.Add(next.Key, next.Value);

                    return dictionary;
                }
            );
        }

        public IDictionary<string, Constructor> GetConstructors(string objectType, ApplicationTypeInfo application)
        {
            if (_typeLoadHelper.TryGetSystemType(objectType, application, out Type? constructorType))
                return GetConstructors(constructorType, application);

            return new Dictionary<string, Constructor>();
        }
    }
}

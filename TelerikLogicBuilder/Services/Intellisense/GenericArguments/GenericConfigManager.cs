using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.GenericArguments
{
    internal class GenericConfigManager : IGenericConfigManager
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericConfigFactory _genericConfigFactory;
        private readonly ITypeHelper _typeHelper;

        public GenericConfigManager(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IGenericConfigFactory genericConfigFactory,
            ITypeHelper typeHelper)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _genericConfigFactory = genericConfigFactory;
            _typeHelper = typeHelper;
        }

        public GenericConfigBase CreateGenericConfig(string genericArgumentName, Type genericTypeArgument)
        {
            if (_typeHelper.IsLiteralType(genericTypeArgument))
                return GetDefaultLiteralGenericConfig(genericArgumentName, _enumHelper.GetLiteralParameterType(genericTypeArgument));
            else if (_typeHelper.IsValidList(genericTypeArgument))
            {
                Type underlyingType = _typeHelper.GetUndelyingTypeForValidList(genericTypeArgument);
                if (_typeHelper.IsLiteralType(underlyingType))
                {
                    return GetDefaultLiteralListGenericConfig
                    (
                        genericArgumentName,
                        _enumHelper.GetLiteralParameterType(underlyingType),
                        _enumHelper.GetListType(genericTypeArgument)
                    );
                }
                else
                {
                    return GetDefaultObjectListGenericConfig
                    (
                        genericArgumentName,
                        _typeHelper.ToId(genericTypeArgument),
                        _enumHelper.GetListType(genericTypeArgument)
                    );
                }
            }
            else if (genericTypeArgument.IsAbstract || genericTypeArgument.IsInterface || genericTypeArgument.IsEnum)
            {//keeping these separate form the regular concrete type below - may need further work
                return GetDefaultObjectGenericConfig(genericArgumentName, _typeHelper.ToId(genericTypeArgument));
            }
            else
            {
                return GetDefaultObjectGenericConfig(genericArgumentName, _typeHelper.ToId(genericTypeArgument));
            }
        }

        public IList<GenericConfigBase> CreateGenericConfigs(IList<string> configuredGenericArgumentNames, IList<Type> arguments)
        {
            if (configuredGenericArgumentNames.Count != arguments.Count)
                throw _exceptionHelper.CriticalException("{A25C4EE3-094E-4894-BC8A-D350A38DB4E1}");

            List<GenericConfigBase> genericConfigList = new();
            for (int i = 0; i < configuredGenericArgumentNames.Count; i++)
                genericConfigList.Add(CreateGenericConfig(configuredGenericArgumentNames[i], arguments[i]));
            return genericConfigList;
        }

        public LiteralGenericConfig GetDefaultLiteralGenericConfig(string genericArgumentName)
           => GetDefaultLiteralGenericConfig(genericArgumentName, LiteralParameterType.String);

        public LiteralListGenericConfig GetDefaultLiteralListGenericConfig(string genericArgumentName)
            => GetDefaultLiteralListGenericConfig(genericArgumentName, LiteralParameterType.String, ListType.GenericList);

        public ObjectGenericConfig GetDefaultObjectGenericConfig(string genericArgumentName)
            => GetDefaultObjectGenericConfig(genericArgumentName, MiscellaneousConstants.DEFAULT_OBJECT_TYPE);

        public ObjectListGenericConfig GetDefaultObjectListGenericConfig(string genericArgumentName)
            => GetDefaultObjectListGenericConfig(genericArgumentName, MiscellaneousConstants.DEFAULT_OBJECT_TYPE, ListType.GenericList);

        public IList<GenericConfigBase> ReconcileGenericArguments(IList<string> configuredGenericArgumentNames, IList<GenericConfigBase> dataConfigGenericArguments)
        {
            if (configuredGenericArgumentNames.SequenceEqual(dataConfigGenericArguments.Select(a => a.GenericArgumentName)))
                return dataConfigGenericArguments;

            Dictionary<string, GenericConfigBase> configDictionary = dataConfigGenericArguments.ToDictionary(c => c.GenericArgumentName);

            return configuredGenericArgumentNames.Aggregate(new List<GenericConfigBase>(), (list, next) =>
            {
                if (configDictionary.TryGetValue(next, out GenericConfigBase? cfg))
                    list.Add(cfg);
                else
                    list.Add(GetDefaultObjectGenericConfig(next));
                return list;
            });
        }

        private LiteralGenericConfig GetDefaultLiteralGenericConfig(string genericArgumentName, LiteralParameterType literalType)
            => _genericConfigFactory.GetLiteralGenericConfig
            (
                genericArgumentName,
                literalType,
                LiteralParameterInputStyle.SingleLineTextBox,
                true,
                false,
                true,
                string.Empty,
                string.Empty,
                string.Empty,
                new List<string>()
            );

        private LiteralListGenericConfig GetDefaultLiteralListGenericConfig(string genericArgumentName, LiteralParameterType literalType, ListType listType)
            => _genericConfigFactory.GetLiteralListGenericConfig
            (
                genericArgumentName,
                literalType,
                listType,
                ListParameterInputStyle.HashSetForm,
                LiteralParameterInputStyle.SingleLineTextBox,
                string.Empty,
                string.Empty,
                new List<string>(),
                new List<string>()
            );

        private ObjectGenericConfig GetDefaultObjectGenericConfig(string genericArgumentName, string objectType)
            => _genericConfigFactory.GetObjectGenericConfig
            (
                genericArgumentName,
                objectType,
                false,
                false,
                true
            );

        private ObjectListGenericConfig GetDefaultObjectListGenericConfig(string genericArgumentName, string objectType = MiscellaneousConstants.DEFAULT_OBJECT_TYPE, ListType listType = ListType.GenericList)
            => _genericConfigFactory.GetObjectListGenericConfig
            (
                genericArgumentName,
                objectType,
                listType,
                ListParameterInputStyle.HashSetForm
            );
    }
}

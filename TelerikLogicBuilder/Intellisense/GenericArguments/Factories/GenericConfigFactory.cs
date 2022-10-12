using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories
{
    internal class GenericConfigFactory : IGenericConfigFactory
    {
        private readonly Func<string, LiteralParameterType, LiteralParameterInputStyle, bool, bool, bool, string, string, string, List<string>, LiteralGenericConfig> _getLiteralGenericConfig;
        private readonly Func<string, LiteralParameterType, ListType, ListParameterInputStyle, LiteralParameterInputStyle, string, string, List<string>, List<string>, LiteralListGenericConfig> _getLiteralListGenericConfig;
        private readonly Func<string, string, bool, bool, bool, ObjectGenericConfig> _getObjectGenericConfig;
        private readonly Func<string, string, ListType, ListParameterInputStyle, ObjectListGenericConfig> _getObjectListGenericConfig;

        public GenericConfigFactory(
            Func<string, LiteralParameterType, LiteralParameterInputStyle, bool, bool, bool, string, string, string, List<string>, LiteralGenericConfig> getLiteralGenericConfig, 
            Func<string, LiteralParameterType, ListType, ListParameterInputStyle, LiteralParameterInputStyle, string, string, List<string>, List<string>, LiteralListGenericConfig> getLiteralListGenericConfig,
            Func<string, string, bool, bool, bool, ObjectGenericConfig> getObjectGenericConfig,
            Func<string, string, ListType, ListParameterInputStyle, ObjectListGenericConfig> getObjectListGenericConfig)
        {
            _getLiteralGenericConfig = getLiteralGenericConfig;
            _getLiteralListGenericConfig = getLiteralListGenericConfig;
            _getObjectGenericConfig = getObjectGenericConfig;
            _getObjectListGenericConfig = getObjectListGenericConfig;
        }

        public LiteralGenericConfig GetLiteralGenericConfig(string genericArgumentName, LiteralParameterType literalType, LiteralParameterInputStyle control, bool useForEquality, bool useForHashCode, bool useForToString, string propertySource, string propertySourceParameter, string defaultValue, List<string> domain) 
            => _getLiteralGenericConfig
            (
                genericArgumentName,
                literalType,
                control,
                useForEquality,
                useForHashCode,
                useForToString,
                propertySource,
                propertySourceParameter,
                defaultValue,
                domain
            );

        public LiteralListGenericConfig GetLiteralListGenericConfig(string genericArgumentName, LiteralParameterType literalType, ListType listType, ListParameterInputStyle control, LiteralParameterInputStyle elementControl, string propertySource, string propertySourceParameter, List<string> defaultValues, List<string> domain) 
            => _getLiteralListGenericConfig
            (
                genericArgumentName,
                literalType,
                listType,
                control,
                elementControl,
                propertySource,
                propertySourceParameter,
                defaultValues,
                domain
            );

        public ObjectGenericConfig GetObjectGenericConfig(string genericArgumentName, string objectType, bool useForEquality, bool useForHashCode, bool useForToString) 
            => _getObjectGenericConfig
            (
                genericArgumentName,
                objectType,
                useForEquality,
                useForHashCode,
                useForToString
            );

        public ObjectListGenericConfig GetObjectListGenericConfig(string genericArgumentName, string objectType, ListType listType, ListParameterInputStyle control) 
            => _getObjectListGenericConfig
            (
                genericArgumentName,
                objectType,
                listType,
                control
            );
    }
}

using ABIS.LogicBuilder.FlowBuilder.Enums;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories
{
    internal class ParameterFactory : IParameterFactory
    {
        private readonly Func<string, bool, string, string, GenericParameter> _getGenericParameter;
        private readonly Func<string, bool, string, string, ListType, ListParameterInputStyle, ListOfGenericsParameter> _getListOfGenericsParameter;
        private readonly Func<string, bool, string, LiteralParameterType, ListType, ListParameterInputStyle, LiteralParameterInputStyle, string, string, List<string>, char[], List<string>, ListOfLiteralsParameter> _getListOfLiteralsParameter;
        private readonly Func<string, bool, string, string, ListType, ListParameterInputStyle, ListOfObjectsParameter> _getListOfObjectsParameter;
        private readonly Func<string, bool, string, LiteralParameterType, LiteralParameterInputStyle, bool, bool, bool, string, string, string, List<string>, LiteralParameter> _getLiteralParameter;
        private readonly Func<string, bool, string, string, bool, bool, bool, ObjectParameter> _getObjectParameter;

        public ParameterFactory(
            Func<string, bool, string, string, GenericParameter> getGenericParameter,
            Func<string, bool, string, string, ListType, ListParameterInputStyle, ListOfGenericsParameter> getListOfGenericsParameter,
            Func<string, bool, string, LiteralParameterType, ListType, ListParameterInputStyle, LiteralParameterInputStyle, string, string, List<string>, char[], List<string>, ListOfLiteralsParameter> getListOfLiteralsParameter,
            Func<string, bool, string, string, ListType, ListParameterInputStyle, ListOfObjectsParameter> getListOfObjectsParameter,
            Func<string, bool, string, LiteralParameterType, LiteralParameterInputStyle, bool, bool, bool, string, string, string, List<string>, LiteralParameter> getLiteralParameter,
            Func<string, bool, string, string, bool, bool, bool, ObjectParameter> getObjectParameter)
        {
            _getGenericParameter = getGenericParameter;
            _getListOfGenericsParameter = getListOfGenericsParameter;
            _getListOfLiteralsParameter = getListOfLiteralsParameter;
            _getListOfObjectsParameter = getListOfObjectsParameter;
            _getLiteralParameter = getLiteralParameter;
            _getObjectParameter = getObjectParameter;
        }

        public GenericParameter GetGenericParameter(string name, bool isOptional, string comments, string genericArgumentName)
            => _getGenericParameter
            (
                name,
                isOptional,
                comments,
                genericArgumentName
            );

        public ListOfGenericsParameter GetListOfGenericsParameter(string name, bool isOptional, string comments, string genericArgumentName, ListType listType, ListParameterInputStyle control)
            => _getListOfGenericsParameter
            (
                name,
                isOptional,
                comments,
                genericArgumentName,
                listType,
                control
            );

        public ListOfLiteralsParameter GetListOfLiteralsParameter(string name, bool isOptional, string comments, LiteralParameterType literalType, ListType listType, ListParameterInputStyle control, LiteralParameterInputStyle elementControl, string fieldSource, string fieldSourceProperty, List<string> defaultValues, char[] defaultValuesDelimiters, List<string> domain)
            => _getListOfLiteralsParameter
            (
                name,
                isOptional,
                comments,
                literalType,
                listType,
                control,
                elementControl,
                fieldSource,
                fieldSourceProperty,
                defaultValues,
                defaultValuesDelimiters,
                domain
            );

        public ListOfObjectsParameter GetListOfObjectsParameter(string name, bool isOptional, string comments, string objectType, ListType listType, ListParameterInputStyle control)
            => _getListOfObjectsParameter
            (
                name,
                isOptional,
                comments,
                objectType,
                listType,
                control
            );

        public LiteralParameter GetLiteralParameter(string name, bool isOptional, string comments, LiteralParameterType literalType, LiteralParameterInputStyle control, bool useForEquality, bool useForHashCode, bool useForToString, string propertySource, string propertySourceParameter, string defaultValue, List<string> domain)
            => _getLiteralParameter
            (
                name,
                isOptional,
                comments,
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

        public ObjectParameter GetObjectParameter(string name, bool isOptional, string comments, string objectType, bool useForEquality, bool useForHashCode, bool useForToString)
            => _getObjectParameter
            (
                name,
                isOptional,
                comments,
                objectType,
                useForEquality,
                useForHashCode,
                useForToString
            );
    }
}

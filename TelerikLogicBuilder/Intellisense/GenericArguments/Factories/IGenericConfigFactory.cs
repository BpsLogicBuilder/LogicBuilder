using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories
{
    internal interface IGenericConfigFactory
    {
        LiteralGenericConfig GetLiteralGenericConfig(string genericArgumentName,
            LiteralParameterType literalType,
            LiteralParameterInputStyle control,
            bool useForEquality,
            bool useForHashCode,
            bool useForToString,
            string propertySource,
            string propertySourceParameter,
            string defaultValue,
            List<string> domain);

        LiteralListGenericConfig GetLiteralListGenericConfig(string genericArgumentName,
            LiteralParameterType literalType,
            ListType listType,
            ListParameterInputStyle control,
            LiteralParameterInputStyle elementControl,
            string propertySource,
            string propertySourceParameter,
            List<string> defaultValues,
            List<string> domain);

        ObjectGenericConfig GetObjectGenericConfig(string genericArgumentName,
            string objectType,
            bool useForEquality,
            bool useForHashCode,
            bool useForToString);

        ObjectListGenericConfig GetObjectListGenericConfig(string genericArgumentName,
            string objectType,
            ListType listType,
            ListParameterInputStyle control);
    }
}

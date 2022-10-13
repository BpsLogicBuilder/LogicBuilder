using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories
{
    internal interface IParameterFactory
    {
        GenericParameter GetGenericParameter(string name,
            bool isOptional,
            string comments,
            string genericArgumentName);

        ListOfGenericsParameter GetListOfGenericsParameter(string name,
            bool isOptional,
            string comments,
            string genericArgumentName,
            ListType listType,
            ListParameterInputStyle control);

        ListOfLiteralsParameter GetListOfLiteralsParameter(string name,
            bool isOptional,
            string comments,
            LiteralParameterType literalType,
            ListType listType,
            ListParameterInputStyle control,
            LiteralParameterInputStyle elementControl,
            string fieldSource,
            string fieldSourceProperty,
            List<string> defaultValues,
            char[] defaultValuesDelimiters,
            List<string> domain);

        ListOfObjectsParameter GetListOfObjectsParameter(string name,
            bool isOptional,
            string comments,
            string objectType,
            ListType listType,
            ListParameterInputStyle control);

        LiteralParameter GetLiteralParameter(string name,
            bool isOptional,
            string comments,
            LiteralParameterType literalType,
            LiteralParameterInputStyle control,
            bool useForEquality,
            bool useForHashCode,
            bool useForToString,
            string propertySource,
            string propertySourceParameter,
            string defaultValue,
            List<string> domain);

        ObjectParameter GetObjectParameter(string name,
            bool isOptional,
            string comments,
            string objectType,
            bool useForEquality,
            bool useForHashCode,
            bool useForToString);
    }
}

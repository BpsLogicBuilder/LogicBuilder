using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseParameterFactoryServices
    {
        internal static IServiceCollection AddIntellisenseParameterFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, bool, string, string, GenericParameter>>
                (
                    provider =>
                    (name, isOptional, comments, genericArgumentName) => new GenericParameter
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name, 
                        isOptional, 
                        comments, 
                        genericArgumentName
                    )
                )
                .AddTransient<Func<string, bool, string, string, ListType, ListParameterInputStyle, ListOfGenericsParameter>>
                (
                    provider =>
                    (name, isOptional, comments, genericArgumentName, listType, control) => new ListOfGenericsParameter
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        isOptional,
                        comments,
                        genericArgumentName,
                        listType, 
                        control
                    )
                )
                .AddTransient<Func<string, bool, string, LiteralParameterType, ListType, ListParameterInputStyle, LiteralParameterInputStyle, string, string, List<string>, char[], List<string>, ListOfLiteralsParameter>>
                (
                    provider =>
                    (name, isOptional, comments, literalType, listType, control, elementControl, fieldSource, fieldSourceProperty, defaultValues, defaultValuesDelimiters, domain) => new ListOfLiteralsParameter
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
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
                    )
                )
                .AddTransient<Func<string, bool, string, string, ListType, ListParameterInputStyle, ListOfObjectsParameter>>
                (
                    provider =>
                    (name, isOptional, comments, objectType, listType, control) => new ListOfObjectsParameter
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        isOptional,
                        comments,
                        objectType,
                        listType,
                        control
                    )
                )
                .AddTransient<Func<string, bool, string, LiteralParameterType, LiteralParameterInputStyle, bool, bool, bool, string, string, string, List<string>, LiteralParameter>>
                (
                    provider =>
                    (name, isOptional, comments, literalType, control, useForEquality, useForHashCode, useForToString, propertySource, propertySourceParameter, defaultValue, domain) => new LiteralParameter
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
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
                    )
                )
                .AddTransient<Func<string, bool, string, string, bool, bool, bool, ObjectParameter>>
                (
                    provider =>
                    (name, isOptional, comments, objectType, useForEquality, useForHashCode, useForToString) => new ObjectParameter
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        isOptional,
                        comments,
                        objectType,
                        useForEquality,
                        useForHashCode,
                        useForToString
                    )
                )
                .AddTransient<IParameterFactory, ParameterFactory>();
        }
    }
}

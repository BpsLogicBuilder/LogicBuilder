using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseGenericConfigFactoryServices
    {
        internal static IServiceCollection AddIntellisenseGenericConfigFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, LiteralParameterType, LiteralParameterInputStyle, bool, bool, bool, string, string, string, List<string>, LiteralGenericConfig>>
                (
                    provider =>
                    (genericArgumentName, literalType, control, useForEquality, useForHashCode, useForToString, propertySource, propertySourceParameter, defaultValue, domain) => new LiteralGenericConfig
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
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
                    )
                )
                .AddTransient<Func<string, LiteralParameterType, ListType, ListParameterInputStyle, LiteralParameterInputStyle, string, string, List<string>, List<string>, LiteralListGenericConfig>>
                (
                    provider =>
                    (genericArgumentName, literalType, listType, control, elementControl, propertySource, propertySourceParameter, defaultValue, domain) => new LiteralListGenericConfig
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        genericArgumentName, 
                        literalType, 
                        listType, 
                        control, 
                        elementControl, 
                        propertySource, 
                        propertySourceParameter, 
                        defaultValue, 
                        domain
                    )
                )
                .AddTransient<Func<string, string, bool, bool, bool, ObjectGenericConfig>>
                (
                    provider =>
                    (genericArgumentName, objectType, useForEquality, useForHashCode, useForToString) => new ObjectGenericConfig
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        genericArgumentName,
                        objectType,
                        useForEquality,
                        useForHashCode,
                        useForToString
                    )
                )
                .AddTransient<Func<string, string, ListType, ListParameterInputStyle, ObjectListGenericConfig>>
                (
                    provider =>
                    (genericArgumentName, objectType, listType, control) => new ObjectListGenericConfig
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        genericArgumentName,
                        objectType, 
                        listType, 
                        control
                    )
                )
                .AddTransient<IGenericConfigFactory, GenericConfigFactory>();
        }
    }
}

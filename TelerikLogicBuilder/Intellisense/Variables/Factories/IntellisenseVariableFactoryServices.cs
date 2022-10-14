using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseVariableFactoryServices
    {
        internal static IServiceCollection AddIntellisenseVariableFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, LiteralVariableType, ListType, ListVariableInputStyle, LiteralVariableInputStyle, string, List<string>, List<string>, ListOfLiteralsVariable>>
                (
                    provider =>
                    (name, memberName, variableCategory, castVariableAs, typeName, referenceName, referenceDefinition, castReferenceAs, referenceCategory, comments, literalType, listType, control, elementControl, propertySource, defaultValue, domain) => new ListOfLiteralsVariable
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        memberName,
                        variableCategory,
                        castVariableAs,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        comments,
                        literalType,
                        listType,
                        control,
                        elementControl,
                        propertySource,
                        defaultValue,
                        domain
                    )
                )
                .AddTransient<Func<MemberInfo, Type, ListOfLiteralsVariableNodeInfo>>
                (
                    provider =>
                    (mInfo, memberType) => new ListOfLiteralsVariableNodeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<IMemberAttributeReader>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IVariableFactory>(),
                        mInfo, 
                        memberType
                    )
                )
                .AddTransient<Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, string, ListType, ListVariableInputStyle, ListOfObjectsVariable>>
                (
                    provider =>
                    (name, memberName, variableCategory, castVariableAs, typeName, referenceName, referenceDefinition, castReferenceAs, referenceCategory, comments, objectType, listType, control) => new ListOfObjectsVariable
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        memberName,
                        variableCategory,
                        castVariableAs,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        comments,
                        objectType,
                        listType,
                        control
                    )
                )
                .AddTransient<Func<MemberInfo, Type, ListOfObjectsVariableNodeInfo>>
                (
                    provider =>
                    (mInfo, memberType) => new ListOfObjectsVariableNodeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<IMemberAttributeReader>(),
                        provider.GetRequiredService<IVariableFactory>(),
                        mInfo,
                        memberType
                    )
                )
                .AddTransient<Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, LiteralVariableType, LiteralVariableInputStyle, string, string, List<string>,  LiteralVariable>>
                (
                    provider =>
                    (name, memberName, variableCategory, castVariableAs, typeName, referenceName, referenceDefinition, castReferenceAs, referenceCategory, comments, literalType, control, propertySource, defaultValue, domain) => new LiteralVariable
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        memberName,
                        variableCategory,
                        castVariableAs,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        comments,
                        literalType,
                        control,
                        propertySource,
                        defaultValue,
                        domain
                    )
                )
                .AddTransient<Func<MemberInfo, Type, LiteralVariableNodeInfo>>
                (
                    provider =>
                    (mInfo, memberType) => new LiteralVariableNodeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IMemberAttributeReader>(),
                        provider.GetRequiredService<IVariableFactory>(),
                        mInfo,
                        memberType
                    )
                )
                .AddTransient<Func<string, string, VariableCategory, string, string, string, string, string, ReferenceCategories, string, string, ObjectVariable>>
                (
                    provider =>
                    (name, memberName, variableCategory, castVariableAs, typeName, referenceName, referenceDefinition, castReferenceAs, referenceCategory, comments, objectType) => new ObjectVariable
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        memberName,
                        variableCategory,
                        castVariableAs,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        comments,
                        objectType
                    )
                )
                .AddTransient<Func<MemberInfo, Type, ObjectVariableNodeInfo>>
                (
                    provider =>
                    (mInfo, memberType) => new ObjectVariableNodeInfo
                    (
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<IMemberAttributeReader>(),
                        provider.GetRequiredService<IVariableFactory>(),
                        mInfo,
                        memberType
                    )
                )
                .AddTransient<IVariableFactory, VariableFactory>()
                .AddTransient<IVariableNodeInfoFactory, VariableNodeInfoFactory>();
        }
    }
}

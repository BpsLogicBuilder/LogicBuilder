using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FunctionFactoryServices
    {
        internal static IServiceCollection AddFunctionFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string, FunctionCategories, string, string, string, string, ReferenceCategories, ParametersLayout, List<ParameterBase>, List<string>, ReturnTypeBase, string, Function>>
                (
                    provider =>
                    (name, memberName, functionCategory, typeName, referenceName, referenceDefinition, castReferenceAs, referenceCategory, parametersLayout, parameters, genericArguments, returnType, summary) => new Function
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        memberName,
                        functionCategory,
                        typeName,
                        referenceName,
                        referenceDefinition,
                        castReferenceAs,
                        referenceCategory,
                        parametersLayout,
                        parameters,
                        genericArguments,
                        returnType,
                        summary
                    )
                )
                .AddTransient<IFunctionFactory, FunctionFactory>()
                .AddTransient<Func<MethodInfo, FunctionNodeInfo>>
                (
                    provider =>
                    mInfo => new FunctionNodeInfo
                    (
                        provider.GetRequiredService<IFunctionFactory>(),
                        provider.GetRequiredService<IMemberAttributeReader>(),
                        provider.GetRequiredService<IParametersManager>(),
                        provider.GetRequiredService<IReturnTypeManager>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        mInfo
                    )
                )
                .AddTransient<IFunctionNodeInfoFactory, FunctionNodeInfoFactory>()
                .AddTransient<Func<string, GenericReturnType>>
                (
                    provider =>
                    genericArgumentName => new GenericReturnType
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        genericArgumentName
                    )
                )
                .AddTransient<Func<MethodInfo, GenericReturnTypeInfo>>
                (
                    provider =>
                    mInfo => new GenericReturnTypeInfo
                    (
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        mInfo
                    )
                )
                .AddTransient<Func<string, ListType, ListOfGenericsReturnType>>
                (
                    provider =>
                    (genericArgumentName, listType) => new ListOfGenericsReturnType
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        genericArgumentName,
                        listType
                    )
                )
                .AddTransient<Func<MethodInfo, ListOfGenericsReturnTypeInfo>>
                (
                    provider =>
                    mInfo => new ListOfGenericsReturnTypeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        mInfo
                    )
                )
                .AddTransient<Func<LiteralFunctionReturnType, ListType, ListOfLiteralsReturnType>>
                (
                    provider =>
                    (underlyingLiteralType, listType) => new ListOfLiteralsReturnType
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        underlyingLiteralType,
                        listType
                    )
                )
                .AddTransient<Func<MethodInfo, ListOfLiteralsReturnTypeInfo>>
                (
                    provider =>
                    mInfo => new ListOfLiteralsReturnTypeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        mInfo
                    )
                )
                .AddTransient<Func<string, ListType, ListOfObjectsReturnType>>
                (
                    provider =>
                    (objectType, listType) => new ListOfObjectsReturnType
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectType,
                        listType
                    )
                )
                .AddTransient<Func<MethodInfo, ListOfObjectsReturnTypeInfo>>
                (
                    provider =>
                    mInfo => new ListOfObjectsReturnTypeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        mInfo
                    )
                )
                .AddTransient<Func<LiteralFunctionReturnType, LiteralReturnType>>
                (
                    provider =>
                    literalType => new LiteralReturnType
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        literalType
                    )
                )
                .AddTransient<Func<MethodInfo, LiteralReturnTypeInfo>>
                (
                    provider =>
                    mInfo => new LiteralReturnTypeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        mInfo
                    )
                )
                .AddTransient<Func<string, ObjectReturnType>>
                (
                    provider =>
                    objectType => new ObjectReturnType
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectType
                    )
                )
                .AddTransient<Func<MethodInfo, ObjectReturnTypeInfo>>
                (
                    provider =>
                    mInfo => new ObjectReturnTypeInfo
                    (
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        mInfo
                    )
                )
                .AddTransient<IReturnTypeInfoFactory, ReturnTypeInfoFactory>()
                .AddTransient<IReturnTypeFactory, ReturnTypeFactory>();
        }
    }
}

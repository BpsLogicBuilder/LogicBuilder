using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseReturnTypeFactoryServices
    {
        internal static IServiceCollection AddIntellisenseReturnTypeFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, GenericReturnType>>
                (
                    provider =>
                    genericArgumentName => new GenericReturnType
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        genericArgumentName
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
                .AddTransient<Func<string, ObjectReturnType>>
                (
                    provider =>
                    objectType => new ObjectReturnType
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectType
                    )
                )
                .AddTransient<IReturnTypeFactory, ReturnTypeFactory>();
        }
    }
}

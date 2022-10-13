using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseReturnTypeInfoFactoryServices
    {
        internal static IServiceCollection AddIntellisenseReturnTypeInfoFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<MethodInfo, GenericReturnTypeInfo>>
                (
                    provider =>
                    mInfo => new GenericReturnTypeInfo
                    (
                        provider.GetRequiredService<IReturnTypeFactory>(),
                        mInfo
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
                .AddTransient<IReturnTypeInfoFactory, ReturnTypeInfoFactory>();
        }
    }
}

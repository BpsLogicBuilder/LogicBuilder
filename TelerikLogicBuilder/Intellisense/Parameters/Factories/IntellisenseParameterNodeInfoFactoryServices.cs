using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseParameterNodeInfoFactoryServices
    {
        internal static IServiceCollection AddIntellisenseParameterNodeInfoFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<ParameterInfo, GenericParameterNodeInfo>>
                (
                    provider =>
                    pInfo => new GenericParameterNodeInfo
                    (
                        provider.GetRequiredService<IParameterAttributeReader>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        pInfo
                    )
                )
                .AddTransient<Func<ParameterInfo, ListOfGenericsParameterNodeInfo>>
                (
                    provider =>
                    pInfo => new ListOfGenericsParameterNodeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IParameterAttributeReader>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        pInfo
                    )
                )
                .AddTransient<Func<ParameterInfo, ListOfLiteralsParameterNodeInfo>>
                (
                    provider =>
                    pInfo => new ListOfLiteralsParameterNodeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IParameterAttributeReader>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        pInfo
                    )
                )
                .AddTransient<Func<ParameterInfo, ListOfObjectsParameterNodeInfo>>
                (
                    provider =>
                    pInfo => new ListOfObjectsParameterNodeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IParameterAttributeReader>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        pInfo
                    )
                )
                .AddTransient<Func<ParameterInfo, LiteralParameterNodeInfo>>
                (
                    provider =>
                    pInfo => new LiteralParameterNodeInfo
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IParameterAttributeReader>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        pInfo
                    )
                )
                .AddTransient<Func<ParameterInfo, ObjectParameterNodeInfo>>
                (
                    provider =>
                    pInfo => new ObjectParameterNodeInfo
                    (
                        provider.GetRequiredService<IParameterAttributeReader>(),
                        provider.GetRequiredService<IParameterFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        pInfo
                    )
                )
                .AddTransient<IParameterNodeInfoFactory, ParameterNodeInfoFactory>();
        }
    }
}

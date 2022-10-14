using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseConstructorFactoryServices
    {
        internal static IServiceCollection AddIntellisenseConstructorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<Dictionary<string, Constructor>, IChildConstructorFinder>>
                (
                    provider =>
                    existingConstructors => new ChildConstructorFinder
                    (
                        provider.GetRequiredService<IConstructorManager>(),
                        provider.GetRequiredService<IParametersManager>(),
                        provider.GetRequiredService<IReflectionHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IMemberAttributeReader>(),
                        existingConstructors
                    )
                )
                .AddTransient<Func<string, string, List<ParameterBase>, List<string>, string, Constructor>>
                (
                    provider =>
                    (name, typeName, parameters, genericArguments, summary) => new Constructor
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name, 
                        typeName, 
                        parameters, 
                        genericArguments, 
                        summary
                    )
                )
                .AddTransient<IChildConstructorFinderFactory, ChildConstructorFinderFactory>()
                .AddTransient<IConstructorFactory, ConstructorFactory>();
        }
    }
}

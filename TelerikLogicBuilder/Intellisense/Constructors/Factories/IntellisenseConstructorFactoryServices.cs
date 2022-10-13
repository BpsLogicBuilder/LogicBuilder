using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseConstructorFactoryServices
    {
        internal static IServiceCollection AddIntellisenseConstructorFactories(this IServiceCollection services)
        {
            return services
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
                .AddTransient<IConstructorFactory, ConstructorFactory>();
        }
    }
}

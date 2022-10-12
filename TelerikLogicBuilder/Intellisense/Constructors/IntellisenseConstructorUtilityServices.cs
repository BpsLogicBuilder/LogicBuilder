using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseConstructorUtilityServices
    {
        internal static IServiceCollection AddIntellisenseConstructorUtilities(this IServiceCollection services)
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

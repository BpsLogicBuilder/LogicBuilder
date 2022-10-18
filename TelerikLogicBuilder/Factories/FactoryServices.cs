using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FactoryServices
    {
        internal static IServiceCollection AddFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, Page, Shape, List<ResultMessage>, IResultMessageHelper>>
                (
                    provider =>
                    (sourceFile, page, shape, validationErrors) => new ResultMessageHelper
                    (
                        provider.GetRequiredService<IResultMessageBuilder>(),
                        provider.GetRequiredService<IVisioFileSourceFactory>(),
                        sourceFile,
                        page,
                        shape,
                        validationErrors
                    )
                )
                .AddTransient<IResultMessageHelperFactory, ResultMessageHelperFactory>();
        }
    }
}
